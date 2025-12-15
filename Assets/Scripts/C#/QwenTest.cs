using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QwenTest : MonoBehaviour
{
    public QwenRuntime qwen;

    void Start()
    {
        Debug.Log("wait");
        string systemTemplate =
            "You are an NPC in a Unity dialogue system. Always speak as the character persona.";
        string personaJson =
        @"{
            ""name"": ""Ragnar"",
            ""role"": ""Blacksmith"",
            ""traits"": [""gruff"", ""honest""],
            ""speech_style"": ""short, direct"",
            ""knowledge"": [""weapons"", ""mines""]
        }";
        string userPrompt = "The player approaches you. What do you say?";
        StartCoroutine(qwen.Generate(personaJson, 
        systemTemplate, 
        userPrompt, 
        OnQwenReply));
    }

    void OnQwenReply(string text)
    {
        Debug.Log("Qwen replied: " + text);
    }

}
