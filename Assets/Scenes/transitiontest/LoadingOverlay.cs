using System.Collections;
using UnityEngine;
using static SceneController;

public class LoadingOverlay : BaseTransitionEffect
{
    [SerializeField] private Animator transitionAnimator;

    public override IEnumerator CoverScreen()
    {
     
       
      if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Cover");
            yield return new WaitForSeconds(2.0f);
        }
    
    }

    public override IEnumerator RevealScreen()
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Reveal");
            yield return new WaitForSeconds(2.0f);
        }
    }
    
}
public abstract class BaseTransitionEffect : MonoBehaviour
{
    public abstract IEnumerator CoverScreen();
    public abstract IEnumerator RevealScreen();
}