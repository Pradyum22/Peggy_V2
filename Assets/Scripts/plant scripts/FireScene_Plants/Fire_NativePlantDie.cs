using UnityEngine;

/// <summary>
/// Controls native plants near invasives that die during Stage 1.
/// Attached to the 'PlantDie' parent container (children dont need it)
/// </summary>
public class Fire_NativePlantDie : MonoBehaviour
{
    private Animator[] childAnimators;

    private void Awake()
    {
        childAnimators = GetComponentsInChildren<Animator>(true);
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

    public void OnFireStageUpdate(int stage)
    {
        if (childAnimators == null) return;

        if (stage == 0 || stage == 4)
        {
            // Stage 4 (Restored) & Stage 0 (Reset) -> Re-enable all child plants
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
            // Stage 1: Invasives Spread -> Trigger "TrDie"
            foreach (var anim in childAnimators)
            {
                if (anim != null && anim.gameObject.activeInHierarchy)
                {
                    anim.SetTrigger("TrDie");
                }
            }
        }
    }
}