using UnityEngine;

public class Texture_Change : MonoBehaviour
{

    public Material newMaterial;
    public Renderer DryGrassRender;
    public float fadeSpeed = 0.4f;

    private float targetAlpha = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        newMaterial = DryGrassRender.material;

        Color c = newMaterial.color;
        c.a = 0f;
        newMaterial.color = c;
    }
    public void ChangeMaterial(int state)
    {
        Renderer rend = GetComponent<Renderer>();

        if (state == -1)
        {
            targetAlpha = 0f;

        }
        else if (state == 0)
        {
            targetAlpha = 0f;

        }
        else if (state == 1)
        {
            targetAlpha = 1f;

        }

    }
    void Update()
    {
        Color c = newMaterial.color;

        c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);

        newMaterial.color = c;
    }

}
