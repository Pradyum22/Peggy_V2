using System.Collections;
using UnityEngine;

/// <summary>
/// Controls burn shaders, smoke VFX, and native plant regrowth for the Fire Cycle.
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
    [Tooltip("Check this if this object contains native plants (like 'Flowers') that need to play a grow animation at Stage 4.")]
    public bool snapAndRegrowAtStage4 = false;

    [Header("VFX Integration")]
    [Tooltip("Drag your Smoke Particle Systems here. They will play at Stage 3 and stop at Stage 4.")]
    public ParticleSystem[] smokeVFX;

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

    // --- Kelly TESTING TOOLS ---
    [ContextMenu("TEST: Trigger Burn")]
    public void TestBurn() => OnFireStageUpdate(3);

    [ContextMenu("TEST: Reset Burn")]
    public void ResetBurn() => OnFireStageUpdate(0);

    public void OnFireStageUpdate(int stage)
    {
        if (stage == 3)
        {
            // Stage 3: Controlled Burn & Smoke
            TriggerBurnLerp(fullyBurnedValue, fadeDuration);
            foreach (var s in smokeVFX) if (s != null) s.Play();
        }
        else if (stage == 4)
        {
            // Stage 4: Native Prarie Restoration
            foreach (var s in smokeVFX) if (s != null) s.Stop();

            if (snapAndRegrowAtStage4)
            {
                // Native Plants: Instantly remove burn shader and play grow animation
                TriggerBurnLerp(unburnedValue, 0f);
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
                // Invasives / Burned Roots: STAY fully burned away while natives regrow!
                TriggerBurnLerp(fullyBurnedValue, 0f);
            }
        }
        else if (stage == 0)
        {
            // Stage 0: Total Reset (Silent)
            foreach (var s in smokeVFX) if (s != null) s.Stop();
            TriggerBurnLerp(unburnedValue, 0f);

            if (snapAndRegrowAtStage4)
            {
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
        }
        else
        {
            // Stages 1 & 2: Normal unburned state
            foreach (var s in smokeVFX) if (s != null) s.Stop();
            TriggerBurnLerp(unburnedValue, 0f);
        }
    }

    private void TriggerBurnLerp(float target, float duration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        if (duration <= 0f)
        {
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