using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class QwenRuntime : MonoBehaviour {
    private string llamacpp = "http://localhost:8080/v1/chat/completions";

    [Header("Generation config")]
    public float temperature = 0.7f;
    [Range(16,256)] public int maxTokens = 32;
    [Range(512,4096)] public int contextSize = 512;
    // cached n compiled persona
    private string compiledPersonaPrompt = "";
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
            sb.AppendLine("stay in character.");
            sb.AppendLine("do NOT wrap responses in quotes or include system tokens.");

            for (int i = 0; i<constraints.Length; ++i)
            {
                sb.AppendLine($"- {constraints[i]}");
            }
        }
        compiledPersonaPrompt = sb.ToString();
        Debug.Log("[LLM] persona compiled");
    }

    public IEnumerator WarmUp()
    {
        if (isWarmedUp) yield break;
        Debug.Log("[LLM] warm up start");
        yield return Generate("ping", _ => {isWarmedUp=true;});
        Debug.Log("[LLM] warm up done");
    }

#region internals
    public IEnumerator Generate(
        string userInput,
        Action<string> callback)
    {
        var payload = new ChatRequest{
            max_tokens = maxTokens,
            messages = new[]
            {
                new ChatMessage {role="system", content=compiledPersonaPrompt},
                new ChatMessage {role="user", content=userInput}
            },
            stop = new[]
            {
                "<|im_end|>",
                "<|im_start|>",
                "<p>",
                "</p>",
                "\nUSER:",
                "\nASSISTANT:"
            },
            temperature = temperature
        };

        string json = JsonUtility.ToJson(payload);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(llamacpp, "POST")) {
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.timeout = 15;
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Accept", "application/json");
            req.SetRequestHeader("Authorization", "Bearer dummy"); // remove this later


            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success) {
                Debug.LogError("error: " + req.error);
                callback?.Invoke("[LLM Error]");
                yield break;
            } 
            var result = JsonUtility.FromJson<ChatResponse>(req.downloadHandler.text);
            if (result.choices == null || result.choices.Length == 0)
            {
                callback?.Invoke("[LLM Empty Response]");
                yield break;
            }
            callback?.Invoke(CleanLLMOutput(result.choices[0].message.content));
        }
    }

    string CleanLLMOutput(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        text = text.Trim();

        if (text.Length >= 2 && text[0] == '"' && text[^1] == '"')
            text = text.Substring(1, text.Length - 2);
        text = text.Replace("<|im_end|>", "")
                .Replace("<|im_start|>", "")
                .Replace("<p>","")
                .Replace("</p>","")
                .Trim();
        return text;
    }


#endregion

#region data

    [Serializable]
    private class ChatRequest
    {
        public int max_tokens;
        public float temperature;
        public ChatMessage[] messages;
        public string[] stop; 
    }

    [Serializable]
    private class ChatMessage
    {
        public string role;
        public string content;
    }

    [Serializable] private class ChatResponse {public Choice[] choices;}
    [Serializable] private class Choice {public Message message;}
    [Serializable] private class Message {public string content;}
}
#endregion