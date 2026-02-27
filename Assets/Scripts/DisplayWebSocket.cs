using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NativeWebSocket;

public class DisplayWebSocket : MonoBehaviour
{
    [Header("WebSocket server (Node.js)")]
    public string serverUrl = "ws://192.168.30.224:3000";

    private WebSocket ws;

    private readonly List<nativePlant> nativePlants = new();
    private readonly List<rattleSnakeMaster> rattlePlants = new();
    private RainController rainController;

    [Serializable]
    private class SliderMessage
    {
        public string type;
        public string factor;   // NEW
        public int value;
    }

    private async void Start()
    {
        // Cache plant controllers
        nativePlants.AddRange(FindObjectsByType<nativePlant>(FindObjectsSortMode.None));
        rattlePlants.AddRange(FindObjectsByType<rattleSnakeMaster>(FindObjectsSortMode.None));

        // Cache RainController
        rainController = FindFirstObjectByType<RainController>();

        Debug.Log($"[DisplayWebSocket] Found {nativePlants.Count} nativePlant, {rattlePlants.Count} rattleSnakeMaster.");
        Debug.Log($"[DisplayWebSocket] RainController found: {rainController != null}");

        ws = new WebSocket(serverUrl);

        ws.OnOpen += () =>
        {
            Debug.Log("[DisplayWebSocket] Connected");

            var register = new SliderMessage { type = "registerDisplay", value = 0 };
            ws.SendText(JsonUtility.ToJson(register));
        };

        ws.OnError += err =>
        {
            Debug.LogError("[DisplayWebSocket] Error: " + err);
        };

        ws.OnClose += code =>
        {
            Debug.Log("[DisplayWebSocket] Closed: " + code);
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
                    DispatchByFactor(msg.factor, v);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[DisplayWebSocket] Parse failed: " + ex.Message);
            }
        };

        await ws.Connect();
    }

    private void DispatchByFactor(string factor, int value)
    {
        Debug.Log($"[DisplayWebSocket] Factor: {factor}, Value: {value}");

        // DEFAULT behavior (backwards compatibility)
        if (string.IsNullOrEmpty(factor))
        {
            DispatchPlants(value);
            return;
        }

        switch (factor)
        {
            case "fire":
                DispatchPlants(value);
                break;

            case "flowers":
                DispatchPlants(value);
                break;

            case "rain":
                if (rainController != null)
                    rainController.SetRainState(value);
                break;
        }
    }

    private void DispatchPlants(int value)
    {
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