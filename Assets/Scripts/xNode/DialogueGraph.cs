using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName="Dialogue/Dialogue graph")]
public class DialogueGraph : NodeGraph { 
    // runtime current node reference
	[HideInInspector] public BaseNode current;
}