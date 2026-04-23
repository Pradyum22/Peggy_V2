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

    void Awake()
    {
        groundEmission = rainGround.emission;
        backEmission = rainBack.emission;

        groundMain = rainGround.main;
        backMain = rainBack.main;
    }

    public void SetRainState(int state)
    {
        switch (state)
        {
            case -1: // Light rain
                ApplyRainSettings(8f, 6f, 0.35f);
                break;

            case 0: // Moderate rain
                ApplyRainSettings(60f, 10f, 0.45f);
                break;

            case 1: // Heavy rain
                ApplyRainSettings(110f, 13f, 0.55f);
                break;
        }
    }

    void ApplyRainSettings(float emissionRate, float fallSpeed, float sizeMultiplier)
    {
        groundEmission.rateOverTime = emissionRate;
        backEmission.rateOverTime = emissionRate * 0.6f;

        groundMain.startSpeed = fallSpeed;
        backMain.startSpeed = fallSpeed * 0.8f;

        groundMain.startSizeMultiplier = sizeMultiplier;
        backMain.startSizeMultiplier = sizeMultiplier * 0.7f;
    }
}