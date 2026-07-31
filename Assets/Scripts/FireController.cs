using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Effect")]
    [SerializeField] private FireDissolveController dissolveController;

    public void SetFireState(int state)
    {
        switch (state)
        {
            // Native Prairie
            case 0:

                if (dissolveController != null)
                    dissolveController.ResetBurn();

                Debug.Log("Fire Stage 0 - Native Prairie");

                break;

            // Invasive Plants
            case 1:

                if (dissolveController != null)
                    dissolveController.ResetBurn();

                Debug.Log("Fire Stage 1 - Invasive Plants");

                break;

            // Controlled Burn
            case 2:

                if (dissolveController != null)
                    dissolveController.PlayBurn();

                Debug.Log("Fire Stage 2 - Controlled Burn");

                break;

            // Recovery
            case 3:

                if (dissolveController != null)
                    dissolveController.ResetBurn();

                Debug.Log("Fire Stage 3 - Recovery");

                break;
        }
    }
}