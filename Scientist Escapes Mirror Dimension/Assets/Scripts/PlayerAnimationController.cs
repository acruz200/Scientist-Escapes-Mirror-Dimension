using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Minimum velocity magnitude required to be considered 'moving'")]
    [SerializeField] private float movementThreshold = 0.1f;
    
    [Tooltip("Whether to apply root motion from animations")]
    [SerializeField] private bool applyRootMotion = false;
    
    // Animation parameter names
    private const string ANIM_IS_WALKING = "isWalking";
    private const string ANIM_IS_DEAD = "isDead";
    
    // Component references
    private Animator animator;
    private CharacterController characterController; // If using CharacterController
    private Rigidbody rb; // If using Rigidbody for movement
    
    // Store animation states to minimize Animator parameter updates
    private bool wasWalking = false;
    private bool isDead = false;

    void Start()
    {
        // Get required components
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
            return;
        }
        
        // Disable root motion to prevent animation from moving the character
        animator.applyRootMotion = applyRootMotion;
        
        // Try to get the movement component (use either CharacterController or Rigidbody)
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            // If no CharacterController, try Rigidbody
            rb = GetComponent<Rigidbody>();
        }
        
        // Make sure animations are set up properly in the import settings
        Debug.Log("Reminder: Ensure your animations have proper import settings (Loop Time for walk, etc.)");
    }

    void Update()
    {
        // Only update animation state if we have an animator and player is not dead
        if (animator != null && !isDead)
        {
            UpdateAnimationState();
        }
    }
    
    void UpdateAnimationState()
    {
        bool isCurrentlyWalking = false;
        
        // Method 1: Detect movement using CharacterController (if available)
        if (characterController != null)
        {
            isCurrentlyWalking = characterController.velocity.magnitude > movementThreshold;
        }
        // Method 2: Detect movement using Rigidbody (if available)
        else if (rb != null)
        {
            isCurrentlyWalking = rb.linearVelocity.magnitude > movementThreshold;
        }
        // Method 3: Detect movement using Input (fallback if no physics components found)
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            isCurrentlyWalking = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        }
        
        // Force immediate update to animation state to avoid sticking
        animator.SetBool(ANIM_IS_WALKING, isCurrentlyWalking);
        wasWalking = isCurrentlyWalking;
    }
    
    // Add a method to reset animation state - call this from other scripts when needed
    public void ResetAnimationState()
    {
        if (animator != null)
        {
            animator.SetBool(ANIM_IS_WALKING, false);
            wasWalking = false;
        }
    }
    
    /// <summary>
    /// Call this method when player health reaches zero to trigger death animation
    /// </summary>
    public void PlayDeathAnimation()
    {
        if (animator != null && !isDead)
        {
            // Stop all other animations by disabling walking
            animator.SetBool(ANIM_IS_WALKING, false);
            wasWalking = false;
            
            // Trigger death animation
            animator.SetBool(ANIM_IS_DEAD, true);
            isDead = true;
            
            // Optional: You may want to disable player controls/physics here
            // or call a method on the player controller to handle that
            DisablePlayerMovement();
        }
    }
    
    /// <summary>
    /// Resets death state - use this for respawning or restarting level
    /// </summary>
    public void ResetDeathState()
    {
        if (animator != null)
        {
            animator.SetBool(ANIM_IS_DEAD, false);
            isDead = false;
            
            // Optional: Re-enable player controls/physics
            EnablePlayerMovement();
        }
    }
    
    // Helper methods to handle player movement components
    private void DisablePlayerMovement()
    {
        // Disable character controller if present
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        // If using rigidbody, you might want to freeze it
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        // You may need additional code here to disable your specific player controller script
        // Example: GetComponent<PlayerController>().enabled = false;
    }
    
    private void EnablePlayerMovement()
    {
        // Re-enable character controller if present
        if (characterController != null)
        {
            characterController.enabled = true;
        }
        
        // If using rigidbody, unfreeze it
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        // You may need additional code here to re-enable your specific player controller script
        // Example: GetComponent<PlayerController>().enabled = true;
    }
    
    // Notes for configuring walking animation:
    // 1. In the FBX import settings for the walk animation, check "Loop Time" to make it loop continuously
    // 2. In the FBX import settings, if the character moves forward during animation:
    //    - Option A: Check "Bake Into Pose" under Root Transform Position (Y/XZ)
    //    - Option B: Keep applyRootMotion = false in this script to ignore forward movement
    //
    // Notes for configuring death animation:
    // 1. Add a new boolean parameter "isDead" in your Animator Controller
    // 2. Create a "Death" state with your death animation
    // 3. Create transitions from any state to the Death state with condition "isDead = true"
    // 4. For the transitions, uncheck "Has Exit Time" to make it trigger immediately
} 