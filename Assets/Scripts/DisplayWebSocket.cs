using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NativeWebSocket;

public class DisplayWebSocket : MonoBehaviour
{
    [Header("WebSocket server (Node.js)")]
    public string serverUrl = "ws://yourIP:3000";

    private WebSocket ws;
    [SerializeField] private FireController fireController;
    [SerializeField] private ButterflyController butterflyController;

    public WaterLevelController waterController;
    public WormController wormController;
    public SkyController skyController;
    public Texture_Change grassController;

    private readonly List<nativePlant> nativePlants = new();
    private readonly List<rattleSnakeMaster> rattlePlants = new();

    private readonly List<root_static> rootStatics = new();
    private readonly List<root3_growdie> rootGrowDies = new();

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

        // Cache Root Controllers
        rootStatics.AddRange(FindObjectsByType<root_static>(FindObjectsSortMode.None));
        rootGrowDies.AddRange(FindObjectsByType<root3_growdie>(FindObjectsSortMode.None));

        // Cache RainController
        rainController = FindFirstObjectByType<RainController>();

        Debug.Log($"[DisplayWebSocket] Found {rootStatics.Count} roots_static, {rootGrowDies.Count} root3_growdie.");
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
        Debug.Log("RAIN SIGNAL RECEIVED: " + value);

        // DEFAULT behavior (backwards compatibility)
        if (string.IsNullOrEmpty(factor))
        {
            DispatchPlants(value);
            return;
        }

        switch (factor)
        {
            case "fire":
                if (fireController != null)
                    fireController.UpdateFire(value);
                break;

            case "flowers":
                DispatchPlants(value);
                break;

            case "rain":

                if (rainController != null)
                    rainController.SetRainState(value);

                foreach (var r in rootStatics)
                    if (r != null)
                        r.OnRemoteSliderUpdate(value);

                foreach (var r in rootGrowDies)
                    if (r != null)
                        r.OnRemoteSliderUpdate(value);

                if (butterflyController != null)
                    butterflyController.SetButterflyState(value);

                if (skyController != null)
                    skyController.SetRainState(value);

                if (grassController != null)
                    grassController.ChangeMaterial(value);

                if (waterController != null)
                    waterController.SetWaterLevel(value);

                if (wormController != null)
                    wormController.SetWormState(value);

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