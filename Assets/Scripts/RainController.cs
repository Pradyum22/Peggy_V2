using System.Collections;
using UnityEngine;

public class RainController : MonoBehaviour
{
    [Header("Rain Layers")]
    public ParticleSystem rainGround;
    public ParticleSystem rainBack;

    private ParticleSystem.EmissionModule groundEmission;
    private ParticleSystem.EmissionModule backEmission;

    private ParticleSystem.MainModule groundMain;
    private ParticleSystem.MainModule backMain;

    [Header("Puddle")]
    [SerializeField] private Renderer puddleRenderer;

    [SerializeField] private string thresholdProperty = "threshold";

    [SerializeField] private float hiddenThreshold = 0.685f;

    [SerializeField] private float visibleThreshold = 0.3f;

    [SerializeField] private float puddleFadeTime = 6f;

    private Material puddleMaterial;
    private Coroutine puddleRoutine;
    private Coroutine rainRoutine;

    // Original values
    private float defaultGroundEmission;
    private float defaultBackEmission;

    private float defaultGroundSpeed;
    private float defaultBackSpeed;

    private float defaultGroundSize;
    private float defaultBackSize;


    void Awake()
    {
        groundEmission = rainGround.emission;
        backEmission = rainBack.emission;

        groundMain = rainGround.main;
        backMain = rainBack.main;

        // Store original values from inspector
        defaultGroundEmission = groundEmission.rateOverTime.constant;
        defaultBackEmission = backEmission.rateOverTime.constant;

        defaultGroundSpeed = groundMain.startSpeed.constant;
        defaultBackSpeed = backMain.startSpeed.constant;

        defaultGroundSize = groundMain.startSizeMultiplier;
        defaultBackSize = backMain.startSizeMultiplier;

        if (puddleRenderer != null)
        {
            puddleMaterial = puddleRenderer.material;

            puddleMaterial.SetFloat(
                thresholdProperty,
                hiddenThreshold
            );
        }

    }


    void Start()
    {
        // Start completely dry
        rainGround.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        rainBack.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }


    public void SetRainState(int state)
    {
        if (rainRoutine != null)
        {
            StopCoroutine(rainRoutine);
            rainRoutine = null;
        }

        switch (state)
        {
            case -1:

                rainGround.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                rainBack.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                FadePuddle(hiddenThreshold);

                break;


            case 0:

                RestoreDefaultRain();

                FadePuddle(hiddenThreshold);

                StartCoroutine(RandomRainRoutine());

                break;


            case 1:

                if (!rainGround.isPlaying)
                    rainGround.Play();

                if (!rainBack.isPlaying)
                    rainBack.Play();

                ApplyHeavyRain();

                FadePuddle(visibleThreshold);

                break;
        }
    }

    private void RestoreDefaultRain()
    {
        groundEmission.rateOverTime = defaultGroundEmission;
        backEmission.rateOverTime = defaultBackEmission;

        groundMain.startSpeed = defaultGroundSpeed;
        backMain.startSpeed = defaultBackSpeed;

        groundMain.startSizeMultiplier = defaultGroundSize;
        backMain.startSizeMultiplier = defaultBackSize;
    }

    private void ApplyHeavyRain()
    {
        groundEmission.rateOverTime = defaultGroundEmission * 2.0f;
        backEmission.rateOverTime = defaultBackEmission * 2.0f;

        groundMain.startSpeed = defaultGroundSpeed * 1.3f;
        backMain.startSpeed = defaultBackSpeed * 1.5f;

        groundMain.startSizeMultiplier = defaultGroundSize * 1.15f;
        backMain.startSizeMultiplier = defaultBackSize * 1.15f;
    }
    private void FadePuddle(float target)
    {
        if (puddleRoutine != null)
            StopCoroutine(puddleRoutine);

        puddleRoutine = StartCoroutine(FadePuddleRoutine(target));
    }

    private IEnumerator FadePuddleRoutine(float target)
    {
        if (puddleMaterial == null)
            yield break;

        float start =
            puddleMaterial.GetFloat(thresholdProperty);

        float elapsed = 0f;

        while (elapsed < puddleFadeTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / puddleFadeTime;

            float value = Mathf.Lerp(start, target, t);

            puddleMaterial.SetFloat(
                thresholdProperty,
                value
            );

            yield return null;
        }

        puddleMaterial.SetFloat(
            thresholdProperty,
            target
        );
    }

    private IEnumerator RandomRainRoutine()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(5f, 10f);

            yield return new WaitForSeconds(waitTime);

            rainGround.Play();
            rainBack.Play();

            yield return new WaitForSeconds(5f);
            Debug.Log("Rain!");

            rainGround.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            rainBack.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}