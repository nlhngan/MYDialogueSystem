using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/Start")]
public class StartNode : BaseNode {
	[Output] public BaseNode firstNode;

	public override string GetNodeTypeString() {return "StartNode";}
	public BaseNode GetFirst()
    {
        NodePort port = GetOutputPort("firstNode");
		if (!port.IsConnected) return null;
		return port.Connection.node as BaseNode;
    }

	public override void OnEnterNode() {}
	public override object GetValue(NodePort node) { return null; }

}