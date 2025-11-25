using XNode;
using UnityEngine;

public abstract class BaseNode : Node {

    // Every dialogue node has an input
    [Input(backingValue = ShowBackingValue.Never)]
    public BaseNode input;

    // Must override GetValue even if unused
    public override object GetValue(NodePort port) {
        return null;
    }

    // Helper to get the next connected node
    public BaseNode GetNextNode(string portName = "output") {
        NodePort port = GetOutputPort(portName);
        if (port != null && port.Connection != null) {
            return port.Connection.node as BaseNode;
        }
        return null;
    }
}
