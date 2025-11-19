using UnityEngine;
using UnityEngine.UI;



public class SliderAnimationController : MonoBehaviour
{
    public Slider slider;
    public Slider slider2;  // Reference to the UI Slider
    public Animator animator;          // Reference to the Animator

    private int lastValue = 999;       // Store last used value to prevent repeat triggers

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

    void OnSliderValueChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        if (intValue == lastValue)
            return; // No change

        lastValue = intValue;

        switch (intValue)
        {
            case -1:
                animator.SetTrigger("TrGrow");
                break;
            case 0:
                animator.SetTrigger("TrIdle");
                break;
            case 1:
                animator.SetTrigger("TrDie");
                break;
        }
    }
}