using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueEvents
{
    public static Action<string> OnLineSpoken;
    public static Action<string> OnChoiceSelected;
    public static Action OnDialogueStarted;
    public static Action OnDialogueEnded;
}
