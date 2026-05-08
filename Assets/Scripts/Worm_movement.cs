using UnityEngine;

public class Worm_movement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public SpriteRenderer mySpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            increaseTargetInt();
            mySpriteRenderer.flipX = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

        if (targetPoint == 0)
        {
            mySpriteRenderer.flipX = false;
        }
    }

    void increaseTargetInt()
    {
        targetPoint++;
        
        
        if(targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
            
        }
    }
}
