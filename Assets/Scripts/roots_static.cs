using UnityEngine;
using UnityEngine.UI;

public class root_static : MonoBehaviour
{
    [Header("Optional test slider (for local play)")]
    public Slider testSlider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[roots_static] No Animator found on {name}");
        }
    }

    private void Start()
    {
        // Optional local slider for testing in the editor
        if (testSlider != null)
        {
            testSlider.minValue = -1;
            testSlider.maxValue = 1;
            testSlider.wholeNumbers = true;

            testSlider.onValueChanged.AddListener(OnSliderValueChanged);
            OnSliderValueChanged(testSlider.value);
        }
    }

    private void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
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

    // LOCAL slider (only for testing in the editor)
    private void OnSliderValueChanged(float value)
    {
        ApplyValue(Mathf.RoundToInt(value), "[roots_static] (local slider)");
    }

    // REMOTE slider � called by DisplayWebSocket
    public void OnRemoteSliderUpdate(int value)
    {
        // Optionally keep the test slider in sync (without triggering its callback)
        if (testSlider != null)
        {
            testSlider.SetValueWithoutNotify(value);
        }

        ApplyValue(value, "[root_static] (remote)");
    }

    private void ApplyValue(int intValue, string source)
    {
        if (animator == null) return;

        Debug.Log($"{source} {name} received value {intValue}");

        // Your friend�s original logic:
        // < 0  -> die
        // > 0  -> (re)appear / grow
        // 0    -> idle (do nothing special)
        
        if (intValue < 0)
        {
            animator.SetTrigger("shrink");
            animator.ResetTrigger("swell");
        }
       
        else if (intValue > 0)
        {
            // Make sure the plant is visible; animator will handle grow/idle
            animator.SetTrigger("swell");
            animator.ResetTrigger("shrink");
        }
        
    }
}
