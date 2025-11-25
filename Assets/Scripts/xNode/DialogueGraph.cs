using XNode;

[CreateAssetMenu(fileName = "DialogueGraph", menuName = "Dialogue/Dialogue Graph")]
public class DialogueGraph : NodeGraph {
    public StartNode GetStartNode() {
        foreach (var node in nodes) {
            if (node is StartNode start) return start;
        }
        return null;
    }
}
