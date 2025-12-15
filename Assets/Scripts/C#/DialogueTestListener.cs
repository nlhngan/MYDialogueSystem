using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTestListener : MonoBehaviour
{
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += OnStarted;
        DialogueEvents.OnLineSpoken += OnLine;
        DialogueEvents.OnChoiceSelected += OnChoice;
        DialogueEvents.OnDialogueEnded += OnEnded;
    }

    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= OnStarted;
        DialogueEvents.OnLineSpoken -= OnLine;
        DialogueEvents.OnChoiceSelected -= OnChoice;
        DialogueEvents.OnDialogueEnded -= OnEnded;
    }

    void OnStarted()
    {
        Debug.Log("[TEST] Dialogue started");
    }

    void OnLine(string line)
    {
        Debug.Log("[TEST] Line spoken: " + line);
    }

    void OnChoice(string choice)
    {
        Debug.Log("[TEST] Choice selected: " + choice);
    }

    void OnEnded()
    {
        Debug.Log("[TEST] Dialogue ended");
    }
}
