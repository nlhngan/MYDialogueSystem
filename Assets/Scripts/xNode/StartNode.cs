using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/Start")]
public class StartNode : BaseNode {
	public BaseNode firstNode;

	public override string GetNodeTypeString()
    {
        return "StartNode";
    }

	public override void OnEnterNode() {}
	public override object GetValue(NodePort node) { return null; }

}