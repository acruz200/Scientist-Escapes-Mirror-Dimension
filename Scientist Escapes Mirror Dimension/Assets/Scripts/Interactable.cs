using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string promptMessage = "Press F to interact";
    [SerializeField] private float interactionDistance = 3f;
    
    [Header("Dialogue Settings")]
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float dialogueDisplayTime = 3f;
    
    private bool playerInRange = false;
    private bool isDisplayingDialogue = false;
    private int currentDialogueLine = 0;
    
    private GameObject player;
    private UIManager uiManager;
    
    void Start()
    {
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Get reference to UI Manager (will create this script next)
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager == null)
        {
            // Create a new UIManager if it doesn't exist
            GameObject uiManagerObj = new GameObject("UIManager");
            uiManager = uiManagerObj.AddComponent<UIManager>();
        }
    }
    
    void Update()
    {
        // Ensure both refs are valid before proceeding
        if (player == null || uiManager == null) 
        {
            // Try to find them again if null (optional, could be done in Start)
            if (player == null) player = GameObject.FindGameObjectWithTag("Player");
            if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
            
            // Log error if still null and exit Update
            if (player == null) Debug.LogError($"Interactable '{name}': Player object not found! Ensure player is tagged 'Player'.", this);
            if (uiManager == null) Debug.LogError($"Interactable '{name}': UIManager reference is missing!", this);
            return; // Exit if references are missing
        }

        // Calculate distance and current range state
        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool currentlyInRange = distance <= interactionDistance;

        // --- State Change Logic --- 

        // Player JUST Entered Range?
        if (currentlyInRange && !playerInRange && !isDisplayingDialogue)
        {
            playerInRange = true; // Update state
            Debug.Log($"Interactable '{name}': Player ENTERED range. Showing prompt.", this);
            uiManager.ShowInteractionPrompt(promptMessage);
            Debug.Log($"Interactable '{name}': Called ShowInteractionPrompt.", this); // Log *after* the call
        }
        // Player JUST Exited Range?
        else if (!currentlyInRange && playerInRange && !isDisplayingDialogue)
        {
            playerInRange = false; // Update state
            Debug.Log($"Interactable '{name}': Player EXITED range. Hiding prompt.", this);
            uiManager.HideInteractionPrompt();
            Debug.Log($"Interactable '{name}': Called HideInteractionPrompt.", this); // Log *after* the call
        }

        // --- Interaction Logic --- 

        // Check for F key press only if currently in range and not displaying dialogue
        if (currentlyInRange && !isDisplayingDialogue && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Interactable '{name}': F key pressed. Interacting.", this);
            Interact();
        }
    }
    
    private void Interact()
    {
        // Show dialogue
        if (dialogueLines.Length > 0)
        {
            isDisplayingDialogue = true;
            uiManager.HideInteractionPrompt();
            StartCoroutine(ShowDialogueSequence());
        }
    }
    
    private IEnumerator ShowDialogueSequence()
    {
        // Reset dialogue line index
        currentDialogueLine = 0;
        
        // Show each dialogue line in sequence
        while (currentDialogueLine < dialogueLines.Length)
        {
            uiManager.ShowDialogue(dialogueLines[currentDialogueLine]);
            
            yield return new WaitForSeconds(dialogueDisplayTime);
            
            currentDialogueLine++;
        }
        
        // Hide dialogue when done
        uiManager.HideDialogue();
        isDisplayingDialogue = false;
    }
    
    // Helper method to visualize the interaction range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
} 