using UnityEngine;
using UnityEngine.UI;

public class Invasive_Plant : MonoBehaviour
{
    private Animator animator;
    public Button burnCycle;
    private Object plant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[roots_static] No Animator found on {name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
