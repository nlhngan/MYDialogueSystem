using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class QwenRuntime : MonoBehaviour {

    [Header("Ollama")]
    //private string modelName = "hopephoto/Qwen3-4B-Instruct-2507_q8";
    private string modelName = "gemma2:2b-instruct-q3_K_M";
    //private string ollamaURL = "http://localhost:11434/api/generate";
    private string llamacpp = "http://<server>:8080/v1/chat/completions";

    [Header("Generation config")]
    [Range(16,256)] public int maxTokens = 32;
    [Range(512,4096)] public int contextSize = 512;
    // cached n compiled persona
    private string compiledPersonaPrompt;
    private bool isWarmedUp = false;
    public QwenRuntime(){}

    public void CompilePersona(string speakerName, string style, string[] constraints)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"you are {speakerName}.");
        if (!string.IsNullOrEmpty(style)) sb.AppendLine($"speaking style: {style}.");

        if (constraints != null && constraints.Length>0)
        {
            sb.AppendLine("constraints: ");
            for (int i = 0; i<constraints.Length; ++i)
            {
                sb.AppendLine($"{constraints[i]}, ");
            }
        }
        compiledPersonaPrompt = sb.ToString();
        Debug.Log("[LLM] persona compiled");
    }

    public IEnumerator WarmUp()
    {
        if (isWarmedUp) yield break;
        Debug.Log("[LLM] warm up start");
        yield return Generate("greeting.", _ => {isWarmedUp=true;});
        Debug.Log("[LLM] warm up done");
    }

#region internals
    public IEnumerator Generate(
        string userInput,
        Action<string> callback)
    {
        // construct final prompt
        string prompt = BuildPrompt(userInput);

        var payload = new OllamaRequest{
            model = modelName,
            prompt = prompt,
            num_predict = maxTokens,
            options = new OllamaOptions{num_ctx = contextSize}
        };

        string json = JsonUtility.ToJson(payload);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(llamacpp, "POST")) {
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.timeout = 15;
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Ollama error: " + req.error);
                callback?.Invoke("[LLM Error]");
                yield break;
            } 
            var result = JsonUtility.FromJson<OllamaResponse>(req.downloadHandler.text);
            callback?.Invoke(result.response);
        }
    }

    private string BuildPrompt(string userInput)
    {
        return
$@"SYSTEM:
{compiledPersonaPrompt}

USER: {userInput}

respond in character, briefly.";
    }

#endregion

#region data

    // [Serializable]
    // private class OllamaRequest {
    //     public string model;
    //     public string prompt;
    //     public bool stream;
    //     public int num_predict;
    //     public OllamaOptions options;
    // }

    // [Serializable] private class OllamaResponse {public string response;}
    // [Serializable] private class OllamaOptions {public int num_ctx;}
    // [Serializable] private class OllamaStreamResponse {public string response;}
    [Serializable]
    private class ChatRequest
    {
        public string model;
        public ChatMessage[] messages;
        public int max_tokens;
    }

    [Serializable]
    private class ChatMessage
    {
        public string role;
        public string content;
    }

}
#endregion