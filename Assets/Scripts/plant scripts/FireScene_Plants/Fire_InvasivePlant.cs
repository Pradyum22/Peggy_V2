using UnityEngine;

/// <summary>
/// Controls invasive plants (invade1, invade2) during the Fire Cycle.
/// </summary>
public class Fire_InvasivePlant : MonoBehaviour
{
    private Animator[] childAnimators;

    [Header("Stage Settings")]
    [Tooltip("Set to 1 for INVADE1, Set to 2 for INVADE2")]
    public int activationStage = 1;

    private void Awake()
    {
        childAnimators = GetComponentsInChildren<Animator>(true);
    }

    public void OnFireStageUpdate(int stage)
    {
        if (stage == 0 || stage == 4)
        {
            // Stage 0 (Reset) & Stage 4 (Restored) -> Hide Invasives
            gameObject.SetActive(false);
        }
        else if (stage == activationStage)
        {
            // Stage 1 or 2: Activate and trigger growth cycle
            gameObject.SetActive(true);
            if (childAnimators != null)
            {
                foreach (var anim in childAnimators)
                {
                    if (anim != null)
                    {
                        anim.Rebind();
                        anim.Update(0f);
                    }
                }
            }
        }
        // At stage 3 (Burn), we do nothing here so the Burn Shader can visually dissolve them.
    }
}