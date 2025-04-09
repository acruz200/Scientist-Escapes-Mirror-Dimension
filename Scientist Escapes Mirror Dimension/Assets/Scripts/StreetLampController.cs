using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLampController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float lightIntensity = 3.0f;
    [SerializeField] private Color lightColor = new Color(1f, 0.85f, 0.4f, 1f);
    [SerializeField] private float lightRange = 20f;
    [SerializeField] private bool castShadows = true;
    [SerializeField] private bool flicker = false;
    [SerializeField] private float flickerIntensity = 0.2f;
    [SerializeField] private float flickerSpeed = 0.1f;
    [SerializeField] private float lightHeightOffset = 1.0f; // Height offset for the light
    
    [Header("Visual Settings")]
    [SerializeField] private bool enableGlow = true;
    [SerializeField] private Color glowColor = new Color(1f, 0.8f, 0.3f, 1f);
    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private string lightPartName = "Light"; // Name of the light part of the lamp
    
    private Light lampLight;
    private Material lampMaterial;
    private MeshRenderer lampRenderer;
    private float baseIntensity;
    private float flickerTimer = 0f;
    private GameObject lightObject; // Reference to the light GameObject
    
    void Start()
    {
        // Find all objects tagged with "StreetLamp"
        GameObject[] streetLamps = GameObject.FindGameObjectsWithTag("StreetLamp");
        
        // Set up each street lamp
        foreach (GameObject lamp in streetLamps)
        {
            SetupLamp(lamp);
        }
    }
    
    void Update()
    {
        // Find all objects tagged with "StreetLamp" (in case new ones are added)
        GameObject[] streetLamps = GameObject.FindGameObjectsWithTag("StreetLamp");
        
        // Update each street lamp
        foreach (GameObject lamp in streetLamps)
        {
            StreetLampController controller = lamp.GetComponent<StreetLampController>();
            if (controller != null)
            {
                controller.UpdateLamp();
            }
        }
    }
    
    private void SetupLamp(GameObject lamp)
    {
        // Add this controller to the lamp if it doesn't have one
        if (lamp.GetComponent<StreetLampController>() == null)
        {
            lamp.AddComponent<StreetLampController>();
            return;
        }
        
        // Create a child GameObject for the light
        lightObject = new GameObject("LampLight");
        lightObject.transform.SetParent(lamp.transform);
        
        // Position the light higher on the lamp
        lightObject.transform.localPosition = new Vector3(0, lightHeightOffset, 0);
        
        // Get or add a light component to the light object
        lampLight = lightObject.GetComponent<Light>();
        if (lampLight == null)
        {
            lampLight = lightObject.AddComponent<Light>();
        }
        
        // Configure the light
        lampLight.type = LightType.Point;
        lampLight.intensity = lightIntensity;
        lampLight.color = lightColor;
        lampLight.range = lightRange;
        lampLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;
        
        // Add a subtle inner radius for more realistic falloff
        lampLight.innerSpotAngle = 30f;
        lampLight.spotAngle = 90f;
        
        // Store the base intensity for flickering
        baseIntensity = lightIntensity;
        
        // Set up the visual glow if enabled
        if (enableGlow)
        {
            SetupGlow(lamp);
        }
    }
    
    private void SetupGlow(GameObject lamp)
    {
        // Try to find the light part of the lamp
        Transform lightPart = lamp.transform.Find(lightPartName);
        
        if (lightPart != null)
        {
            // Get the renderer of the light part
            lampRenderer = lightPart.GetComponent<MeshRenderer>();
            
            if (lampRenderer != null)
            {
                // Create a new material with the Unlit/Color shader for better control
                lampMaterial = new Material(Shader.Find("Unlit/Color"));
                
                // Set the color with intensity
                lampMaterial.color = glowColor * glowIntensity;
                
                // Apply the material
                lampRenderer.material = lampMaterial;
            }
        }
        else
        {
            // If we can't find a specific light part, try to find a child with "light" in its name
            foreach (Transform child in lamp.transform)
            {
                if (child.name.ToLower().Contains("light"))
                {
                    lampRenderer = child.GetComponent<MeshRenderer>();
                    
                    if (lampRenderer != null)
                    {
                        // Create a new material with the Unlit/Color shader for better control
                        lampMaterial = new Material(Shader.Find("Unlit/Color"));
                        
                        // Set the color with intensity
                        lampMaterial.color = glowColor * glowIntensity;
                        
                        // Apply the material
                        lampRenderer.material = lampMaterial;
                        break;
                    }
                }
            }
            
            // If we still couldn't find a light part, log a warning
            if (lampRenderer == null)
            {
                Debug.LogWarning("Could not find a light part for the street lamp: " + lamp.name);
            }
        }
    }
    
    private void UpdateLamp()
    {
        // Handle flickering if enabled
        if (flicker && lampLight != null)
        {
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= flickerSpeed)
            {
                flickerTimer = 0f;
                
                // Random flicker
                float randomFlicker = Random.Range(-flickerIntensity, flickerIntensity);
                lampLight.intensity = baseIntensity + randomFlicker;
                
                // Also flicker the glow if enabled
                if (enableGlow && lampMaterial != null)
                {
                    float glowFlicker = randomFlicker * 0.5f; // Less intense flicker for the glow
                    lampMaterial.color = glowColor * (glowIntensity + glowFlicker);
                }
            }
        }
    }
    
    // Public methods for external control
    public void TurnOn()
    {
        if (lampLight != null)
        {
            lampLight.enabled = true;
        }
        
        if (lampMaterial != null && enableGlow)
        {
            // For Unlit/Color shader, we just need to set the color
            lampMaterial.color = glowColor * glowIntensity;
        }
    }
    
    public void TurnOff()
    {
        if (lampLight != null)
        {
            lampLight.enabled = false;
        }
        
        if (lampMaterial != null && enableGlow)
        {
            // For Unlit/Color shader, we just set the color to black
            lampMaterial.color = Color.black;
        }
    }
    
    public void SetIntensity(float intensity)
    {
        lightIntensity = intensity;
        baseIntensity = intensity;
        
        if (lampLight != null)
        {
            lampLight.intensity = lightIntensity;
        }
    }
    
    public void SetGlowIntensity(float intensity)
    {
        glowIntensity = intensity;
        
        if (lampMaterial != null && enableGlow)
        {
            // For Unlit/Color shader, we just update the color
            lampMaterial.color = glowColor * glowIntensity;
        }
    }
    
    public void SetLightHeight(float height)
    {
        lightHeightOffset = height;
        
        if (lightObject != null)
        {
            lightObject.transform.localPosition = new Vector3(0, lightHeightOffset, 0);
        }
    }
} 