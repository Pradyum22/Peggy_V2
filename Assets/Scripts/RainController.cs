using UnityEngine;

public class RainController : MonoBehaviour
{
    [Header("Rain Layers")]
    public ParticleSystem rainFront;
    public ParticleSystem rainGround;
    public ParticleSystem rainBack;

    private ParticleSystem.EmissionModule frontEmission;
    private ParticleSystem.EmissionModule groundEmission;
    private ParticleSystem.EmissionModule backEmission;

    private ParticleSystem.MainModule frontMain;
    private ParticleSystem.MainModule groundMain;
    private ParticleSystem.MainModule backMain;

    void Awake()
    {
        frontEmission = rainFront.emission;
        groundEmission = rainGround.emission;
        backEmission = rainBack.emission;

        frontMain = rainFront.main;
        groundMain = rainGround.main;
        backMain = rainBack.main;
    }

    public void SetRainState(int state)
    {
        switch (state)
        {
            case 0: // Too little rain
                ApplyRainSettings(8f, 6f, 0.35f);
                break;

            case 1: // Just enough
                ApplyRainSettings(65f, 10f, 0.45f);
                break;

            case 2: // Too much rain
                ApplyRainSettings(140f, 13f, 0.55f);
                break;
        }
    }

    void ApplyRainSettings(float emissionRate, float fallSpeed, float sizeMultiplier)
    {
        // Emission
        frontEmission.rateOverTime = emissionRate * 1.2f;
        groundEmission.rateOverTime = emissionRate;
        backEmission.rateOverTime = emissionRate * 0.5f;

        // Speed
        frontMain.startSpeed = fallSpeed;
        groundMain.startSpeed = fallSpeed;
        backMain.startSpeed = fallSpeed * 0.8f;

        // Size scaling
        frontMain.startSizeMultiplier = sizeMultiplier;
        groundMain.startSizeMultiplier = sizeMultiplier;
        backMain.startSizeMultiplier = sizeMultiplier * 0.6f;
    }
}