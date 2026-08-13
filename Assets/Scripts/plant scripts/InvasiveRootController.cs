using System.Collections;
using UnityEngine;

public class InvasiveRootController : MonoBehaviour
{
    [Header("Root Renderers")]
    [Tooltip("Leave empty to automatically grab all renderers on this object and its children.")]
    public Renderer[] rootRenderers;

    [Header("Shader Settings")]
    [Tooltip("The Reference Name in Shader Graph (usually starts with an underscore, e.g. _threshold)")]
    public string propertyName = "_threshold";

    [Header("Threshold Bounds")]
    public float visibleThreshold = 0.009f; // Fully visible roots (Invasive = -1)
    public float hiddenThreshold = 0.76f;   // Fully hidden roots (Native = 1)

    [Header("Transition Settings")]
    public float fadeDuration = 1.5f; // Fade duration in seconds

    private Coroutine fadeCoroutine;
    private int propertyID;

    private void Awake()
    {
        propertyID = Shader.PropertyToID(propertyName);

        // Auto-detect child renderers if list is empty
        if (rootRenderers == null || rootRenderers.Length == 0)
        {
            rootRenderers = GetComponentsInChildren<Renderer>(true);
        }
    }

    public void OnRemoteSliderUpdate(int value)
    {
        // Web UI value:
        // -1 = Invasive Species -> Target: 0.009 (Visible)
        //  1 = Native Species   -> Target: 0.76  (Hidden)
        float targetThreshold = (value <= -1) ? visibleThreshold : hiddenThreshold;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeThreshold(targetThreshold));
    }

    private IEnumerator FadeThreshold(float target)
    {
        if (rootRenderers == null || rootRenderers.Length == 0) yield break;

        // Find a valid renderer that has the property
        Renderer validRenderer = null;
        foreach (var r in rootRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
            {
                validRenderer = r;
                break;
            }
        }

        if (validRenderer == null)
        {
            Debug.LogWarning($"[InvasiveRootController] No root material found with property '{propertyName}'. Check shader Reference Name!");
            yield break;
        }

        float startValue = validRenderer.material.GetFloat(propertyID);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, target, elapsed / fadeDuration);

            foreach (var r in rootRenderers)
            {
                if (r != null && r.material != null && r.material.HasProperty(propertyID))
                {
                    r.material.SetFloat(propertyID, currentValue);
                }
            }

            yield return null;
        }

        // Final snap to target
        foreach (var r in rootRenderers)
        {
            if (r != null && r.material != null && r.material.HasProperty(propertyID))
            {
                r.material.SetFloat(propertyID, target);
            }
        }
    }
}