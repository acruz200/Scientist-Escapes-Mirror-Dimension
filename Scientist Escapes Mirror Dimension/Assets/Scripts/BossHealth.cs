using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic; // Needed for List

// Define the states the boss can be in
public enum BossState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Retreat, // Optional: Add retreat logic if needed
    Dead
}

// Combine Health and AI Controller for the Boss
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
// Add Animator requirement if animations are directly controlled here
[RequireComponent(typeof(Animator))] 
public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 500f; // Boss health set to 500
    [SerializeField] private float currentHealth;
    // [SerializeField] private float retreatHealthThreshold = 0.2f; // Example: Retreat when health is below 20%

    [Header("AI Settings")]
    [SerializeField] private BossState currentState = BossState.Idle;
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float memoryDuration = 5f; // How long the boss remembers the player's position after losing sight

    [Header("Movement Speeds")]
    [SerializeField] private float patrolSpeed = 2.5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackMovementSpeed = 0f; // Boss might stop to attack
    // [SerializeField] private float retreatSpeed = 6f; // Speed for retreating

    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] private bool createDefaultPatrolPoints = true;
    [SerializeField] private float defaultPatrolRadius = 15f;

    [Header("Effects & Feedback")]
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound; // Optional: Sound for attacking
    // Materials for different states (optional)
    // [SerializeField] private Material normalMaterial;
    // [SerializeField] private Material chaseMaterial;
    // [SerializeField] private Material attackMaterial;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    // --- Component References ---
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Animator animator; // Reference to the Animator
    // private MeshRenderer meshRenderer; // If using materials

    // --- References ---
    private Transform player;

    // --- Internal State ---
    private bool isDead = false;
    private float lastAttackTime = -Mathf.Infinity; // Initialize to allow immediate attack if conditions met
    private int currentPatrolIndex = 0;
    private Vector3 lastKnownPlayerPosition;
    private bool playerVisible = false;
    private float timeSinceLastSeenPlayer = Mathf.Infinity;

    // --- Animator Parameters (match your Animator Controller) ---
    private const string AnimIsMoving = "isMoving";
    private const string AnimIsDead = "isDead";
    private const string AnimAttackTrigger = "Attack"; // Example attack trigger

    //=================================
    // Initialization
    //=================================

    void Awake()
    {
        // Get required components
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        // meshRenderer = GetComponent<MeshRenderer>(); // If using materials

        if (agent == null) Debug.LogError($"BossHealth on {name} requires a NavMeshAgent component.", this);
        if (audioSource == null) Debug.LogError($"BossHealth on {name} requires an AudioSource component.", this);
        if (animator == null) Debug.LogError($"BossHealth on {name} requires an Animator component.", this);
    }

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        // Find Player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError($"BossHealth on {name}: Player not found! Make sure the player GameObject has the 'Player' tag.", this);
            enabled = false; // Disable script if player not found
            return;
        }

        // Setup Patrol Points
        if (patrolPoints.Count == 0 && createDefaultPatrolPoints)
        {
            CreateDefaultPatrolPoints();
        }

        // Configure Agent Defaults (can be overridden by states)
        agent.speed = patrolSpeed;
        agent.stoppingDistance = attackRadius * 0.8f; // Stop slightly before attack range
        agent.angularSpeed = rotationSpeed * 120; // NavMeshAgent angular speed is in deg/sec

        // Set initial state
        ChangeState(BossState.Patrol); // Start patrolling
    }

    //=================================
    // Update Loop & State Machine
    //=================================

    void Update()
    {
        if (isDead || player == null) return; // Don't update if dead or player missing

        // 1. Update Sensory Info
        UpdatePlayerVisibility();
        UpdateAnimatorMovement(); // Update movement animation parameter

        // 2. State Machine Logic
        ExecuteCurrentState();
        CheckStateTransitions(); // Decide if we should change state
    }

    void ExecuteCurrentState()
    {
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;
            case BossState.Patrol:
                HandlePatrolState();
                break;
            case BossState.Chase:
                HandleChaseState();
                break;
            case BossState.Attack:
                HandleAttackState();
                break;
            // case BossState.Retreat:
            //     HandleRetreatState();
            //     break;
            case BossState.Dead:
                // Logic is mostly handled in Die(), but can add checks here if needed
                break;
        }
    }

    void CheckStateTransitions()
    {
        // Transitions only matter if not attacking or dead
        if (currentState == BossState.Attack || currentState == BossState.Dead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // --- Check for Death ---
        // (Handled via TakeDamage -> Die -> ChangeState(Dead))

        // --- Check for Attack ---
        // If player is visible and close enough, ATTACK
        if (playerVisible && distanceToPlayer <= attackRadius)
        {
            ChangeState(BossState.Attack);
            return;
        }

        // --- Check for Chase ---
        // If player is visible OR recently seen and outside attack range, CHASE
        if (playerVisible || timeSinceLastSeenPlayer < memoryDuration)
        {
             if (currentState != BossState.Chase) // Only switch if not already chasing
             {
                 ChangeState(BossState.Chase);
                 return;
             }
        }
        // --- Check for Patrol ---
        // If player is not visible and memory has faded, PATROL
        else if (timeSinceLastSeenPlayer >= memoryDuration)
        {
             if (currentState != BossState.Patrol) // Only switch if not already patrolling
             {
                ChangeState(BossState.Patrol);
                return;
             }
        }

        // --- Check for Retreat (Optional) ---
        // if (currentHealth / maxHealth <= retreatHealthThreshold)
        // {
        //     ChangeState(BossState.Retreat);
        //     return;
        // }
    }


    void ChangeState(BossState newState)
    {
        if (currentState == newState || isDead) return; // Don't transition to the same state or if dead

        // Exit logic for the previous state (optional cleanup)
        // switch (currentState) { ... }

        if (showDebugInfo) Debug.Log($"{name}: Changing state from {currentState} to {newState}");

        currentState = newState;
        agent.isStopped = false; // Ensure agent can move unless explicitly stopped by state logic

        // Entry logic for the new state
        switch (currentState)
        {
            case BossState.Idle:
                agent.speed = 0;
                agent.isStopped = true;
                // SetMaterial(normalMaterial);
                break;
            case BossState.Patrol:
                agent.speed = patrolSpeed;
                StartPatrolling(); // Set initial patrol destination
                // SetMaterial(normalMaterial);
                break;
            case BossState.Chase:
                agent.speed = chaseSpeed;
                // SetMaterial(chaseMaterial);
                break;
            case BossState.Attack:
                agent.speed = attackMovementSpeed; // May stop or slow down to attack
                if (attackMovementSpeed <= 0.01f) agent.isStopped = true; // Stop if speed is near zero
                agent.velocity = Vector3.zero; // Clear velocity immediately if stopping
                // SetMaterial(attackMaterial);
                break;
            // case BossState.Retreat:
            //     agent.speed = retreatSpeed;
            //     // SetMaterial(retreatMaterial); // Assuming a retreat material
            //     StartRetreating();
            //     break;
            case BossState.Dead:
                agent.isStopped = true;
                agent.enabled = false; // Disable agent completely
                // Death effects handled in Die()
                break;
        }
    }

    //=================================
    // State Handling Methods
    //=================================

    void HandleIdleState()
    {
        // Boss does nothing, waits for player or timer
        // Transitions checked in CheckStateTransitions()
    }

    void HandlePatrolState()
    {
        // Move between patrol points
        if (patrolPoints.Count == 0)
        {
            ChangeState(BossState.Idle); // Can't patrol without points
            return;
        }

        // Check if agent needs a destination or has arrived
        if (agent.isOnNavMesh && agent.isActiveAndEnabled && !agent.pathPending)
        {
            if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance + 0.1f) // Use stopping distance
            {
                StartPatrolling(); // Go to next point
            }
        }
    }

     void StartPatrolling()
     {
         if (patrolPoints.Count == 0) return;
         currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
         SetDestination(patrolPoints[currentPatrolIndex].position);
         if (showDebugInfo) Debug.Log($"{name}: Patrolling to point {currentPatrolIndex}");
     }

    void HandleChaseState()
    {
        // Move towards player or last known position
        if (playerVisible)
        {
            SetDestination(player.position);
        }
        else if (timeSinceLastSeenPlayer < memoryDuration)
        {
            // Continue towards last known position
            SetDestination(lastKnownPlayerPosition);
        }
        else
        {
            // Should have transitioned back to Patrol already via CheckStateTransitions
             ChangeState(BossState.Patrol);
        }
    }

    void HandleAttackState()
    {
         // Stop moving (or slow down based on attackMovementSpeed)
         if (agent.isOnNavMesh && agent.isActiveAndEnabled)
         {
             if (attackMovementSpeed <= 0.01f && !agent.isStopped)
             {
                 agent.isStopped = true;
                 agent.velocity = Vector3.zero;
             }
             else if (attackMovementSpeed > 0.01f)
             {
                 agent.isStopped = false; // Allow slow movement during attack
                 // Optionally, still move towards player if moving while attacking
                 // SetDestination(player.position);
             }
         }


        // Face the player
        FaceTarget(player.position);

        // Check if can attack (cooldown)
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }

        // Transition out of attack if player moves out of range or becomes invisible
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!playerVisible || distanceToPlayer > attackRadius)
        {
            // Decide whether to chase or patrol based on visibility/memory
            if (playerVisible || timeSinceLastSeenPlayer < memoryDuration)
                ChangeState(BossState.Chase);
            else
                ChangeState(BossState.Patrol);
        }
    }

    // void HandleRetreatState()
    // {
    //     // Logic for moving away from the player
    //     if (agent.remainingDistance < 1.0f) // Reached retreat point
    //     {
    //         // Decide next action: maybe back to patrol or idle if far enough
    //          ChangeState(BossState.Patrol);
    //     }
    //     // If player gets close again while retreating, maybe switch back to chase/attack?
    // }

    // void StartRetreating()
    // {
    //     if(player == null) return;
    //     Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
    //     Vector3 targetRetreatPos = transform.position + directionAwayFromPlayer * (detectionRadius * 0.5f); // Retreat some distance away

    //     NavMeshHit hit;
    //     if (NavMesh.SamplePosition(targetRetreatPos, out hit, 5.0f, NavMesh.AllAreas))
    //     {
    //          SetDestination(hit.position);
    //          if (showDebugInfo) Debug.Log($"{name}: Retreating to {hit.position}");
    //     } else {
    //         if (showDebugInfo) Debug.Log($"{name}: Could not find valid retreat position.");
    //          ChangeState(BossState.Chase); // Fallback if retreat fails
    //     }
    // }

    //=================================
    // Core Actions & Helpers
    //=================================

    void PerformAttack()
    {
        if (player == null) return;

        lastAttackTime = Time.time;
        if (showDebugInfo) Debug.Log($"{name}: Attacking Player!");

        // Trigger animation
        if (animator != null)
        {
            animator.SetTrigger(AnimAttackTrigger);
        }

        // Play effects
        if (attackParticles != null)
        {
            attackParticles.Play();
        }
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Deal Damage (Get PlayerHealth component and call its TakeDamage)
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Define boss's attack damage somewhere (e.g., serialize field)
            float attackDamage = 25f; // Example damage
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning($"{name}: Could not find PlayerHealth component on Player to deal damage.", player);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (showDebugInfo) Debug.Log($"{name} took {amount} damage, health remaining: {currentHealth}");

        // Play damage feedback
        if (damageParticles != null) damageParticles.Play();
        if (damageSound != null && audioSource != null) audioSource.PlayOneShot(damageSound);

        // Trigger "Hit" animation? (Optional)
        // if (animator != null) animator.SetTrigger("Hit");

        // Check for death
        if (currentHealth <= 0f)
        {
            currentHealth = 0f; // Prevent negative health display
            Die();
        }
        // Check for retreat condition (Optional)
        // else if (currentState != BossState.Retreat && currentHealth / maxHealth <= retreatHealthThreshold)
        // {
        //      ChangeState(BossState.Retreat);
        // }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{name} (Boss) has been defeated!");
        ChangeState(BossState.Dead); // Ensure state is Dead

        // Add score bonus
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddPoints(1000); // Boss kill score
        }

        // Play death animation
        if (animator != null)
        {
            animator.SetBool(AnimIsMoving, false); // Stop movement anim
            animator.SetBool(AnimIsDead, true);    // Trigger death anim
        }

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Disable components
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Agent already stopped/disabled in ChangeState(Dead)

        // Destroy after delay (allows animation/sound to play)
        Destroy(gameObject, 5.0f);
    }

    //=================================
    // Utility Methods
    //=================================

     void SetDestination(Vector3 destination)
     {
         if (agent.isOnNavMesh && agent.isActiveAndEnabled)
         {
             agent.SetDestination(destination);
             agent.isStopped = false; // Make sure agent moves towards new destination
         }
         else if (showDebugInfo)
         {
             Debug.LogWarning($"{name}: Cannot set destination - Agent not on NavMesh or not active/enabled.");
         }
     }

    void FaceTarget(Vector3 targetPosition)
    {
        if (!agent.isOnNavMesh || !agent.updateRotation) return; // Only face if agent controls rotation

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero) // Prevent LookRotation error with zero vector
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Ignore Y difference
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdatePlayerVisibility()
    {
        if (player == null)
        {
            playerVisible = false;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool previousVisibility = playerVisible;
        playerVisible = false; // Assume not visible unless proven otherwise

        if (distanceToPlayer <= detectionRadius)
        {
            // Use a sphere cast or simple raycast for visibility check
            RaycastHit hit;
            // Use eye height or center of boss for raycast origin
            Vector3 rayOrigin = transform.position + Vector3.up * (agent.height * 0.75f); // Approx eye level
            Vector3 directionToPlayer = (player.position - rayOrigin).normalized;

            // Check line of sight
            // Increase layer mask sensitivity if needed (e.g., ignore certain layers)
            if (Physics.Raycast(rayOrigin, directionToPlayer, out hit, detectionRadius))
            {
                if (hit.transform == player)
                {
                    playerVisible = true;
                    lastKnownPlayerPosition = player.position;
                    timeSinceLastSeenPlayer = 0f;
                    if (!previousVisibility && showDebugInfo) Debug.Log($"{name}: Player spotted!");
                }
                 // else: Obstacle hit first
            }
             // else: Raycast hit nothing within range (unlikely if player is within radius, but possible)
        }

        if (!playerVisible)
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            if (previousVisibility && showDebugInfo) Debug.Log($"{name}: Lost sight of player.");
        }
    }

    void CreateDefaultPatrolPoints()
    {
        patrolPoints.Clear(); // Clear any existing points first
        if (showDebugInfo) Debug.Log($"{name}: Creating default patrol points around origin {transform.position} with radius {defaultPatrolRadius}.");

        for (int i = 0; i < 4; i++) // Create 4 points
        {
            float angle = i * Mathf.PI / 2; // 0, 90, 180, 270 degrees
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * defaultPatrolRadius;
            Vector3 targetPos = transform.position + offset;

            // Try to find a valid position on the NavMesh near the target
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, 2.0f, NavMesh.AllAreas)) // Search within 2 units
            {
                GameObject pp = new GameObject($"DefaultPatrolPoint_{i}");
                pp.transform.position = hit.position;
                pp.transform.SetParent(null); // Don't parent to boss
                patrolPoints.Add(pp.transform);
                 if (showDebugInfo) Debug.Log($"- Created point {i} at {hit.position}");
            }
            else
            {
                 Debug.LogWarning($"{name}: Could not find valid NavMesh position near {targetPos} for default patrol point {i}.");
            }
        }
         if (patrolPoints.Count == 0)
             Debug.LogError($"{name}: Failed to create any valid default patrol points!", this);
    }

    void UpdateAnimatorMovement()
    {
        if (animator == null || !agent.isOnNavMesh) return;

        // Use desired velocity projected onto forward direction, or simple magnitude
        float speedPercent = agent.velocity.magnitude / agent.speed; // Use agent's current max speed
        animator.SetBool(AnimIsMoving, speedPercent > 0.1f); // Set bool based on movement
        // Alternatively, set a float parameter:
        // animator.SetFloat("SpeedPercent", speedPercent, 0.1f, Time.deltaTime);
    }

    // Optional: Set material based on state
    // void SetMaterial(Material mat)
    // {
    //     if (meshRenderer != null && mat != null)
    //     {
    //         meshRenderer.material = mat;
    //     }
    // }

    //=================================
    // Gizmos for Debugging
    //=================================
    void OnDrawGizmos()
    {
        if (!showDebugInfo) return;

        // Detection Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Current Destination
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, agent.destination);
            Gizmos.DrawWireSphere(agent.destination, 0.5f);
        }

        // Line to Player if Visible
        if (player != null && playerVisible)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
        // Line to Last Known Position if remembering
        else if (player != null && timeSinceLastSeenPlayer < memoryDuration)
        {
            Gizmos.color = Color.magenta;
             Gizmos.DrawLine(transform.position, lastKnownPlayerPosition);
             Gizmos.DrawWireSphere(lastKnownPlayerPosition, 0.3f);
        }

        // Patrol Points
        Gizmos.color = Color.blue;
        foreach (Transform point in patrolPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.3f);
        }
    }

    //=================================
    // Public Getters for UI / Other Scripts
    //=================================

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
} 