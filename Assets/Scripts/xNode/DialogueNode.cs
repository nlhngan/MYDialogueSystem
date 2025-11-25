using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/Dialogue Node")]
public class DialogueNode : BaseNode {
	[Input(backingValue = ShowBackingValue.Never)] public int input; // value can't be edited in graph editor
	[Output(backingValue = ShowBackingValue.Never)] public int output;

	[Header("Content")]
	public Sprite portrait;
	public string speakerName;
	[TextArea(2,6)] public string dialogueLine;

	[Header("Branching")]
	[Tooltip("Option text shown to the player. If empty, the node is considered linear")]
	public string[] choices; // length N
	[Tooltip("Reference to the next node for each choice. For linear nodes, set nextNodes[0] to the next node")]
	public BaseNode[] nextNodes; // length N (must match choices), 1 for linear
	
	public override string GetNodeTypeString()
    {
        return "DialogueNode";
    }

	public override string GetDialogueText()
    {
        return dialogueLine;
    }

	public override string GetSpeakerName()
    {
        return speakerName;
    }

	public override Sprite GetPortrait()
    {
        return portrait;
    }

	public override object GetValue(NodePort port) { return null; }
}