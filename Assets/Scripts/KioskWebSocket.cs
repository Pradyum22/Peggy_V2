using UnityEngine;
using NativeWebSocket;
using System.Text;
using System.Threading.Tasks;

public class KioskWebSocket : MonoBehaviour
{
    private WebSocket websocket;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log(" Connected to WebSocket Server as Kiosk!");
            RegisterAsKiosk();
        };

        websocket.OnError += (err) => Debug.LogError(" WebSocket Error: " + err);
        websocket.OnClose += (code) => Debug.Log(" WebSocket Closed: " + code);

        await websocket.Connect();
    }

    private async void RegisterAsKiosk()
    {
        string json = "{\"type\":\"registerKiosk\"}";
        await websocket.SendText(json);
        Debug.Log(" Sent: Registering as Kiosk");
    }

    public async void SendQuizAnswer(string answer)
    {
        if (websocket.State == WebSocketState.Open)
        {
            string json = $"{{\"type\":\"quizResponse\", \"answer\":\"{answer}\"}}";
            await websocket.SendText(json);
            Debug.Log(" Sent Answer: " + answer);
        }
    }

    private void Update()
    {
        websocket?.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
