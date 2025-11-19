using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.ShaderGraph.Serialization;

public class DisplayWebSocket : MonoBehaviour
{
    private WebSocket websocket;

    [Header("Slider Target")]
    public Slider targetSlider; // Assign in Inspector
    public Animator plantAnimator; // For flower state change

    async void Start()
    {
        websocket = new WebSocket("ws://<your-ipv4>:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("[WebSocket] Connection open!");
            websocket.SendText("{"type":"registerDisplay"}");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("[WebSocket Error] " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("[WebSocket] Connection closed.");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("[WebSocket] Message received: " + message);

            try
            {
                JSONObject json = new JSONObject(message);
                if (json.HasField("type"))
                {
                    string type = json["type"].str;
                    if (type == "sliderUpdate" && json.HasField("value"))
                    {
                        float value = json["value"].f;
                        Debug.Log("[WebSocket] Setting slider value to: " + value);
                        targetSlider.value = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[WebSocket] JSON parse error: " + ex.Message);
            }
        };

        await websocket.Connect();
    }

    void Update()
    {
        websocket?.DispatchMessageQueue();
    }

    async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}






