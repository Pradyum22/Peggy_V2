using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public ParticleSystem[] butterflies;

    private ParticleSystem.EmissionModule[] emissions;

    void Awake()
    {
        emissions = new ParticleSystem.EmissionModule[butterflies.Length];

        for (int i = 0; i < butterflies.Length; i++)
        {
            emissions[i] = butterflies[i].emission;
        }
    }

    public void SetButterflyState(int state)
    {
        switch (state)
        {
            case -1: // Very little rain
                SetEmission(15f);   // lots of butterflies
                break;

            case 0: // Moderate rain
                SetEmission(9f);   // few butterflies
                break;

            case 1: // Heavy rain
                SetEmission(0f);    // none
                break;
        }
    }

    void SetEmission(float rate)
    {
        for (int i = 0; i < emissions.Length; i++)
        {
            emissions[i].rateOverTime = rate;
        }
    }
}