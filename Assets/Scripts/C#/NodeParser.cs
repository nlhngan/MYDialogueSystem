using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XNode;
using System.Linq;

public class NodeParser : MonoBehaviour
{
    [Header("Reference")]
    public QwenRuntime qwen;
    public DialogueGraph graph;

    [Header("UI")]
    public Text speaker;
    public Text dialogueText;
    public Image portrait;
    public RectTransform choicesContainer;
    public Button choiceButtonPrefab;

    [Header("Settings")]
    public float textRevealSpeed = 0.0f;
    Coroutine parseCoroutine;
    Coroutine textCoroutine;

    private void Start()
    {
        // find StartNode in graph
        StartNode start = graph.nodes.OfType<StartNode>().FirstOrDefault();
        if (start == null)
        {
            Debug.LogError("NodeParser: no start node found in graph");
            return;
        }

        graph.current = start;
        parseCoroutine = StartCoroutine(ParseNodeRoutine());
    }

    IEnumerator ParseNodeRoutine()
    {
        while (graph.current != null)
        {
            BaseNode b = graph.current;
            b.OnEnterNode();

            if (b is StartNode s)
            {
                graph.current = s.firstNode;
                b.OnExitNode();
                continue;
            } 
            if (b is DialogueNode dn)
            {
                // update UI
                speaker.text = string.IsNullOrEmpty(dn.GetSpeakerName())? "" : dn.GetSpeakerName();
                portrait.sprite = dn.GetPortrait();              
                dialogueText.text ="";
                ClearChoiceButtons();

                // show dialogue
                string textToDisplay;

                // LLM
                if (dn.UsesLLM())
                {
                    textToDisplay = "[...]";
                    yield return StartCoroutine( 
                        qwen.Generate(dn.GetPersonaJSON(),
                        dn.systemPrompt,
                        dn.userPrompt,
                        resp => { if(!string.IsNullOrEmpty(resp)) textToDisplay = resp; }
                    ));
                }
                else 
                // default static dialogue
                {
                    textToDisplay = dn.GetDialogueText();
                }
                textCoroutine = StartCoroutine(DisplayText(textToDisplay));
                yield return textCoroutine;
                textCoroutine = null;

                // branching if node has choices
                if (dn.choices != null && dn.choices.Length > 0)
                {
                    // build buttons
                    int choiceCount = dn.choices.Length;
                    for (int i = 0; i<choiceCount; i++)
                    {
                        int idx = i;
                        Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
                        btn.GetComponentInChildren<Text>().text = dn.choices[i];
                        btn.onClick.AddListener(() => OnChoiceSelected(dn, idx));
                    }
                    // yield until graph.current changes away from dn
                    yield return new WaitUntil(() => graph.current!=dn);
                } 
                else
                {
                    // linear node; use nextNodes[0] as next if set
                    BaseNode next= (dn.nextNodes != null && dn.nextNodes.Length > 0) ? dn.nextNodes[0] : null;
                    yield return WaitForClick();
                    dn.OnExitNode();
                    graph.current = next;
                }            
            } else
            {
                Debug.LogError("NodeParser: unsupported node type:"+ b.GetType().Name);
                graph.current = null;
            }

            if (graph.current!=b) b.OnExitNode();

            // frame yield
            yield return null;
        }

        // end dialogue
        parseCoroutine = null;
        Debug.Log("dialogue finished");
    }

    IEnumerator DisplayText(string text)
    {       
        if (string.IsNullOrEmpty(text)) yield break;

        if (textRevealSpeed <= 0f)
        {
            dialogueText.text=text;
            yield break;
        }
        float delay = 1f / textRevealSpeed;
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }
        //yield return null;   
        
    }

    IEnumerator WaitForClick()
    {
        yield return new WaitUntil(() =>
        Input.GetMouseButtonDown(0) &&
        !EventSystem.current.IsPointerOverGameObject()
        );
        yield return new WaitUntil(()=> Input.GetMouseButtonUp(0));
    }

    void OnChoiceSelected(DialogueNode node, int index)
    {
        ClearChoiceButtons();
        node.OnExitNode();
        if (node.nextNodes == null || index < 0 || index>= node.nextNodes.Length)
        {
            Debug.LogWarning("no corresponding next node defined. ending dialogue");
            graph.current=null;
        } else
        {
            graph.current = node.nextNodes[index];
        }
    }

    void ClearChoiceButtons()
    {
        if (choicesContainer == null) return;
        for (int i = choicesContainer.childCount-1; i >= 0; i--)
        {
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }
}
