using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName="Dialogue/Dialogue graph")]
public class DialogueGraph : NodeGraph { 
    // runtime current node reference
	public BaseNode current;
    public StartNode GetStartNode()
    {
        foreach (var node in nodes)
        {
            if (node is StartNode s) return s;
        }
        return null;
    }
}