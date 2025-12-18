using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Context")]
public class DialogueContext : ScriptableObject
{
    public string npcName;
    public string location;
    public List<string> conversationHistory = new();
    public Dictionary<string,string> flags = new(); // later
}
