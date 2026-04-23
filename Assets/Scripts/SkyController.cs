using UnityEngine;

public class SkyController : MonoBehaviour
{
    public GameObject rainySky;

    public void SetRainState(int state)
    {
        if (state == 1)
        {
            rainySky.SetActive(true);
        }
        else
        {
            rainySky.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
