using System.Collections;
using UnityEngine;

public class PuddleController : MonoBehaviour
{
    [Header("Puddle")]
    [SerializeField] private Renderer puddleRenderer;

    [SerializeField] private string thresholdProperty = "_threshold";

    [Header("Threshold Values")]
    [SerializeField] private float hiddenThreshold = 0.685f;
    [SerializeField] private float visibleThreshold = 0.30f;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 6f;

    private Material puddleMaterial;
    private Coroutine currentRoutine;
   
    private void Awake()
    {
        // Creates a unique material instance for this puddle
        puddleMaterial = puddleRenderer.material;

        // Start invisible
        puddleMaterial.SetFloat(thresholdProperty, hiddenThreshold);
    }

    public void SetPuddleState(int state)
    {
        float targetThreshold;

        switch (state)
        {
            case 1:
                targetThreshold = visibleThreshold;
                break;

            default:
                targetThreshold = hiddenThreshold;
                break;
        }

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeThreshold(targetThreshold));
    }

    private IEnumerator FadeThreshold(float target)
    {
        float start =
            puddleMaterial.GetFloat(thresholdProperty);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;

            float value = Mathf.Lerp(start, target, t);

            puddleMaterial.SetFloat(thresholdProperty, value);

            yield return null;
        }

        puddleMaterial.SetFloat(thresholdProperty, target);
    }
}