using UnityEngine;
using UnityEngine.UI;



public class rattleSnakeMaster : MonoBehaviour
{
    public Slider slider;  // Reference to the UI Slider
    public Animator animator;          // Reference to the Animator
    public GameObject plant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (slider == null || animator == null)
        {
            Debug.LogError("Slider or Animator not assigned.");
            return;
        }

        slider.minValue = -1;
        slider.maxValue = 1;
        slider.wholeNumbers = true;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(slider.value); // Trigger initial value
    }

    // Update is called once per frame
    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
        {
            gameObject.SetActive(false);
            OnSliderValueChanged(slider.value); //call slider value changed again in case death animation isnt done playing before slider changes to "grow"
        }
    }
    void OnSliderValueChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        if (intValue <= 0)
        {
            animator.SetTrigger("TrDie");

        }
        if (intValue > 0)
        {
            gameObject.SetActive(true);
        }
    }
}