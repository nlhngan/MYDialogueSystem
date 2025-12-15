using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseNode : Node {

	[SerializeField, HideInInspector] private string _guid;
	public string GUID
    {
        get
        {
            if (string.IsNullOrEmpty(_guid)) _guid = System.Guid.NewGuid().ToString();
			return _guid;
        }
    }

	public virtual string GetNodeTypeString() {return "BaseNode";}

	// content helpers
	public virtual string GetDialogueText() => null;
	public virtual string GetSpeakerName() => null;
	public virtual Sprite GetPortrait() => null;
	public virtual string GetPersonaJSON() => null;
	public virtual bool UsesLLM() => false;

	// called when node becomes active
	public virtual void OnEnterNode() {}

	//called when node is left
	public virtual void OnExitNode() {}
	public override object GetValue(NodePort port) { return null; }
}