using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    [Header("Pollinator Particle Systems")]
    [Tooltip("Leave empty to automatically grab all child particle systems (Butterflies + Bees).")]
    public ParticleSystem[] butterflies;

    [Header("Emission Rates")]
    public float activeEmissionRate = 4f;   // Native Species / Clear Sky
    public float inactiveEmissionRate = 0f; // Invasive Species / Heavy Rain

    private ParticleSystem.EmissionModule[] emissions;

    void Awake()
    {
        // Automatically find all child particle systems (Monarch, Cabbage, Bees) if empty
        if (butterflies == null || butterflies.Length == 0)
        {
            butterflies = GetComponentsInChildren<ParticleSystem>(true);
        }

        emissions = new ParticleSystem.EmissionModule[butterflies.Length];
        for (int i = 0; i < butterflies.Length; i++)
        {
            emissions[i] = butterflies[i].emission;
        }
    }

    /// <summary>
    /// Controls emission based on the slider state.
    /// Plant Diversity:  1 = Native (Present), -1 = Invasive (Absent)
    /// Rain Scene:      -1 = Heavy Rain (Absent), 0 = Moderate (Few), 1 = Clear (Present)
    /// </summary>
    public void SetButterflyState(int state)
    {
        switch (state)
        {
            case 1:
                // Native Species (Plants) or Clear Day (Rain) -> Pollinators Present
                SetEmission(activeEmissionRate);
                break;

            case 0:
                // Moderate Rain (Legacy RainScene support)
                SetEmission(activeEmissionRate * 0.5f);
                break;

            case -1:
                // Invasive Species (Plants) or Heavy Rain (Rain) -> Pollinators Absent
                SetEmission(inactiveEmissionRate);
                break;
        }
    }

    private void SetEmission(float rate)
    {
        if (emissions == null) return;

        for (int i = 0; i < emissions.Length; i++)
        {
            emissions[i].rateOverTime = rate;
        }
    }
}