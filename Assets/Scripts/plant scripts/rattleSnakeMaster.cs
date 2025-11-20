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
    }

    private void OnDestroy()
    {
        if (testSlider != null)
        {
            testSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    // LOCAL slider (editor testing)
    private void OnSliderValueChanged(float value)
    {
        ApplyValue(Mathf.RoundToInt(value), "[rattleSnakeMaster] (local slider)");
    }

    // REMOTE slider – called by DisplayWebSocket
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

        // Friend’s original behaviour:
        // <= 0 -> die
        // >  0 -> alive / grow

        if (intValue <= 0)
        {
            animator.SetTrigger("TrDie");
        }
        else // > 0
        {
            gameObject.SetActive(true);
        }
    }
}
