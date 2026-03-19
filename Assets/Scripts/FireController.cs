using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Particle Systems (all layers)")]
    public ParticleSystem[] fireSystems;

    private float[] baseEmissionRates;
    private Vector3 baseScale;
    private Vector3 targetScale;

    void Start()
    {
        // Cache original emission values
        baseEmissionRates = new float[fireSystems.Length];

        for (int i = 0; i < fireSystems.Length; i++)
        {
            baseEmissionRates[i] = fireSystems[i].emission.rateOverTime.constant;
        }

        // Cache initial scale
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    public void UpdateFire(float fireValue)
    {
        float emissionMultiplier;
        float scaleMultiplier;

        // Map slider values (-1, 0, 1)
        if (fireValue == -1)
        {
            emissionMultiplier = 0.3f;
            scaleMultiplier = 0.95f;
        }
        else if (fireValue == 0)
        {
            emissionMultiplier = 1.0f;
            scaleMultiplier = 1.0f;
        }
        else // fireValue == 1
        {
            emissionMultiplier = 2.0f;
            scaleMultiplier = 1.1f;
        }

        // Apply emission changes instantly
        for (int i = 0; i < fireSystems.Length; i++)
        {
            var emission = fireSystems[i].emission;
            emission.rateOverTime = baseEmissionRates[i] * emissionMultiplier;

            if (!fireSystems[i].isPlaying)
                fireSystems[i].Play();
        }

        // Set target scale (DO NOT apply directly)
        targetScale = baseScale * scaleMultiplier;
    }

    void Update()
    {
        // Smoothly interpolate scale
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * 3f   // smoothing speed (tweakable)
        );
    }
}