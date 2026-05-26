using UnityEngine;
using System.Collections;

public class Texture_Change : MonoBehaviour
{
    [Header("Dry Grass Material")]
    public Renderer dryGrassRenderer;

    [Header("Fade Settings")]
    public float fadeSpeed = 0.4f;

    [Header("Drought Delay")]
    public float dryGrassDelay = 3f;

    private Material dryMaterial;

    private float targetAlpha = 0f;

    private Coroutine delayRoutine;

    void Start()
    {
        dryMaterial = dryGrassRenderer.material;

        Color c = dryMaterial.color;
        c.a = 0f;
        dryMaterial.color = c;
    }

    public void ChangeMaterial(int state)
    {
        // Stop previous delay coroutine
        if (delayRoutine != null)
        {
            StopCoroutine(delayRoutine);
        }

        // DROUGHT
        if (state == -1)
        {
            delayRoutine = StartCoroutine(FadeDryGrassAfterDelay());
        }

        // NORMAL / HEAVY RAIN
        else
        {
            targetAlpha = 0f;
        }
    }

    IEnumerator FadeDryGrassAfterDelay()
    {
        yield return new WaitForSeconds(dryGrassDelay);

        targetAlpha = 1f;
    }

    void Update()
    {
        Color c = dryMaterial.color;

        c.a = Mathf.MoveTowards(
            c.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );

        dryMaterial.color = c;
    }
}