using XNode;
using UnityEngine;

[NodeTint("#2196F3")]
public class DialogueNode : BaseNode {

    [TextArea(2, 5)]
    public string text;

    [Output] 
    public BaseNode next;

    public override object GetValue(NodePort port) {
        return null;
    }
}
