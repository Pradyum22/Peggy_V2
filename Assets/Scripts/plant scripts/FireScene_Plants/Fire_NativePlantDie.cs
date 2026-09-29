using UnityEngine;

/// <summary>
/// Controls native plants near invasives that die during a specific invasion stage.
/// Attach to the parent container (e.g. PlantDie_Stage1 or PlantDie_Stage2).
/// </summary>
public class Fire_NativePlantDie : MonoBehaviour
{
    private Animator[] childAnimators;

    [Header("Stage Settings")]
    [Tooltip("Set to 1 to die with INVADE1, Set to 2 to die with INVADE2")]
    public int activationStage = 1;

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

    // --- ARTIST TESTING TOOLS ---
    [ContextMenu("TEST: Trigger Die (Burn)")]
    public void TestBurn()
    {
        if (childAnimators == null) return;
        foreach (var anim in childAnimators)
        {
            if (anim != null && anim.gameObject.activeInHierarchy)
            {
                anim.SetTrigger("TrDie");
            }
        }
    }

    [ContextMenu("TEST: Reset")]
    public void ResetBurn()
    {
        if (childAnimators == null) return;
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

    public void OnFireStageUpdate(int stage)
    {
        if (childAnimators == null) return;

        if (stage == 0 || stage == 4)
        {
            // Stage 4 (Restored) & Stage 0 (Reset) -> Re-enable all child plants
            ResetBurn();
        }
        else if (stage == activationStage)
        {
            // Trigger "TrDie" when the slider matches this group's activation stage
            TestBurn();
        }
    }
}