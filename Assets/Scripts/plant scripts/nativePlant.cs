using UnityEngine;
using UnityEngine.UI;

public class nativePlant : MonoBehaviour
{
    [Header("Optional test slider (for local play)")]
    public Slider testSlider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[nativePlant] No Animator found on {name}");
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
            if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("isdead"))
    {
                gameObject.SetActive(false);
            }
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
        ApplyValue(Mathf.RoundToInt(value), "[nativePlant] (local slider)");
    }

    // REMOTE slider � called by DisplayWebSocket
    public void OnRemoteSliderUpdate(int value)
    {
        // Optionally keep the test slider in sync (without triggering its callback)
        if (testSlider != null)
        {
            testSlider.SetValueWithoutNotify(value);
        }

        ApplyValue(value, "[nativePlant] (remote)");
    }

    private void ApplyValue(int intValue, string source)
    {
        if (animator == null) return;

        Debug.Log($"{source} {name} received value {intValue}");

        if (intValue < 0)
        {
            animator.SetTrigger("TrDie");
        }
        else if (intValue > 0)
        {
            gameObject.SetActive(true);
        }



    }
}
