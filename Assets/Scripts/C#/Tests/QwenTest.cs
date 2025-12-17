using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QwenTest : MonoBehaviour
{
    public QwenRuntime qwen;

    IEnumerator Start()
    {
        qwen.CompilePersona(
            "Aiko",
            "calm and concise",
            new string[] { "Never break character" }
        );

        yield return qwen.WarmUp();

        yield return qwen.Generate(
            "Say hello in one short sentence.",
            resp => Debug.Log("LLM: " + resp)
        );
    }

}
