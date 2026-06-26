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

    // Original artist-tuned values
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
    }

    void Start()
    {
        // Start completely dry
        rainGround.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        rainBack.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void SetRainState(int state)
    {
        switch (state)
        {
            case -1:
                rainGround.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                rainBack.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;

            case 0:
                if (!rainGround.isPlaying)
                    rainGround.Play();

                if (!rainBack.isPlaying)
                    rainBack.Play();

                RestoreDefaultRain();
                break;

            case 1:
                if (!rainGround.isPlaying)
                    rainGround.Play();

                if (!rainBack.isPlaying)
                    rainBack.Play();

                ApplyHeavyRain();
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
}