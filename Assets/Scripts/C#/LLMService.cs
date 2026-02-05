using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LLMService : MonoBehaviour {
    private string llamacpp = "http://localhost:8080/v1/chat/completions";

    [Header("Generation config")]
    public float temperature = 0.7f;
    [Range(16,256)] public int maxTokens = 32;
    // [Range(512,4096)] public int contextSize = 512;
    // cached n compiled persona
    private string compiledPersonaPrompt = "";
    private bool isWarmedUp = false;
    public LLMService(){}

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
        yield return Generate("ping", null, _ => {isWarmedUp=true;});
        Debug.Log("[LLM] warm up done");
    }

#region internals
    public IEnumerator Generate(string userInput, Action<string> onTokenReceived, Action<string> onComplete)
    {
        var payload = new ChatRequest{
            max_tokens = maxTokens,
            stream = true,
            messages = new[] {
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
            // read incrementally
            req.downloadHandler = new DownloadHandlerBuffer();
            req.timeout = 15;
            req.SetRequestHeader("Content-Type", "application/json");

            AsyncOperation op = req.SendWebRequest();
            int lastPos = 0;
            StringBuilder fullResponse = new StringBuilder();

            while (!op.isDone)
            {
                string currentText = req.downloadHandler.text;
                if (currentText.Length > lastPos)
                {
                    string newChunk = currentText.Substring(lastPos);
                    lastPos = currentText.Length;
                    ProcessStreamChunk(newChunk,(token)=>
                    {
                        fullResponse.Append(token);
                        onTokenReceived?.Invoke(token); //update ui
                    });
                }
                yield return null;
            }

            onComplete?.Invoke(CleanLLMOutput(fullResponse.ToString()));
        }
    }

    private void ProcessStreamChunk(string chunk, Action<string> onToken)
    {
        string[] lines = chunk.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("data: ") && !line.Contains("[DONE]"))
            {
                string json = line.Substring(6);
                try
                {
                    var delta = JsonUtility.FromJson<StreamResponse>(json);
                    string content = delta.choices[0].delta.content;
                    if (!string.IsNullOrEmpty(content)) onToken(content);
                } catch {}
            }
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
        public bool stream;
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

    [Serializable] private class StreamResponse { public StreamChoice[] choices; }
    [Serializable] private class StreamChoice { public Delta delta; }
    [Serializable] private class Delta { public string content; }
}
#endregion