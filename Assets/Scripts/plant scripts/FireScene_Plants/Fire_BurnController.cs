using System.Collections;
using UnityEngine;

/// <summary>
/// Controls burn shader graphs via smooth threshold lerping, with optional Animator regrowth.
/// </summary>
public class Fire_BurnController : MonoBehaviour
{
    [Header("Burn Renderers")]
    public Renderer[] burnRenderers;

    [Header("Shader Settings")]
    public string propertyName = "_burn";

    [Header("Burn Threshold Bounds")]
    public float unburnedValue = 0.009f;
    public float fullyBurnedValue = 0.76f;

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f;

    [Header("Restoration Settings (Native Plants)")]
    [Tooltip("Check this if this object contains native plants (like 'Flowers') that need to play a grow animation at Stage 4 instead of 'unburning'.")]
    public bool snapAndRegrowAtStage4 = false;

    private Coroutine fadeCoroutine;
    private int propertyID;
    private Animator[] childAnimators;

    private void Awake()
    {
        propertyID = Shader.PropertyToID(propertyName);
        if (burnRenderers == null || burnRenderers.Length == 0)
        {
            burnRenderers = GetComponentsInChildren<Renderer>(true);
        }
        childAnimators = GetComponentsInChildren<Animator>(true);
    }

    // --- ARTIST TESTING TOOLS ---
    [ContextMenu("TEST: Trigger Burn")]
    public void TestBurn() => TriggerBurnLerp(fullyBurnedValue, fadeDuration);

    [ContextMenu("TEST: Reset Burn")]
    public void ResetBurn() => TriggerBurnLerp(unburnedValue, fadeDuration);

    public void OnFireStageUpdate(int stage)
    {
        if (stage == 3)
        {
            // Stage 3: Controlled Burn (Smooth Lerp)
            TriggerBurnLerp(fullyBurnedValue, fadeDuration);
        }
        else if (stage == 4 || stage == 0)
        {
            // Stage 4 / 0: Ecosystem Restoration
            if (snapAndRegrowAtStage4)
            {
                // 1. Snap shader instantly so we don't see the reverse burn
                TriggerBurnLerp(unburnedValue, 0f);

                // 2. Play natural Grow Animation for all child native plants
                foreach (var anim in childAnimators)
                {
                    if (anim != null)
                    {
                        anim.gameObject.SetActive(true);
                        anim.Rebind();
                        anim.Update(0f);
                    }
                }
            }
            else
            {
                // Default behavior for roots_INV (standard unburn/reset)
                TriggerBurnLerp(unburnedValue, fadeDuration);
            }
        }
        else
        {
            // Stages 1 & 2: Ensure elements remain unburned before the fire hits
            TriggerBurnLerp(unburnedValue, 0f);
        }
    }

    private void TriggerBurnLerp(float target, float duration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        if (duration <= 0f)
        {
            // Instant snap
            foreach (var r in burnRenderers)
            {
                if (r != null && r.material != null && r.material.HasProperty(propertyID))
                    r.material.SetFloat(propertyID, target);
            }
        }
        else
        {
            fadeCoroutine = StartCoroutine(FadeBurnThreshold(target, duration));
        }
    }

    private IEnumerator FadeBurnThreshold(float target, float duration)
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

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, target, elapsed / duration);

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