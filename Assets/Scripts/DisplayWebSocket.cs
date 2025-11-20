using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NativeWebSocket;

public class DisplayWebSocket : MonoBehaviour
{
    [Header("WebSocket server (Node.js)")]
    public string serverUrl = "ws://75.102.253.180:3000";   // change to your laptop's IPv4 later

    private WebSocket ws;

    private readonly List<nativePlant> nativePlants = new();
    private readonly List<rattleSnakeMaster> rattlePlants = new();

    [Serializable]
    private class SliderMessage
    {
        public string type;
        public int value;
    }

    private async void Start()
    {
        // Cache all plant controllers once at startup
        nativePlants.AddRange(FindObjectsByType<nativePlant>(FindObjectsSortMode.None));
        rattlePlants.AddRange(FindObjectsByType<rattleSnakeMaster>(FindObjectsSortMode.None));

        Debug.Log($"[DisplayWebSocket] Found {nativePlants.Count} nativePlant and {rattlePlants.Count} rattleSnakeMaster scripts.");

        ws = new WebSocket(serverUrl);

        ws.OnOpen += () =>
        {
            Debug.Log("[DisplayWebSocket] Connected to WebSocket server");

            // Optional register message (keep same JSON shape as before)
            var register = new SliderMessage { type = "registerDisplay", value = 0 };
            var json = JsonUtility.ToJson(register);
            ws.SendText(json);
        };

        ws.OnError += err =>
        {
            Debug.LogError("[DisplayWebSocket] Error: " + err);
        };

        ws.OnClose += code =>
        {
            Debug.Log("[DisplayWebSocket] WebSocket closed with code: " + code);
        };

        ws.OnMessage += bytes =>
        {
            var json = Encoding.UTF8.GetString(bytes);
            Debug.Log("[DisplayWebSocket] Raw message: " + json);

            try
            {
                var msg = JsonUtility.FromJson<SliderMessage>(json);
                if (msg != null && msg.type == "slider")
                {
                    int v = Mathf.Clamp(msg.value, -1, 1);
                    DispatchSliderValue(v);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[DisplayWebSocket] Failed to parse message: " + ex.Message);
            }
        };

        await ws.Connect();
    }

    private void DispatchSliderValue(int value)
    {
        Debug.Log($"[DisplayWebSocket] Dispatching slider value {value}");

        foreach (var p in nativePlants)
        {
            if (p != null)
                p.OnRemoteSliderUpdate(value);
        }

        foreach (var p in rattlePlants)
        {
            if (p != null)
                p.OnRemoteSliderUpdate(value);
        }
    }

    private void Update()
    {
        // Required for NativeWebSocket in non-WebGL builds
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (ws != null)
        {
            await ws.Close();
            ws = null;
        }
    }
}
