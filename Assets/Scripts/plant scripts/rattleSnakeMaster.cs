using UnityEngine;
using UnityEngine.UI;

public class rattleSnakeMaster : MonoBehaviour
{
    [Header("Optional test slider (for local play)")]
    public Slider testSlider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[rattleSnakeMaster] No Animator found on {name}");
        }
    }

    private void Start()
    {
        if (testSlider != null)
        {
            testSlider.minValue = -1;
            testSlider.maxValue = 1;
            testSlider.wholeNumbers = true;

            testSlider.onValueChanged.AddListener(OnSliderValueChanged);
            OnSliderValueChanged(testSlider.value);
        }
        // Removed forced SetActive(false) so Unity discovers these objects on launch
    }

    private void Update()
    {
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (testSlider != null)
        {
            testSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        ApplyValue(Mathf.RoundToInt(value), "[rattleSnakeMaster] (local slider)");
    }

    public void OnRemoteSliderUpdate(int value)
    {
        if (testSlider != null)
        {
            testSlider.SetValueWithoutNotify(value);
        }

        ApplyValue(value, "[rattleSnakeMaster] (remote)");
    }

    private void ApplyValue(int intValue, string source)
    {
        if (animator == null) return;

        Debug.Log($"{source} {name} received value {intValue}");

        if (intValue <= -1) // Invasive Species Active (-1)
        {
            gameObject.SetActive(true);
            // Animator returns to default/alive state
        }
        else if (intValue >= 1) // Native Species Active (1)
        {
            animator.SetTrigger("TrDie");
        }
    }
}