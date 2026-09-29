using UnityEngine;

public class SceneTrans : MonoBehaviour
{
    /* #region Singleton
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

    [SerializedField] private LoadingOverlay

    private Dictionary<string, string> loadedSceneBySlot = new();

    private bool isBusy = false;
//API

    public SceneTransitionPlan NewTransition()
    {
        return new SceneTransitionPlan();
    }

    private Coroutine ExecutePlan(SceneTransitionPlan plan)
    {
        if (isBusy)
        {
            Debug.LogWarning("Scene change already in progress")
            return null;
        }
        isBusy = true;
        return StartCoroutine(ChangeSceneRoutine(plan));

    }

    private IEnumerator ChangeSceneRoutine
    {
        if (plan.Overlay)
        {
            yeild return loadingOverlay.FadeInBlack();
            yield return new WaitForSeconds(0.5f);

        }

        foreach (var slotKey in plan.ScenesToUnload)
        {
            yield return UnloadSceneRoutine(slotKey);
        }
        if (plan.ClearUnusedAssets) yield return CleanupUnusedAssetsRoutine():

        foreach(var kvp in plan.ScenesToLoad)
        {
            if(loadedSceneBySlot.ContainsKey(kvp,Key))
            {
                yeild return UnloadSceneRoutine(kvp, Key);
            }
            yield return LoadAdditiveRoutine(kvp,Key, kvp.Value, plan.ActiveSceneName == kvp.Value);
        }
        if(plan.Overlay)
        {
            yield return loadingOverlay.FadeOutBlack();
        }
        isBusy = false;
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
            ScenesToLoad(slotKey) = sceneName;
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

    */


}
