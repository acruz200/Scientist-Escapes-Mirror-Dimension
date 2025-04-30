using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MirrorGuardianController : MonoBehaviour
{
    [Header("Guardian Settings")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float attackRadius = 5f;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackSpeed = 7f;
    [SerializeField] private float retreatSpeed = 8f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    
    [Header("Visual Effects")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material chaseMaterial;
    [SerializeField] private Material attackMaterial;
    [SerializeField] private Material retreatMaterial;
    [SerializeField] private GameObject mirrorEffect;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private ParticleSystem damageParticles;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool showPathfinding = false;
    
    // Components
    private NavMeshAgent navAgent;
    private MeshRenderer meshRenderer;
    private Animator animator;
    private MirrorGuardianFSM fsm;
    private MirrorGuardianPathfinding pathfinding;
    private MirrorGuardianBayesianNetwork bayesianNetwork;
    
    // References
    private Transform player;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentPatrolIndex = 0;
    
    // State variables
    private float lastAttackTime = 0f;
    private Vector3 lastKnownPlayerPosition;
    private bool playerVisible = false;
    private float timeSinceLastSeenPlayer = 0f;
    private float memoryDuration = 5f; // How long the guardian remembers the player's position
    
    void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            enabled = false;
            return;
        }
        
        // Get or add components
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            navAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }
        
        // Initialize AI components
        fsm = GetComponent<MirrorGuardianFSM>();
        if (fsm == null)
        {
            fsm = gameObject.AddComponent<MirrorGuardianFSM>();
        }
        
        pathfinding = GetComponent<MirrorGuardianPathfinding>();
        if (pathfinding == null)
        {
            pathfinding = gameObject.AddComponent<MirrorGuardianPathfinding>();
        }
        
        bayesianNetwork = GetComponent<MirrorGuardianBayesianNetwork>();
        if (bayesianNetwork == null)
        {
            bayesianNetwork = gameObject.AddComponent<MirrorGuardianBayesianNetwork>();
        }
        
        // Find patrol points
        GameObject[] patrolPointObjects = GameObject.FindGameObjectsWithTag("PatrolPoint");
        foreach (GameObject point in patrolPointObjects)
        {
            patrolPoints.Add(point.transform);
        }
        
        // If no patrol points found, create a default patrol behavior
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points found. Creating default patrol behavior.");
            CreateDefaultPatrolPoints();
        }
        
        // Initialize the guardian
        InitializeGuardian();
    }
    
    void Update()
    {
        // Update player visibility
        UpdatePlayerVisibility();
        
        // Update the FSM
        fsm.UpdateState(this);
        
        // Update the Bayesian network with player movement data
        if (playerVisible)
        {
            bayesianNetwork.UpdateWithPlayerData(player.position, player.forward);
        }
        
        // Update pathfinding visualization if debug is enabled
        if (showPathfinding)
        {
            pathfinding.VisualizePath();
        }
    }
    
    private void InitializeGuardian()
    {
        // Set initial material
        if (normalMaterial != null)
        {
            meshRenderer.material = normalMaterial;
        }
        
        // Configure NavMeshAgent
        navAgent.speed = patrolSpeed;
        navAgent.angularSpeed = rotationSpeed * 10f;
        navAgent.stoppingDistance = 1.5f;
        
        // Set initial state
        fsm.SetState(MirrorGuardianState.Patrol);
        
        // Set initial destination
        if (patrolPoints.Count > 0)
        {
            SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    
    private void UpdatePlayerVisibility()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Check if player is within detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            // Check if player is visible (not behind obstacles)
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
            {
                if (hit.transform == player)
                {
                    playerVisible = true;
                    lastKnownPlayerPosition = player.position;
                    timeSinceLastSeenPlayer = 0f;
                    return;
                }
            }
        }
        
        // Player is not visible
        playerVisible = false;
        timeSinceLastSeenPlayer += Time.deltaTime;
    }
    
    private void CreateDefaultPatrolPoints()
    {
        // Create 4 patrol points in a square around the guardian
        float patrolRadius = 10f;
        
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f * Mathf.Deg2Rad;
            Vector3 position = transform.position + new Vector3(
                Mathf.Cos(angle) * patrolRadius,
                0,
                Mathf.Sin(angle) * patrolRadius
            );
            
            GameObject patrolPoint = new GameObject("DefaultPatrolPoint_" + i);
            patrolPoint.transform.position = position;
            patrolPoint.tag = "PatrolPoint";
            
            patrolPoints.Add(patrolPoint.transform);
        }
    }
    
    // Public methods for FSM to call
    
    public void Patrol()
    {
        // Set speed for patrol
        navAgent.speed = patrolSpeed;
        
        // Set material for patrol
        if (normalMaterial != null)
        {
            meshRenderer.material = normalMaterial;
        }
        
        // Ensure we have patrol points and the agent is ready
        if (patrolPoints.Count == 0)
        {
            // Optionally, log a warning or have the guardian stand still
            // Debug.LogWarning($"{name}: No patrol points assigned.");
            return; // Cannot patrol without points
        }

        // Check if the agent is ready and has reached the destination (or close enough)
        // Add checks for navAgent.isOnNavMesh and navAgent.hasPath
        if (navAgent.isOnNavMesh && navAgent.isActiveAndEnabled && !navAgent.pathPending)
        {
            // Only check remaining distance if a path exists
            if (!navAgent.hasPath || navAgent.remainingDistance < 0.5f)
            {
                // Safely get the next index
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; 
                SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
        // else
        // {
            // Agent might still be initializing or pathfinding, wait...
            // Debug.Log("Agent not ready or path pending...");
        // }
    }
    
    public void Chase()
    {
        // Set speed for chase
        navAgent.speed = chaseSpeed;
        
        // Set material for chase
        if (chaseMaterial != null)
        {
            meshRenderer.material = chaseMaterial;
        }
        
        // If player is visible, chase them directly
        if (playerVisible)
        {
            SetDestination(player.position);
        }
        // Otherwise, use the last known position or predicted position
        else if (timeSinceLastSeenPlayer < memoryDuration)
        {
            // Use Bayesian network to predict player position
            Vector3 predictedPosition = bayesianNetwork.PredictPlayerPosition();
            SetDestination(predictedPosition);
        }
    }
    
    public void Attack()
    {
        // Set speed for attack
        navAgent.speed = attackSpeed;
        
        // Set material for attack
        if (attackMaterial != null)
        {
            meshRenderer.material = attackMaterial;
        }
        
        // Face the player
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        
        // Attack the player if in range and cooldown has passed
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
            }
        }
    }
    
    public void Retreat()
    {
        // Set speed for retreat
        navAgent.speed = retreatSpeed;
        
        // Set material for retreat
        if (retreatMaterial != null)
        {
            meshRenderer.material = retreatMaterial;
        }
        
        // Find a position away from the player
        if (player != null)
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            Vector3 retreatPosition = transform.position + directionAwayFromPlayer * 10f;
            
            // Use pathfinding to find a valid retreat position
            Vector3 validRetreatPosition = pathfinding.FindValidPosition(retreatPosition);
            SetDestination(validRetreatPosition);
        }
    }
    
    private void PerformAttack()
    {
        lastAttackTime = Time.time;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Play attack particles
        if (attackParticles != null)
        {
            attackParticles.Play();
        }
        
        // Deal damage to player (implement in PlayerHealth script)
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10f);
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        // Play damage particles
        if (damageParticles != null)
        {
            damageParticles.Play();
        }
        
        // Check if guardian is defeated
        if (health <= 0)
        {
            Defeat();
        }
    }
    
    private void Defeat()
    {
        // Disable the guardian
        enabled = false;
        
        // Play defeat animation
        if (animator != null)
        {
            animator.SetTrigger("Defeat");
        }
        
        // Disable components
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }
        
        // Disable collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        // Destroy after delay
        Destroy(gameObject, 3f);
    }
    
    private void SetDestination(Vector3 destination)
    {
        if (navAgent != null && navAgent.isActiveAndEnabled)
        {
            navAgent.SetDestination(destination);
        }
    }
    
    // Getters for FSM
    
    public bool IsPlayerVisible()
    {
        return playerVisible;
    }
    
    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Vector3.Distance(transform.position, player.position);
    }
    
    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }
    
    public Vector3 GetLastKnownPlayerPosition()
    {
        return lastKnownPlayerPosition;
    }
    
    public float GetTimeSinceLastSeenPlayer()
    {
        return timeSinceLastSeenPlayer;
    }
    
    // OnDrawGizmos for debugging
    void OnDrawGizmos()
    {
        if (!showDebugInfo) return;
        
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Draw attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        
        // Draw line to player if visible
        if (player != null && playerVisible)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
} 