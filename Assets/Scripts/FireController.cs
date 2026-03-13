using UnityEngine;

public class FireController : MonoBehaviour
{
    public ParticleSystem fireLow;
    public ParticleSystem fireMedium;
    public ParticleSystem fireHigh;

    public void UpdateFire(float fireValue)
    {
        if (fireValue == -1)
        {
            PlayOnly(fireLow);
        }
        else if (fireValue == 0)
        {
            PlayOnly(fireMedium);
        }
        else if (fireValue == 1)
        {
            PlayOnly(fireHigh);
        }
    }

    void PlayOnly(ParticleSystem activeSystem)
    {
        ParticleSystem[] systems = { fireLow, fireMedium, fireHigh };

        foreach (ParticleSystem ps in systems)
        {
            if (ps == activeSystem)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
            else
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
        }
    }
}