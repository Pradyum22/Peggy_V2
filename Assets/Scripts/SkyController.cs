using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Renderer rainySkyRenderer;
    public GameObject clouds;

    public float fadeSpeed = 0.4f;

    private float targetAlpha = 0f;
    private Material skyMaterial;

    void Start()
    {
        skyMaterial = rainySkyRenderer.material;

        Color c = skyMaterial.color;
        c.a = 0f;
        skyMaterial.color = c;
    }

    public void SetRainState(int state)
    {
        if (state == -1)
        {
            targetAlpha = 0f;

            if (clouds != null)
                clouds.SetActive(true);
        }
        else if (state == 0)
        {
            targetAlpha = 1f;

            if (clouds != null)
                clouds.SetActive(false);
        }
        else if (state == 1)
        {
            targetAlpha = 1f;

            if (clouds != null)
                clouds.SetActive(false);
        }
    }

    void Update()
    {
        Color c = skyMaterial.color;

        c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);

        skyMaterial.color = c;
    }
}