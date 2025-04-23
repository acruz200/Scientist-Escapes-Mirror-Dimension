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
    
    // Component references
    private Animator animator;
    private CharacterController characterController; // If using CharacterController
    private Rigidbody rb; // If using Rigidbody for movement
    
    // Store previous walking state to minimize Animator parameter updates
    private bool wasWalking = false;

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
        
        // Make sure "Walk" animation is set to loop in the import settings
        // We can't do this programmatically, but we can remind in the console
        Debug.Log("Reminder: Ensure your walk animation has 'Loop Time' checked in the import settings!");
    }

    void Update()
    {
        // Only update animation state if we have an animator
        if (animator != null)
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
    
    // Optional: Public method to manually set walking state from other scripts
    public void SetWalking(bool walking)
    {
        if (animator != null && walking != wasWalking)
        {
            animator.SetBool(ANIM_IS_WALKING, walking);
            wasWalking = walking;
        }
    }
    
    // Notes for configuring walking animation:
    // 1. In the FBX import settings for the walk animation, check "Loop Time" to make it loop continuously
    // 2. In the FBX import settings, if the character moves forward during animation:
    //    - Option A: Check "Bake Into Pose" under Root Transform Position (Y/XZ)
    //    - Option B: Keep applyRootMotion = false in this script to ignore forward movement
} 