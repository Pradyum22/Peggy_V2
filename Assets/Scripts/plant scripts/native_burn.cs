using UnityEngine;
using UnityEngine.UI;

public class native_burn : MonoBehaviour
{
    private Animator animator;
    public Button burnCycle;
    public Object plant;

    private int state = 1;

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

        //deactivating after burn for regrow
        /* if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
         {
             if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
             {
                 gameObject.SetActive(false);
             }
         }
        */
        

    }

    void actions()
    {
    }

    void OnEnable()
    {
        // Add a listener to detect the click event
        burnCycle.onClick.AddListener(OnButtonClicked);
    }

    void OnDisable()
    {
        // Always remove listeners when disabled to prevent memory leaks
        burnCycle.onClick.RemoveListener(OnButtonClicked);
    }

    //button press causing next step of the cycle and reseting
    private void OnButtonClicked()
    {
        state++;
        if (state > 5)
        {
            Debug.Log("cycle reset!");
            state = 1;
        }
        cycleChange();
        
    }
    private void cycleChange()
    {
        //activating animation triggers depending on state of the cycle
        if (state == 1)
        {
            Debug.Log("Invade activated");
            animator.SetTrigger("Invade");
        }
        else if (state == 2)
        {
            Debug.Log("Invade2 activated");
            animator.SetTrigger("Invade2");
        }
        else if (state == 3)
        {
            Debug.Log("Burn activated");
            animator.SetTrigger("Burn");
        }
        else if (state == 4)
        {
            Debug.Log("Regrow activated");
            animator.SetTrigger("Regrow");
        }
        else if (state == 5)
        {
            Debug.Log("Reset activated");
            animator.SetTrigger("Reset");
        }
        
        //tag checks under each if statement! native_root, native_plant, invasive1, invasive2
    }
}
