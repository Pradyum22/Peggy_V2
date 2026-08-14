using UnityEngine;

/// <summary>
/// Controls native plants near invasives that die during Stage 1.
/// Can be attached directly to a parent container (e.g. PlantDie) or individual plants.
/// </summary>
public class Fire_NativePlantDie : MonoBehaviour
{
    private Animator[] childAnimators;

    private void Awake()
    {
        // Automatically grabs Animators on this object and ALL children!
        childAnimators = GetComponentsInChildren<Animator>(true);

        if (childAnimators == null || childAnimators.Length == 0)
        {
            Debug.LogWarning($"[Fire_NativePlantDie] No Animators found on or under {name}");
        }
    }

    private void Update()
    {
        if (childAnimators == null) return;

        // Hide child plant GameObjects individually when their 'isdead' animation finishes
        foreach (var anim in childAnimators)
        {
            if (anim != null && anim.gameObject.activeSelf)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
                {
                    anim.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Called when the Web UI Fire slider updates (Stage 0 to 4).
    /// </summary>
    public void OnFireStageUpdate(int stage)
    {
        Debug.Log($"[Fire_NativePlantDie] {name} received Fire Stage {stage}");

        if (childAnimators == null) return;

        if (stage == 0)
        {
            // Stage 0: Healthy reference ecosystem -> Re-enable and reset all child plants
            foreach (var anim in childAnimators)
            {
                if (anim != null)
                {
                    anim.gameObject.SetActive(true);
                    anim.Rebind();
                    anim.Update(0f);
                }
            }
        }
        else if (stage == 1)
        {
            // Stage 1: Invasives Spread -> Trigger "TrDie" on all child plants
            foreach (var anim in childAnimators)
            {
                if (anim != null && anim.gameObject.activeInHierarchy)
                {
                    anim.SetTrigger("TrDie");
                }
            }
        }
        else if (stage >= 3)
        {
            // Stage 3 & 4: Post-fire ecosystem restoration -> Re-enable all native plants
            foreach (var anim in childAnimators)
            {
                if (anim != null)
                {
                    anim.gameObject.SetActive(true);
                    anim.Rebind();
                    anim.Update(0f);
                }
            }
        }
    }
}