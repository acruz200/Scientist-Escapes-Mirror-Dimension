using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionTester : MonoBehaviour
{
    [SerializeField] private DoorController doorToTest;
    [SerializeField] private KeyCode testKey = KeyCode.T;
    
    void Start()
    {
        // If no door is assigned, try to find one
        if (doorToTest == null)
        {
            doorToTest = FindObjectOfType<DoorController>();
            if (doorToTest != null)
            {
                Debug.Log("Found door: " + doorToTest.gameObject.name);
            }
            else
            {
                Debug.LogError("No DoorController found in the scene!");
            }
        }
        
        // Check if player is tagged correctly
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found! Make sure your player is tagged correctly.");
        }
        else
        {
            Debug.Log("Player found: " + player.name);
        }
        
        // Check if UIManager exists
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("No UIManager found in the scene!");
        }
        else
        {
            Debug.Log("UIManager found: " + uiManager.gameObject.name);
        }
    }
    
    void Update()
    {
        // Test door interaction with T key
        if (Input.GetKeyDown(testKey) && doorToTest != null)
        {
            Debug.Log("Testing door interaction...");
            doorToTest.OpenDoor();
            
            // Close the door after 2 seconds
            StartCoroutine(CloseDoorAfterDelay(2f));
        }
    }
    
    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (doorToTest != null)
        {
            Debug.Log("Closing door...");
            doorToTest.CloseDoor();
        }
    }
} 