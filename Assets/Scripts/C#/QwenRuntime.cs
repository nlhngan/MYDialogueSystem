using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class QwenRuntime : MonoBehaviour {

    [Header("Ollama")]
    public string modelName = "hopephoto/Qwen3-4B-Instruct-2507_q8";
    public string ollamaURL = "http://localhost:11434/api/generate";
    public QwenRuntime(){}

    public IEnumerator Generate(
        string personaJSON,
        string systemTemplate,
        string userTemplate,
        Action<string> callback)
    {
        // Construct final prompt
        string prompt = 
$@"SYSTEM:
{systemTemplate}

PERSONA_JSON:
{personaJSON}

USER:
{userTemplate}

Respond in character.";

        var payload = new OllamaRequest{
            model = modelName,
            prompt = prompt,
            stream = false
        };

        string json = JsonUtility.ToJson(payload);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(ollamaURL, "POST")) {
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();
            //Debug.Log("Server returned: " + req.downloadHandler.text);

            if (req.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Ollama error: " + req.error);
                callback?.Invoke("[LLM Error]");
            } else {
                var result = JsonUtility.FromJson<OllamaResponse>(req.downloadHandler.text);
                //Debug.Log("Raw Ollama response: " + req.downloadHandler.text);
                callback?.Invoke(result.response);
            }
        }
    }

    [Serializable]
    public class OllamaRequest {
        public string model;
        public string prompt;
        public bool stream;
    }


    [Serializable]
    private class OllamaResponse {
        public string response;
    }
}
