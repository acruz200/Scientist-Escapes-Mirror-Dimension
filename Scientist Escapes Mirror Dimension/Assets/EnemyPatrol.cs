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

    public AudioClip chaseClip;

    private Transform target;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private bool hasPlayedChaseSound = false;

    void Start()
    {
        currentState = State.Idle;
        target = pointA;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Auto-assign player if not set
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

        // Set up AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.minDistance = 0.5f;
        audioSource.maxDistance = 1f;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
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
                    PlayChaseSoundOnce();
                }
                break;

            case State.Chasing:
                ChasePlayer();
                if (distanceToPlayer > chaseRange)
                {
                    currentState = State.Idle;
                    ResetPatrol();
                    Debug.Log("Switching to Idle");
                    hasPlayedChaseSound = false; // Allow sound to play again next time
                    audioSource.Stop();
                }
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
            agent.SetDestination(target.position);
            Debug.Log("Patrol: Switching target");
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void ResetPatrol()
    {
        target = (Vector3.Distance(transform.position, pointA.position) < Vector3.Distance(transform.position, pointB.position)) ? pointA : pointB;
        agent.SetDestination(target.position);
        Debug.Log("Resetting Patrol");
    }

    void PlayChaseSoundOnce()
    {
        if (chaseClip != null && audioSource != null && !hasPlayedChaseSound)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(chaseClip);
            hasPlayedChaseSound = true;
        }
    }
}
