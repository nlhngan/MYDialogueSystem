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
	public string speakerName = "";
	[TextArea(2,6)] public string dialogueLine;

	[Header("Branching")]
	[Tooltip("Option text shown to the player. If empty, the node is considered linear")]
	public string[] choices; // length N
	[Tooltip("Reference to the next node for each choice. For linear nodes, set nextNodes[0] to the next node")]
	public BaseNode[] nextNodes; // length N (must match choices), 1 for linear
	
	//[Header("Persona metadata")]
	// [TextArea(4,10)] public string personaJSON;

	[Tooltip("Prompt for Qwen")]
	[TextArea(4,12)] public string userInput;

	[Header("LLM generation")]
	public bool useLLM = false;


	//public string[] tags; // emotion, tone, etc
	public override bool UsesLLM() => useLLM;
	public override string GetUserInput() => userInput;
	public override string GetNodeTypeString() {return "DialogueNode";}
	public override string GetDialogueText() {return dialogueLine;}
	public override string GetSpeakerName() {return speakerName;}
	public override Sprite GetPortrait() {return portrait;}
	public override object GetValue(NodePort port) { return null; }

	public BaseNode GetNext(int index)
    {
        if (nextNodes == null || nextNodes.Length == 0) return null;
		if (index < 0 || index >= nextNodes.Length) return null;
		return nextNodes[index];
    }

	public bool IsBranching()
    {
        return choices != null && choices.Length > 1;
    }

    // public string BuildQwenPrompt()
    // {
    //     return
	// 		$@"<persona>
	// 		{personaJSON}
	// 		</persona>

	// 		<context>
	// 		Speaker: {speakerName}
	// 		Line: {dialogueLine}
	// 		</context>

	// 		You are role-playing as the persona above. Respond concisely in character.";
    // }
}