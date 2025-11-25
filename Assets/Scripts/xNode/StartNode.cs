using XNode;
using UnityEngine;

[NodeTint("#4CAF50")]
public class StartNode : BaseNode {

    [Output] public BaseNode output;

    public override object GetValue(NodePort port) {
        return null;
    }
}
