using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA; // First waypoint
    public Transform pointB; // Second waypoint
    public float speed = 2f;

    private Transform target;

    void Start()
    {
        target = pointA; // Start moving towards point A
    }

    void Update()
    {
        // Move enemy towards the current target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the enemy reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Switch target to the other waypoint
            target = target == pointA ? pointB : pointA;
        }
    }
}
