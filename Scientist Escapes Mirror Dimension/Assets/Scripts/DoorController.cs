using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private bool openForward = true;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    
    [Header("Interaction Settings")]
    [SerializeField] private bool canBeToggled = true;
    [SerializeField] private KeyCode toggleKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 5f; // Increased default distance
    [SerializeField] private string promptMessage = "Press E to open/close";
    
    private bool isOpen = false;
    private bool isAnimating = false;
    private float currentAngle = 0f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    
    private GameObject player;
    private UIManager uiManager;
    private bool playerInRange = false;
    
    void Start()
    {
        // Store initial rotation
        initialRotation = transform.localRotation;
        
        // Calculate target rotation
        float direction = openForward ? 1f : -1f;
        targetRotation = Quaternion.Euler(rotationAxis * openAngle * direction);
        
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found! Make sure it's tagged as 'Player'");
        }
        
        // Get UI Manager reference
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager not found! Creating one...");
            
            // Create a new UIManager if it doesn't exist
            GameObject uiManagerObj = new GameObject("UIManager");
            uiManager = uiManagerObj.AddComponent<UIManager>();
        }
    }
    
    void Update()
    {
        // Check if player is in range
        if (player != null && canBeToggled)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            // Update player in range status
            playerInRange = distance <= interactionDistance;
            
            // Show/hide interaction prompt
            if (playerInRange && !isAnimating)
            {
                string message = isOpen ? "Press " + toggleKey.ToString() + " to close" : "Press " + toggleKey.ToString() + " to open";
                
                if (uiManager != null)
                {
                    uiManager.ShowInteractionPrompt(message);
                }
                
                // Check for toggle key press
                if (Input.GetKeyDown(toggleKey))
                {
                    ToggleDoor();
                }
            }
            else if (!playerInRange && !isAnimating)
            {
                if (uiManager != null)
                {
                    uiManager.HideInteractionPrompt();
                }
            }
        }
        else if (player == null)
        {
            // Try to find player again if it's null
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Handle door animation
        if (isAnimating)
        {
            AnimateDoor();
        }
    }
    
    private void ToggleDoor()
    {
        isOpen = !isOpen;
        isAnimating = true;
    }
    
    private void AnimateDoor()
    {
        // Calculate target angle based on door state
        float targetAngle = isOpen ? openAngle : 0f;
        float direction = openForward ? 1f : -1f;
        
        // Smoothly rotate the door
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle * direction, openSpeed * Time.deltaTime);
        
        // Apply rotation
        transform.localRotation = initialRotation * Quaternion.Euler(rotationAxis * currentAngle);
        
        // Check if animation is complete
        if (Mathf.Approximately(currentAngle, targetAngle * direction))
        {
            isAnimating = false;
        }
    }
    
    // Helper method to visualize the interaction range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
        
        // Visualize door swing direction
        Gizmos.color = Color.blue;
        Vector3 direction = openForward ? transform.right : -transform.right;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
    
    // Public methods for external control
    public void OpenDoor()
    {
        if (!isOpen && !isAnimating)
        {
            isOpen = true;
            isAnimating = true;
        }
    }
    
    public void CloseDoor()
    {
        if (isOpen && !isAnimating)
        {
            isOpen = false;
            isAnimating = true;
        }
    }
    
    public void SetOpenAngle(float angle)
    {
        openAngle = angle;
        
        // Recalculate target rotation
        float direction = openForward ? 1f : -1f;
        targetRotation = Quaternion.Euler(rotationAxis * openAngle * direction);
    }
} 