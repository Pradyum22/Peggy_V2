using UnityEngine;

public class WormController : MonoBehaviour
{
    public GameObject[] worms;

    public void SetWormState(int state)
    {
        int wormsToShow = 0;

        switch (state)
        {
            case -1:
                wormsToShow = 0;
                break;

            case 0:
                wormsToShow = 2;
                break;

            case 1:
                wormsToShow = worms.Length;
                break;
        }

        for (int i = 0; i < worms.Length; i++)
        {
            worms[i].SetActive(i < wormsToShow);
        }
    }
}