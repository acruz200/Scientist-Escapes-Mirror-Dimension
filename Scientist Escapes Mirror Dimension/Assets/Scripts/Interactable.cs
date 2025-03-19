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
        // Check if player is in range
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            // Update player in range status
            playerInRange = distance <= interactionDistance;
            
            // Show/hide interaction prompt
            if (playerInRange && !isDisplayingDialogue)
            {
                uiManager.ShowInteractionPrompt(promptMessage);
                
                // Check for F key press to interact
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Interact();
                }
            }
            else if (!playerInRange && !isDisplayingDialogue)
            {
                uiManager.HideInteractionPrompt();
            }
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