using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    [Header("Pollinator Particle Systems")]
    [Tooltip("Leave empty to automatically grab all child particle systems (Butterflies + Bees).")]
    public ParticleSystem[] butterflies;

    [Header("Scene Configuration")]
    [Tooltip("Check this in RainScene so 1 (Max Rain) = 0 pollinators, and -1 (No Rain) = Max pollinators.")]
    public bool invertRainLogic = false;

    [Header("Emission Rates")]
    public float activeEmissionRate = 4f;   // Full presence
    public float inactiveEmissionRate = 0f; // Completely absent

    private ParticleSystem.EmissionModule[] emissions;

    void Awake()
    {
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

    public void SetButterflyState(int state)
    {
        // Flip input if this instance is running in RainScene
        int effectiveState = invertRainLogic ? -state : state;

        switch (effectiveState)
        {
            case 1:
                // Active pollinators (Native Species in PlantScene OR Dry/Clear in RainScene)
                SetEmission(activeEmissionRate);
                break;

            case 0:
                // Moderate emission
                SetEmission(activeEmissionRate * 0.5f);
                break;

            case -1:
                // Inactive pollinators (Invasive in PlantScene OR Max Rain in RainScene)
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