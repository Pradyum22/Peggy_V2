using System.Collections;
using UnityEngine;
using static SceneController;

public class LoadingOverlay : BaseTransitionEffect
{
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    [Header("Optional VFX / Animation")]
    [SerializeField] private ParticleSystem butterflyParticles;
    [SerializeField] private Animator transitionAnimator;

    public override IEnumerator CoverScreen()
    {
        // 1. If artist made a particle system:
        if (butterflyParticles != null)
        {
            butterflyParticles.Play();
            // Wait until screen is covered (e.g. 1.5 seconds)
            yield return new WaitForSeconds(1.5f);
        }
        // 2. If artist made an animation trigger:
        else if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Cover");
            yield return new WaitForSeconds(1.0f);
        }
        // 3. Fallback: Standard Black Fade
        else if (canvasGroup != null)
        {
            yield return FadeTo(1f, fadeInTime);
        }
    }

    public override IEnumerator RevealScreen()
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Reveal");
            yield return new WaitForSeconds(1.0f);
        }
        else if (canvasGroup != null)
        {
            yield return FadeTo(0f, fadeOutTime);
        }

        if (butterflyParticles != null)
        {
            butterflyParticles.Stop();
        }
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (canvasGroup == null) yield break;

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
public abstract class BaseTransitionEffect : MonoBehaviour
{
    public abstract IEnumerator CoverScreen();
    public abstract IEnumerator RevealScreen();
}