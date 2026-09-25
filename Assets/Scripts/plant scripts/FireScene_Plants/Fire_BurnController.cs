using System.Collections;
using UnityEngine;

/// <summary>
/// Controls burn shader graphs via smooth threshold lerping.
/// </summary>
public class Fire_BurnController : MonoBehaviour
{
    [Header("Burn Renderers")]
    public Renderer[] burnRenderers;

    [Header("Shader Settings")]
    public string propertyName = "_threshold";

    [Header("Burn Threshold Bounds")]
    public float unburnedValue = 0.009f;
    public float fullyBurnedValue = 0.76f;

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;
    private int propertyID;

    private void Awake()
    {
        propertyID = Shader.PropertyToID(propertyName);
        if (burnRenderers == null || burnRenderers.Length == 0)
        {
            burnRenderers = GetComponentsInChildren<Renderer>(true);
        }
    }

    // --- KELLY TESTING TOOLS ---
    public void TestBurn() => TriggerBurnLerp(fullyBurnedValue);
    public void ResetBurn() => TriggerBurnLerp(unburnedValue);

    public void OnFireStageUpdate(int stage)
    {
        // Stage 3 = Controlled Burn. All other stages = Reset/Unburned.
        float targetValue = (stage == 3) ? fullyBurnedValue : unburnedValue;
        TriggerBurnLerp(targetValue);
    }

    private void TriggerBurnLerp(float target)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeBurnThreshold(target));
    }

    private IEnumerator FadeBurnThreshold(float target)
    {
        if (burnRenderers == null || burnRenderers.Length == 0) yield break;

        Renderer validRenderer = null;
        foreach (var r in burnRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
            {
                validRenderer = r;
                break;
            }
        }

        if (validRenderer == null) yield break;

        float startValue = validRenderer.material.GetFloat(propertyID);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, target, elapsed / fadeDuration);

            foreach (var r in burnRenderers)
            {
                if (r != null && r.material != null && r.material.HasProperty(propertyID))
                    r.material.SetFloat(propertyID, currentValue);
            }
            yield return null;
        }

        foreach (var r in burnRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
                r.material.SetFloat(propertyID, target);
        }
    }
}