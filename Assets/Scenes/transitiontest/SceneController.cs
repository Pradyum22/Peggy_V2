using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Singleton
    public static SceneController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] private BaseTransitionEffect transitionEffect;

    private Dictionary<string, string> loadedSceneBySlot = new();
    private bool isBusy = false;

    public string CurrentActiveScene { get; private set; } = "";

    // Convenience method for UI buttons and WebSocket commands
    public void SwitchToScene(string targetSceneName)
    {
        if (CurrentActiveScene == targetSceneName || isBusy) return;

        CurrentActiveScene = targetSceneName;

        NewTransition()
            .Load(SceneDatabase.Slots.SessionContent, targetSceneName, setActive: true)
            .WithOverlay()
            .WithClearUnusedAssets()
            .Perform();
    }

    // API
    public SceneTransitionPlan NewTransition()
    {
        return new SceneTransitionPlan();
    }

    private Coroutine ExecutePlan(SceneTransitionPlan plan)
    {
        if (isBusy)
        {
            Debug.LogWarning("Scene change already in progress");
            return null;
        }
        isBusy = true;
        return StartCoroutine(ChangeSceneRoutine(plan));
    }

    private IEnumerator ChangeSceneRoutine(SceneTransitionPlan plan)
    {
        // 1. Play transition effect (Fade, Butterfly VFX, etc.)
        if (plan.Overlay && transitionEffect != null)
        {
            yield return transitionEffect.CoverScreen();
            yield return new WaitForSeconds(0.2f);
        }

        // 2. Unload requested scenes
        foreach (var slotKey in plan.ScenesToUnload)
        {
            yield return UnloadSceneRoutine(slotKey);
        }

        if (plan.ClearUnusedAssets) yield return CleanupUnusedAssetsRoutine();

        // 3. Load incoming scenes (automatically unloads previous occupant of that slot)
        foreach (var kvp in plan.ScenesToLoad)
        {
            if (loadedSceneBySlot.ContainsKey(kvp.Key))
            {
                yield return UnloadSceneRoutine(kvp.Key);
            }
            yield return LoadAdditiveRoutine(kvp.Key, kvp.Value, plan.ActiveSceneName == kvp.Value);
        }

        // 4. Reveal the new scene
        if (plan.Overlay && transitionEffect != null)
        {
            yield return transitionEffect.RevealScreen();
        }

        isBusy = false;
    }

    private IEnumerator LoadAdditiveRoutine(string slotKey, string sceneName, bool setActive)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOp == null) yield break;

        loadOp.allowSceneActivation = false;
        while (loadOp.progress < 0.9f)
        {
            yield return null;
        }

        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone)
        {
            yield return null;
        }

        if (setActive)
        {
            Scene newScene = SceneManager.GetSceneByName(sceneName);
            if (newScene.IsValid() && newScene.isLoaded)
            {
                SceneManager.SetActiveScene(newScene);
            }
        }
        loadedSceneBySlot[slotKey] = sceneName;
    }

    private IEnumerator UnloadSceneRoutine(string slotKey)
    {
        if (!loadedSceneBySlot.TryGetValue(slotKey, out string sceneName)) yield break;
        if (string.IsNullOrEmpty(sceneName)) yield break;

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        if (unloadOp != null)
        {
            while (!unloadOp.isDone)
            {
                yield return null;
            }
        }
        loadedSceneBySlot.Remove(slotKey);
    }

    private IEnumerator CleanupUnusedAssetsRoutine()
    {
        AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
        while (!cleanupOp.isDone)
        {
            yield return null;
        }
    }

    public class SceneTransitionPlan
    {
        public Dictionary<string, string> ScenesToLoad { get; } = new();
        public List<string> ScenesToUnload { get; } = new();
        public string ActiveSceneName { get; private set; } = "";
        public bool ClearUnusedAssets { get; private set; } = false;
        public bool Overlay { get; private set; } = false;

        public SceneTransitionPlan Load(string slotKey, string sceneName, bool setActive = false)
        {
            ScenesToLoad[slotKey] = sceneName;
            if (setActive) ActiveSceneName = sceneName;
            return this;
        }

        public SceneTransitionPlan Unload(string slotKey)
        {
            ScenesToUnload.Add(slotKey);
            return this;
        }

        public SceneTransitionPlan WithOverlay()
        {
            Overlay = true;
            return this;
        }

        public SceneTransitionPlan WithClearUnusedAssets()
        {
            ClearUnusedAssets = true;
            return this;
        }

        public Coroutine Perform()
        {
            return SceneController.Instance.ExecutePlan(this);
        }
    }
}