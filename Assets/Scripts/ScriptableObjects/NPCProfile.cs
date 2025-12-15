using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string npcName;
    [TextArea(6,12)] public string personaJSON;
}
