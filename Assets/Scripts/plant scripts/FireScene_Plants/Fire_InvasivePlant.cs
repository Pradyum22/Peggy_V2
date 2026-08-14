using UnityEngine;

/// <summary>
/// Controls invasive plants (invade1, invade2) during the Fire Cycle.
/// </summary>
public class Fire_InvasivePlant : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when the Web UI Fire slider updates (Stage 0 to 4).
    /// </summary>
    public void OnFireStageUpdate(int stage)
    {
        Debug.Log($"[Fire_InvasivePlant] {name} received Fire Stage {stage}");

        if (stage == 0)
        {
            // Stage 0: Healthy reference ecosystem -> Hide Invasives
            gameObject.SetActive(false);
        }
        else if (stage == 1)
        {
            // Stage 1: Invasives Spread -> Activate and trigger growth cycle
            gameObject.SetActive(true);

            if (animator != null)
            {
                // Rebind resets the animator state so the growth animation plays fresh
                animator.Rebind();
                animator.Update(0f);
            }
        }
        else if (stage >= 2)
        {
            // Stage 2+: Controlled Fire -> Burn shader handles destruction / stays inactive
            // (If using burn shader directly on mesh, keep active; otherwise set inactive if burned away)
        }
    }
}