using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light lampLight;
    [SerializeField] private bool isOn = false;
    [SerializeField] private float intensity = 1.0f;
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private float range = 10f;
    [SerializeField] private Vector3 lightOffset = new Vector3(0, 1.5f, 0); // Default offset to position light higher
    
    [Header("Interaction Settings")]
    [SerializeField] private bool canBeToggled = true;
    [SerializeField] private KeyCode toggleKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 3f;
    
    private GameObject player;
    private UIManager uiManager;
    private bool playerInRange = false;
    private GameObject lightObject;
    
    void Start()
    {
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Get UI Manager reference
        uiManager = FindObjectOfType<UIManager>();
        
        // If no light component is assigned, try to find one
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
            
            // If still no light, create one
            if (lampLight == null)
            {
                CreateLight();
            }
            else
            {
                // Store reference to the light's GameObject
                lightObject = lampLight.gameObject;
            }
        }
        
        // Set initial light state
        UpdateLightState();
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
            if (playerInRange)
            {
                string promptMessage = isOn ? "Press " + toggleKey.ToString() + " to turn off" : "Press " + toggleKey.ToString() + " to turn on";
                uiManager.ShowInteractionPrompt(promptMessage);
                
                // Check for toggle key press
                if (Input.GetKeyDown(toggleKey))
                {
                    ToggleLight();
                }
            }
            else if (!playerInRange)
            {
                uiManager.HideInteractionPrompt();
            }
        }
    }
    
    private void CreateLight()
    {
        // Create a new GameObject for the light
        lightObject = new GameObject("LampLight");
        lightObject.transform.SetParent(transform);
        lightObject.transform.localPosition = lightOffset; // Apply the offset
        
        // Add and configure the light component
        lampLight = lightObject.AddComponent<Light>();
        lampLight.type = LightType.Point;
        lampLight.intensity = intensity;
        lampLight.color = lightColor;
        lampLight.range = range;
    }
    
    public void ToggleLight()
    {
        isOn = !isOn;
        UpdateLightState();
    }
    
    public void TurnOn()
    {
        isOn = true;
        UpdateLightState();
    }
    
    public void TurnOff()
    {
        isOn = false;
        UpdateLightState();
    }
    
    private void UpdateLightState()
    {
        if (lampLight != null)
        {
            lampLight.enabled = isOn;
        }
    }
    
    // Helper method to visualize the interaction range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
        
        // Visualize light position
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + lightOffset, 0.2f);
    }
    
    // Public method to adjust light position at runtime
    public void SetLightPosition(Vector3 newOffset)
    {
        lightOffset = newOffset;
        if (lightObject != null)
        {
            lightObject.transform.localPosition = lightOffset;
        }
    }
} 