using UnityEngine;

public class WaterLevelController : MonoBehaviour
{
    public Transform waterPlane;

    public float lowRainY = 0.4f;
    public float midRainY = 0.4305f;
    public float highRainY = 0.437f;

    public float riseSpeed = 0.02f;

    private float targetY;

    void Start()
    {
        targetY = waterPlane.position.y;
    }

    public void SetWaterLevel(int state)
    {
        switch (state)
        {
            case -1:
                targetY = lowRainY;
                break;

            case 0:
                targetY = midRainY;
                break;

            case 1:
                targetY = highRainY;
                break;
        }
    }

    void Update()
    {
        Vector3 pos = waterPlane.position;

        pos.y = Mathf.MoveTowards(pos.y, targetY, riseSpeed * Time.deltaTime);

        waterPlane.position = pos;
    }
}