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
            case -1: // No rain
                ApplyRainSettings(
                    0f,     // ground amount
                    0f,     // back amount
                    0f,     // ground speed
                    0f,     // back speed
                    0f,     // ground size
                    0f      // back size
                );
                break;

            case 0: // Medium rain
                ApplyRainSettings(
                    25f,    // ground amount
                    17f,    // back amount
                    2f,     // ground speed
                    1f,   // back speed
                    0.05f,  // ground size
                    0.15f   // back size
                );
                break;

            case 1: // Heavy rain
                ApplyRainSettings(
                    65f,    // ground amount
                    55f,    // back amount
                    3.4f,    // ground speed
                    1.2f,     // back speed
                    0.2f,  // ground size
                    0.25f   // back size
                );
                break;
        }
    }

    void ApplyRainSettings(
        float groundEmissionRate,
        float backEmissionRate,
        float groundSpeed,
        float backSpeed,
        float groundSize,
        float backSize)
    {
        // EMISSION
        groundEmission.rateOverTime = groundEmissionRate;
        backEmission.rateOverTime = backEmissionRate;

        // SPEED
        groundMain.startSpeed = groundSpeed;
        backMain.startSpeed = backSpeed;

        // SIZE
        groundMain.startSizeMultiplier = groundSize;
        backMain.startSizeMultiplier = backSize;
    }
}