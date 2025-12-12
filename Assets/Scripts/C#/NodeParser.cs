using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

            if (b is StartNode)
            {
                StartNode s = (StartNode)b;
                graph.current = s.firstNode;
                continue;
            } 
            if (b is DialogueNode)
            {
                DialogueNode dn = (DialogueNode)b;
                // update UI
                speaker.text = string.IsNullOrEmpty(dn.GetSpeakerName())? "" : dn.GetSpeakerName();
                portrait.sprite = dn.GetPortrait();

                ClearChoiceButtons();

                // show dialogue
                // default static dialogue
                if (!dn.UsesLLM())
                {
                    yield return DisplayText(dn.GetDialogueText());
                }
                else 
                // LLM
                {
                    string llmText = null;

                    bool done = false;
                    yield return qwen.Generate(
                        dn.GetPersonaJSON(),
                        dn.systemPrompt,
                        dn.userPrompt,
                        (resp) => {llmText=resp; done=true;}
                    );
                    yield return new WaitUntil(() => done);
                    yield return DisplayText(llmText);
                }

                // branching if node has choices
                if (dn.choices != null && dn.choices.Length > 0)
                {
                    // build buttons
                    int choiceCount = dn.choices.Length;
                    for (int i = 0; i<choiceCount; i++)
                    {
                        int idx = i;
                        // string label = dn.choices[i];
                        Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
                        btn.GetComponentInChildren<Text>().text = dn.choices[i];
                        // if (btnText != null) btnText.text = label;                
                        btn.onClick.AddListener(() => OnChoiceSelected(dn, idx));
                    }
                    // yield until graph.current changes away from dn
                    yield return new WaitUntil(() => graph.current!=dn);
                } else
                {
                    // linear node; use nextNodes[0] as next if set
                    BaseNode next= null;
                    if (dn.nextNodes != null && dn.nextNodes.Length>0) next = dn.nextNodes[0];
                    // wait for click to continue or end if no next
                    if (next == null)
                    {
                        // wait for click to end dialogue then null current
                        yield return WaitForClick();
                        graph.current = null;
                    } else
                    {
                        yield return WaitForClick();
                        graph.current = next;
                    }
                }            
            } else
            {
                Debug.LogError("NodeParser: unsupported node type:"+ b.GetType().Name);
                graph.current = null;
            }

            b.OnExitNode();

            // frame yield
            yield return null;
        }

        // end dialogue
        parseCoroutine = null;
        Debug.Log("dialogue finished");
    }

    IEnumerator DisplayText(string text)
    {
        if (textRevealSpeed <= 0f) dialogueText.text = text;
        else
        {
            dialogueText.text ="";
            float delay = 1f / Mathf.Max(0.0001f, textRevealSpeed); // ??
            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(delay);
            }
        }
        
        
    }

    IEnumerator WaitForClick()
    {
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0));
        yield return new WaitUntil(()=> Input.GetMouseButtonUp(0));
    }

    void OnChoiceSelected(DialogueNode node, int index)
    {
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
