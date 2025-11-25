using XNode;
using UnityEngine;

[NodeTint("#FF9800")]
public class ChoiceNode : BaseNode {

    [System.Serializable]
    public class Choice {
        public string text;
        public BaseNode next;
    }

    public Choice[] choices;

    // Dynamic number of output ports
    protected override void Init() {
        base.Init();

        // Create ports for each choice
        for (int i = 0; i < choices.Length; i++) {
            AddDynamicOutput(typeof(BaseNode), ConnectionType.Override, TypeConstraint.None, "choice_" + i);
        }
    }

    public BaseNode GetChoice(int index) {
        NodePort port = GetOutputPort("choice_" + index);
        if (port != null && port.Connection != null) {
            return port.Connection.node as BaseNode;
        }
        return null;
    }

    public override object GetValue(NodePort port) {
        return null;
    }
}
