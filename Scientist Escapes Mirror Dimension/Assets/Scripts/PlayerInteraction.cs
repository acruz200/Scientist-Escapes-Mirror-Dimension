using UnityEngine;
using UnityEngine.UI; // Required for interacting with UI elements like Text

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f; // How far the player can interact from
    [SerializeField] private KeyCode interactionKey = KeyCode.F; // Key to trigger interaction
    [SerializeField] private LayerMask interactionLayer; // Which layers should be checked for interactables
    [SerializeField] private Transform playerCameraTransform; // Reference to the player's camera

    [Header("UI")]
    [SerializeField] private Text interactionPromptText; // UI Text element to display the prompt

    private IInteractable currentInteractable; // The interactable object currently in range and looked at

    void Start()
    {
        if (playerCameraTransform == null)
        {
            // Try to find the main camera if not assigned
            playerCameraTransform = Camera.main?.transform;
            if (playerCameraTransform == null)
            {
                 Debug.LogError("Player Camera Transform is not assigned and Main Camera could not be found!", this);
            }
        }

        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false); // Hide prompt initially
        }
        else
        {
             Debug.LogWarning("Interaction Prompt Text UI element is not assigned in the Inspector.", this);
        }
    }

    void Update()
    {
        CheckForInteractable();
        HandleInteractionInput();
    }

    /// <summary>
    /// Casts a ray forward to detect interactable objects.
    /// </summary>
    private void CheckForInteractable()
    {
        RaycastHit hit;
        bool hitInteractable = false;

        if (playerCameraTransform != null && Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, interactionDistance, interactionLayer))
        {
            // Check if the hit object has an IInteractable component
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Found an interactable object
                currentInteractable = interactable;
                hitInteractable = true;
                ShowInteractionPrompt(currentInteractable.GetInteractionPrompt());
            }
        }

        // If no interactable was hit or the raycast didn't hit anything relevant
        if (!hitInteractable)
        {
            ClearInteractable();
        }
    }

    /// <summary>
    /// Checks if the interaction key is pressed while an interactable is targeted.
    /// </summary>
    private void HandleInteractionInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.Interact();
            // Optional: Clear interactable immediately after interaction if desired
            // ClearInteractable();
        }
    }

    /// <summary>
    /// Displays the interaction prompt UI.
    /// </summary>
    /// <param name="prompt">The text to display.</param>
    private void ShowInteractionPrompt(string prompt)
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.text = prompt;
            interactionPromptText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the interaction prompt UI and clears the current interactable reference.
    /// </summary>
    private void ClearInteractable()
    {
        currentInteractable = null;
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }

    // Optional: Draw a gizmo in the editor to visualize the interaction range
    private void OnDrawGizmosSelected()
    {
        if (playerCameraTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(playerCameraTransform.position, playerCameraTransform.position + playerCameraTransform.forward * interactionDistance);
        }
    }
} 