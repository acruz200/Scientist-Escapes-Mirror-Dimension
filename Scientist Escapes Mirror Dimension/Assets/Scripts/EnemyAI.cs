using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 15f;
    public float attackRange = 8f;
    public float patrolRadius = 10f;
    public float waitTime = 2f;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 3f;
    
    [Header("Combat Settings")]
    public float health = 100f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    
    // AI State
    private enum AIState { Patrol, Chase, Attack, TakeCover, Stunned }
    private AIState currentState = AIState.Patrol;
    
    // References
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 startPosition;
    private float stateTimer = 0f;
    private float attackTimer = 0f;
    private bool isTakingCover = false;
    
    // For pathfinding
    private Vector3 currentPatrolTarget;
    private float coverTimer = 0f;
    private float stunTimer = 0f;
    
    void Start()
    {
        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Find player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag.");
        }
        
        // Store initial position
        startPosition = transform.position;
        
        // Set initial patrol target
        SetNewPatrolTarget();
        
        // Configure NavMeshAgent
        if (agent != null)
        {
            agent.speed = patrolSpeed;
            agent.stoppingDistance = 1.5f;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        // Update state timer
        stateTimer += Time.deltaTime;
        
        // Check for plasma bullets nearby
        CheckForPlasmaBullets();
        
        // State machine
        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState();
                break;
            case AIState.Chase:
                UpdateChaseState();
                break;
            case AIState.Attack:
                UpdateAttackState();
                break;
            case AIState.TakeCover:
                UpdateTakeCoverState();
                break;
            case AIState.Stunned:
                UpdateStunnedState();
                break;
        }
        
        // Update animations
        UpdateAnimations();
    }
    
    void UpdatePatrolState()
    {
        // Check if we've reached the patrol target
        if (agent.remainingDistance < 0.5f)
        {
            // Wait at the current position
            if (stateTimer < waitTime)
            {
                agent.isStopped = true;
            }
            else
            {
                // Set a new patrol target
                SetNewPatrolTarget();
                stateTimer = 0f;
            }
        }
        
        // Check if player is in detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRange)
        {
            // Check if there's a clear line of sight to the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    // Player spotted! Switch to chase state
                    currentState = AIState.Chase;
                    agent.speed = chaseSpeed;
                    stateTimer = 0f;
                }
            }
        }
    }
    
    void UpdateChaseState()
    {
        // Update destination to player position
        agent.SetDestination(player.position);
        agent.isStopped = false;
        
        // Check if we're close enough to attack
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            currentState = AIState.Attack;
            stateTimer = 0f;
        }
        
        // Check if player is too far away
        if (distanceToPlayer > detectionRange * 1.5f)
        {
            currentState = AIState.Patrol;
            agent.speed = patrolSpeed;
            SetNewPatrolTarget();
            stateTimer = 0f;
        }
    }
    
    void UpdateAttackState()
    {
        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
        // Attack cooldown
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            // Perform attack
            AttackPlayer();
            attackTimer = 0f;
        }
        
        // Check if player is still in range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            currentState = AIState.Chase;
            stateTimer = 0f;
        }
    }
    
    void UpdateTakeCoverState()
    {
        coverTimer += Time.deltaTime;
        
        // After taking cover for a while, resume previous behavior
        if (coverTimer > 3f)
        {
            isTakingCover = false;
            coverTimer = 0f;
            
            // Return to previous state (usually patrol or chase)
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < detectionRange)
            {
                currentState = AIState.Chase;
            }
            else
            {
                currentState = AIState.Patrol;
                SetNewPatrolTarget();
            }
        }
    }
    
    void UpdateStunnedState()
    {
        stunTimer += Time.deltaTime;
        
        // After being stunned, return to patrol
        if (stunTimer > 2f)
        {
            currentState = AIState.Patrol;
            stunTimer = 0f;
            SetNewPatrolTarget();
        }
    }
    
    void SetNewPatrolTarget()
    {
        // Find a random point within patrol radius
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += startPosition;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            currentPatrolTarget = hit.position;
            agent.SetDestination(currentPatrolTarget);
            agent.isStopped = false;
        }
    }
    
    void AttackPlayer()
    {
        // This is where you would implement the actual attack
        // For example, raycast to check if player is in front, then apply damage
        
        // Example attack implementation:
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                // Apply damage to player
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
    
    void CheckForPlasmaBullets()
    {
        // Find all plasma bullets in the scene
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        
        foreach (GameObject bullet in bullets)
        {
            float distanceToBullet = Vector3.Distance(transform.position, bullet.transform.position);
            
            // If bullet is close and heading towards us
            if (distanceToBullet < 5f)
            {
                Vector3 bulletDirection = bullet.GetComponent<Rigidbody>()?.linearVelocity.normalized ?? Vector3.zero;
                float dotProduct = Vector3.Dot(bulletDirection, (transform.position - bullet.transform.position).normalized);
                
                // If bullet is heading towards us
                if (dotProduct > 0.5f)
                {
                    // Take cover or dodge
                    if (!isTakingCover && currentState != AIState.Stunned)
                    {
                        // 50% chance to take cover, 50% chance to dodge
                        if (Random.value > 0.5f)
                        {
                            currentState = AIState.TakeCover;
                            isTakingCover = true;
                            coverTimer = 0f;
                            
                            // Find cover position (this is a simple implementation)
                            Vector3 coverDirection = -bulletDirection;
                            Vector3 coverPosition = transform.position + coverDirection * 3f;
                            
                            NavMeshHit hit;
                            if (NavMesh.SamplePosition(coverPosition, out hit, 3f, NavMesh.AllAreas))
                            {
                                agent.SetDestination(hit.position);
                            }
                        }
                        else
                        {
                            // Quick dodge to the side
                            Vector3 dodgeDirection = Vector3.Cross(bulletDirection, Vector3.up);
                            if (Random.value > 0.5f) dodgeDirection = -dodgeDirection;
                            
                            Vector3 dodgePosition = transform.position + dodgeDirection * 3f;
                            
                            NavMeshHit hit;
                            if (NavMesh.SamplePosition(dodgePosition, out hit, 3f, NavMesh.AllAreas))
                            {
                                agent.SetDestination(hit.position);
                            }
                        }
                    }
                }
            }
        }
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            // Update animation parameters based on current state
            animator.SetBool("IsPatrolling", currentState == AIState.Patrol);
            animator.SetBool("IsChasing", currentState == AIState.Chase);
            animator.SetBool("IsAttacking", currentState == AIState.Attack);
            animator.SetBool("IsTakingCover", currentState == AIState.TakeCover);
            animator.SetBool("IsStunned", currentState == AIState.Stunned);
            
            // Set movement speed for blend tree
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
    
    // Called when hit by a plasma bullet
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        // If health is low, take cover
        if (health < 50f && currentState != AIState.TakeCover && currentState != AIState.Stunned)
        {
            currentState = AIState.TakeCover;
            isTakingCover = true;
            coverTimer = 0f;
            
            // Find cover position
            Vector3 coverDirection = (transform.position - player.position).normalized;
            Vector3 coverPosition = transform.position + coverDirection * 5f;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(coverPosition, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        
        // If health is depleted, enter stunned state
        if (health <= 0f)
        {
            currentState = AIState.Stunned;
            stunTimer = 0f;
            agent.isStopped = true;
            
            // Reset health after being stunned
            StartCoroutine(ResetHealthAfterStun());
        }
    }
    
    IEnumerator ResetHealthAfterStun()
    {
        yield return new WaitForSeconds(5f);
        health = 100f;
    }
} 