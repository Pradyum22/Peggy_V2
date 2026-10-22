using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NativeWebSocket;

public class DisplayWebSocket : MonoBehaviour
{
    [Header("WebSocket server (Node.js)")]
    public string serverUrl = "ws://10.0.0.146:3000"; // Enter IP Here

    private WebSocket ws;
    [SerializeField] private FireController fireController;
    [SerializeField] private ButterflyController butterflyController;

    public WaterLevelController waterController;
    public WormController wormController;
    public SkyController skyController;
    public Texture_Change grassController;

    private readonly List<Fire_InvasivePlant> fireInvasives = new();
    private readonly List<Fire_NativePlantDie> fireNativeDies = new();
    private readonly List<Fire_BurnController> fireBurnControllers = new();

    private readonly List<nativePlant> nativePlants = new();
    private readonly List<rattleSnakeMaster> rattlePlants = new();
    private readonly List<InvasiveRootController> invasiveRootShaderControllers = new();

    private readonly List<root_static> rootStatics = new();
    private readonly List<root3_growdie> rootGrowDies = new();

    private RainController rainController;
    private PuddleController puddleController;

    [Serializable]
    private class SliderMessage
    {
        public string type;
        public string factor;   // NEW
        public int value;
    }

    private async void Start()
    {
        // 1. Cache all controllers in the scene (including hidden/inactive ones)
        CacheSceneControllers();

        Debug.Log($"[DisplayWebSocket] Found {rootStatics.Count} roots_static, {rootGrowDies.Count} root3_growdie.");
        Debug.Log($"[DisplayWebSocket] Found {nativePlants.Count} nativePlant, {rattlePlants.Count} rattleSnakeMaster.");
        Debug.Log($"[DisplayWebSocket] RainController found: {rainController != null}");

        // 2. Connect to WebSocket
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
                    int value;

                    if (msg.factor == "fire")
                        value = Mathf.Clamp(msg.value, 0, 3);
                    else
                        value = Mathf.Clamp(msg.value, -1, 1);

                    DispatchByFactor(msg.factor, value);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[DisplayWebSocket] Parse failed: " + ex.Message);
            }
        };

        await ws.Connect();
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Automatically grab the new controllers every time a scene transition finishes
        CacheSceneControllers();
    }

    // =========================================================
    // CACHE CONTROLLERS METHOD
    // Keeps things optimized 
    // Clears old references
    // =========================================================
    public void CacheSceneControllers()
    {
        //PlantDiversity
        nativePlants.Clear();
        rattlePlants.Clear();
        rootStatics.Clear();
        rootGrowDies.Clear();
        invasiveRootShaderControllers.Clear();

        //FireScene
        fireInvasives.Clear();
        fireNativeDies.Clear();
        fireBurnControllers.Clear();



        nativePlants.AddRange(FindObjectsByType<nativePlant>(FindObjectsInactive.Include));
        rattlePlants.AddRange(FindObjectsByType<rattleSnakeMaster>(FindObjectsInactive.Include));
        rootStatics.AddRange(FindObjectsByType<root_static>(FindObjectsInactive.Include));
        rootGrowDies.AddRange(FindObjectsByType<root3_growdie>(FindObjectsInactive.Include));

        //For Butterflies
        butterflyController = FindAnyObjectByType<ButterflyController>();

        //FireScene
        fireInvasives.AddRange(FindObjectsByType<Fire_InvasivePlant>(FindObjectsInactive.Include));
        fireNativeDies.AddRange(FindObjectsByType<Fire_NativePlantDie>(FindObjectsInactive.Include));
        fireBurnControllers.AddRange(FindObjectsByType<Fire_BurnController>(FindObjectsInactive.Include));

        // Auto-detect the shader root controller
        invasiveRootShaderControllers.AddRange(FindObjectsByType<InvasiveRootController>(FindObjectsInactive.Include));

        rainController = FindAnyObjectByType<RainController>();
        puddleController = FindAnyObjectByType<PuddleController>();
        fireController = FindAnyObjectByType<FireController>();
        butterflyController = FindAnyObjectByType<ButterflyController>();
        waterController = FindAnyObjectByType<WaterLevelController>();
        wormController = FindAnyObjectByType<WormController>();
        skyController = FindAnyObjectByType<SkyController>();
        grassController = FindAnyObjectByType<Texture_Change>();
    }

    private void DispatchByFactor(string factor, int value)
    {
        Debug.Log($"[DisplayWebSocket] Factor: {factor}, Value: {value}");

        // DEFAULT behavior (backwards compatibility)
        if (string.IsNullOrEmpty(factor))
        {
            // ADDED: Fallback transition to Plant scene for older slider formats
            SceneController.Instance.SwitchToScene(SceneDatabase.Scenes.PlantScene);

            DispatchPlants(value);
            return;
        }

        switch (factor)
        {
            case "fire":
                // ADDED: Trigger scene transition when Fire tab/slider is interacted with
                SceneController.Instance.SwitchToScene(SceneDatabase.Scenes.FireScene);

                // Route fire stage (0-4) to dedicated fire controllers
                foreach (var p in fireInvasives) if (p != null) p.OnFireStageUpdate(value);
                foreach (var p in fireNativeDies) if (p != null) p.OnFireStageUpdate(value);
                foreach (var b in fireBurnControllers) if (b != null) b.OnFireStageUpdate(value);

                // Call legacy fire controller if present
                if (fireController != null) fireController.SetFireState(value);
                break;

            case "flowers":
                // ADDED: Trigger scene transition when Plant Diversity tab/slider is interacted with
                SceneController.Instance.SwitchToScene(SceneDatabase.Scenes.PlantScene);

                DispatchPlants(value);
                break;

            case "rain":
                // ADDED: Trigger scene transition when Rainfall tab/slider is interacted with
                SceneController.Instance.SwitchToScene(SceneDatabase.Scenes.RainScene);

                Debug.Log($"Factor = {factor}, Value = {value}");

                if (rainController != null)
                {
                    Debug.Log("Calling RainController!");
                    rainController.SetRainState(value);
                }

                foreach (var r in rootStatics)
                    if (r != null)
                        r.OnRemoteSliderUpdate(value);

                foreach (var r in rootGrowDies)
                    if (r != null)
                        r.OnRemoteSliderUpdate(value);

                if (puddleController != null)
                    puddleController.SetPuddleState(value);

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
        // Web UI value: 1 = Native Species, -1 = Invasive Species

        // 1. Native Plants
        foreach (var p in nativePlants)
        {
            if (p != null) p.OnRemoteSliderUpdate(value);
        }

        // 2. Invasive Plants (Above ground thistles)
        foreach (var p in rattlePlants)
        {
            if (p != null) p.OnRemoteSliderUpdate(value);
        }

        // 3. Invasive Roots Shader (Below ground)
        foreach (var r in invasiveRootShaderControllers)
        {
            if (r != null) r.OnRemoteSliderUpdate(value);
        }

        // 4. Pollinators (Butterflies & Bees)
        if (butterflyController != null)
        {
            butterflyController.SetButterflyState(value);
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