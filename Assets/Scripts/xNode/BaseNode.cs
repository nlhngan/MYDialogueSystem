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

	public virtual string GetNodeTypeString()
    {
        return "BaseNode";
    }

	// content helpers
	public virtual string GetDialogueText() {return null;}
	public virtual string GetSpeakerName() {return null;}
	public virtual Sprite GetPortrait() {return null;}

	// called when node becomes active
	public virtual void OnEnterNode() {}

	//called when node is left
	public virtual void OnExitNode() {}
	public override object GetValue(NodePort port) { return null; }
}