using UnityEngine;
using UnityEngine.AI; // Include if using NavMeshAgent for movement detection

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossHealth))] // Ensure BossHealth script is present
public class BossAnimationController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent; // Assign if using NavMeshAgent
    [SerializeField] private BossHealth bossHealth;

    // Animator parameter names (ensure these match your Animator Controller)
    private const string IsMovingParam = "isMoving";
    private const string IsDeadParam = "isDead";
    // Add other parameters if needed (e.g., attack triggers)
    // private const string AttackTriggerParam = "Attack";

    private bool hasDied = false; // To prevent updates after death

    void Awake()
    {
        // Get references if not assigned in Inspector
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>(); // Optional, depends on movement implementation
        if (bossHealth == null) bossHealth = GetComponent<BossHealth>();
    }

    void Update()
    {
        // Don't update animations if the boss is dead
        if (hasDied) return;

        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = false;

        // Determine if the boss is moving
        if (agent != null && agent.enabled)
        {
            // Check NavMeshAgent velocity magnitude
            float sqrVelocity = agent.velocity.sqrMagnitude;
            isMoving = sqrVelocity > 0.1f; // Use sqrMagnitude for efficiency
            // Debug.Log($"Boss Agent Velocity SqrMagnitude: {sqrVelocity}, IsMoving set to: {isMoving}"); // Add this line for debugging
        }
        // --- Alternative Movement Detection ---
        // else if (/* Check custom AI state */) 
        // {
        //     // Implement logic based on your BossAI script's state
        // }
        // ------------------------------------

        // Update the Animator parameter
        if (animator != null)
        {
            // Debug.Log($"Setting Animator IsMovingParam to: {isMoving}"); // Optional: Confirm animator update
            animator.SetBool(IsMovingParam, isMoving);
        }
        else
        {
            Debug.LogError("Boss Animator reference is missing!", this);
        }
    }

    // Public method to be called by BossHealth when the boss dies
    public void PlayDeathAnimation()
    {
        if (hasDied || animator == null) return; // Already dead or no animator

        hasDied = true;
        Debug.Log("BossAnimationController: Playing death animation.");

        // Trigger the death animation
        animator.SetBool(IsMovingParam, false); // Stop movement animations
        animator.SetBool(IsDeadParam, true);    // Trigger death state

        // Optionally disable the NavMeshAgent if it hasn't been disabled already
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
    }

    // --- Optional: Add methods for other animations (e.g., attacks) ---
    // public void PlayAttackAnimation()
    // {
    //     if (hasDied || animator == null) return;
    //     animator.SetTrigger(AttackTriggerParam);
    // }
    // ------------------------------------------------------------------
} 