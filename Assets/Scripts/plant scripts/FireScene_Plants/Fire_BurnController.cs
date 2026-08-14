using System.Collections;
using UnityEngine;

/// <summary>
/// Controls burn shader graphs (INV_PL_BURN, INVroot_BURN, native_pl_BURN) via smooth threshold lerping.
/// </summary>
public class Fire_BurnController : MonoBehaviour
{
    [Header("Burn Renderers")]
    [Tooltip("Leave empty to auto-detect all child renderers with burn shaders.")]
    public Renderer[] burnRenderers;

    [Header("Shader Settings")]
    [Tooltip("The reference name of the burn float property in Shader Graph (e.g. _threshold or _BurnAmount)")]
    public string propertyName = "_threshold";

    [Header("Burn Threshold Bounds")]
    public float unburnedValue = 0.0f;    // No burn (Normal state)
    public float fullyBurnedValue = 1.0f;  // Fully burned / incinerated state

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f;     // Duration of burn transition in seconds

    private Coroutine fadeCoroutine;
    private int propertyID;

    private void Awake()
    {
        propertyID = Shader.PropertyToID(propertyName);

        // Auto-detect renderers if not manually assigned
        if (burnRenderers == null || burnRenderers.Length == 0)
        {
            burnRenderers = GetComponentsInChildren<Renderer>(true);
        }
    }

    /// <summary>
    /// Called when the Web UI Fire slider updates (Stage 0 to 4).
    /// </summary>
    public void OnFireStageUpdate(int stage)
    {
        // Stage 2 = Controlled Fire Active -> Lerp to full burn (1.0)
        // Stages 0, 1, 3, 4 = No Fire -> Reset to unburned (0.0)
        float targetValue = (stage == 2) ? fullyBurnedValue : unburnedValue;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeBurnThreshold(targetValue));
    }

    private IEnumerator FadeBurnThreshold(float target)
    {
        if (burnRenderers == null || burnRenderers.Length == 0) yield break;

        // Find a valid renderer containing the target shader property
        Renderer validRenderer = null;
        foreach (var r in burnRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
            {
                validRenderer = r;
                break;
            }
        }

        if (validRenderer == null)
        {
            Debug.LogWarning($"[Fire_BurnController] No material found on {name} with property '{propertyName}'.");
            yield break;
        }

        float startValue = validRenderer.material.GetFloat(propertyID);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, target, elapsed / fadeDuration);

            foreach (var r in burnRenderers)
            {
                if (r != null && r.material != null && r.material.HasProperty(propertyID))
                {
                    r.material.SetFloat(propertyID, currentValue);
                }
            }

            yield return null;
        }

        // Snap to target at end of lerp
        foreach (var r in burnRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
            {
                r.material.SetFloat(propertyID, target);
            }
        }
    }
}