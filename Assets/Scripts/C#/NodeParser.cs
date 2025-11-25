using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using System.Linq;

public class NodeParser : MonoBehaviour {

    public DialogueGraph graph;
    private BaseNode current;

    void Start() {
        current = graph.GetStartNode();
        GoToNext();
    }

    public void GoToNext() {
        if (current == null) {
            Debug.Log("Dialogue ended.");
            return;
        }

        if (current is StartNode) {
            current = current.GetNextNode();
            GoToNext();
        }
        else if (current is DialogueNode dialogue) {
            Debug.Log("NPC says: " + dialogue.text);
            current = dialogue.GetNextNode("next");
        }
        else if (current is ChoiceNode choice) {
            Debug.Log("Reached a choice (" + choice.choices.Length + " options)");

            // For demo: auto-select the first choice
            current = choice.GetChoice(0);
        }
        else {
            Debug.Log("Unknown node type");
            current = null;
        }

        if (current != null)
            GoToNext();
    }
}
