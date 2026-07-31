using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FireDissolveController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private VisualEffect fireVFX;

    [Header("Dissolve")]
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;

    private Material[] materials;
    private Coroutine dissolveRoutine;

    private void Start()
    {
        if (meshRenderer != null)
            materials = meshRenderer.materials;
    }

    public void PlayBurn()
    {
        if (dissolveRoutine != null)
            StopCoroutine(dissolveRoutine);

        dissolveRoutine = StartCoroutine(DissolveRoutine());
    }

    public void ResetBurn()
    {
        if (dissolveRoutine != null)
            StopCoroutine(dissolveRoutine);

        if (materials == null)
            return;

        foreach (Material mat in materials)
            mat.SetFloat("_DissolveAmount", 0f);

        if (fireVFX != null)
            fireVFX.Stop();
    }

    private IEnumerator DissolveRoutine()
    {
        if (fireVFX != null)
            fireVFX.Play();

        float amount = 0f;

        while (amount < 1f)
        {
            amount += dissolveRate;

            foreach (Material mat in materials)
                mat.SetFloat("_DissolveAmount", amount);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}