using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTestStarter : MonoBehaviour
{
    public NodeParser parser;
    public DialogueGraph graph;
    public NPCProfile npc;
    public DialogueContext context;

    void Start()
    {
        parser.StartDialogue(graph,npc,context);
    }
}
