using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XNode;
using System.Linq;

public class NodeParser : MonoBehaviour
{
    #region references
    [Header("Reference")]
    public LLMService llm;
    public DialogueGraph graph;
    private NPCProfile currentNPC;
    private DialogueContext context;
    #endregion

    #region UI + settings
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
    #endregion

    #region public api
    public void StartDialogue(DialogueGraph graph, NPCProfile npc, DialogueContext context)
    {
        if (parseCoroutine != null) StopCoroutine(parseCoroutine);
        if (textCoroutine != null) StopCoroutine(textCoroutine);

        ClearChoiceButtons();
        choicesContainer.gameObject.SetActive(false);

        DialogueEvents.OnDialogueStarted?.Invoke();

        this.graph = graph;
        this.currentNPC=npc;
        this.context = context;

        llm.CompilePersona(npc.npcName, npc.speakingStyle, npc.constraints);

        StartCoroutine(StartDialogueRoutine()); 
    }
    #endregion

    #region dialoge flow
    IEnumerator StartDialogueRoutine()
    {
        yield return StartCoroutine(llm.WarmUp());
        graph.current = graph.nodes.OfType<StartNode>().FirstOrDefault();
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
                choicesContainer.gameObject.SetActive(false);

                // show dialogue
                string textToDisplay;

                // LLM
                if (dn.UsesLLM())
                {
                    textToDisplay = "[...]";
                    yield return StartCoroutine( 
                        llm.Generate(
                        dn.userInput,
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
                DialogueEvents.OnLineSpoken?.Invoke(textToDisplay);

                // branching if node has choices
                if (dn.choices != null && dn.choices.Length > 0)
                {
                    choicesContainer.gameObject.SetActive(true);
                    // build buttons
                    int choiceCount = dn.choices.Length;
                    for (int i = 0; i<choiceCount; i++)
                    {
                        int idx = i;
                        Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
                        btn.GetComponentInChildren<Text>().text = dn.choices[i];
                        btn.onClick.AddListener(() =>
                        {
                            // disable all buttons
                            foreach (Button b in choicesContainer.GetComponentsInChildren<Button>())
                                b.interactable = false;

                            OnChoiceSelected(dn, idx);
                        });
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
        DialogueEvents.OnDialogueEnded?.Invoke();
    }
    #endregion

    #region utility
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
            // add prevent interruption / display when clicked
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
        choicesContainer.gameObject.SetActive(false);

        DialogueEvents.OnChoiceSelected?.Invoke(node.choices[index]);
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
    #endregion
}
