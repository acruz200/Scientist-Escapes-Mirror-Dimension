using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    enum State { Idle, Chasing }
    private State currentState;

    public Transform pointA; // First waypoint
    public Transform pointB; // Second waypoint
    public Transform player; // Reference to the player
    public float speed = 2f;
    public float chaseRange = 5f; // Distance to start chasing

    private Transform target;
    private NavMeshAgent agent;

   void Start()
{
    currentState = State.Idle;
    target = pointA; // Start moving towards point A
    agent = GetComponent<NavMeshAgent>();
    agent.speed = speed;

    // 🔧 Automatically assign the player if not set in Inspector
    if (player == null)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("⚠️ EnemyPatrol could not find a GameObject tagged 'Player'.");
        }
    }
}


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                Patrol();
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = State.Chasing;
                    Debug.Log("Switching to Chasing");
                }
                break;

            case State.Chasing:
                ChasePlayer();
                if (distanceToPlayer > chaseRange)
                {
                    currentState = State.Idle;
                    ResetPatrol(); // Properly reset the patrol when switching back
                    Debug.Log("Switching to Idle");
                }
                break;
        }
    }

   void Patrol()
{
    // Only set destination if we're not already going to the target
    if (!agent.pathPending && agent.remainingDistance < 0.1f)
    {
        // Switch target to the other waypoint
        target = (target == pointA) ? pointB : pointA;
        agent.SetDestination(target.position);
        Debug.Log("Patrol: Switching target");
    }
}


    void ChasePlayer()
    {
        // Move towards the player using NavMeshAgent
        agent.SetDestination(player.position);
    }

    void ResetPatrol()
    {
        // Ensure the patrol resumes from the current target
        target = (Vector3.Distance(transform.position, pointA.position) < Vector3.Distance(transform.position, pointB.position)) ? pointA : pointB;
        agent.SetDestination(target.position);
        Debug.Log("Resetting Patrol");
    }
}
