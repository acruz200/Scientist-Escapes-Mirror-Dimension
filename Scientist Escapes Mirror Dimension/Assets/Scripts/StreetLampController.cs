using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLampController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float lightIntensity = 1.5f;
    [SerializeField] private Color lightColor = new Color(1f, 0.95f, 0.8f, 1f);
    [SerializeField] private float lightRange = 15f;
    [SerializeField] private bool castShadows = true;
    [SerializeField] private bool flicker = false;
    [SerializeField] private float flickerIntensity = 0.2f;
    [SerializeField] private float flickerSpeed = 0.1f;
    
    [Header("Visual Settings")]
    [SerializeField] private bool enableGlow = true;
    [SerializeField] private Color glowColor = new Color(1f, 0.9f, 0.6f, 1f);
    [SerializeField] private float glowIntensity = 1.0f;
    
    private Light lampLight;
    private Material lampMaterial;
    private MeshRenderer lampRenderer;
    private float baseIntensity;
    private float flickerTimer = 0f;
    
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
        
        // Get or add a light component
        lampLight = lamp.GetComponent<Light>();
        if (lampLight == null)
        {
            lampLight = lamp.AddComponent<Light>();
        }
        
        // Configure the light
        lampLight.type = LightType.Point;
        lampLight.intensity = lightIntensity;
        lampLight.color = lightColor;
        lampLight.range = lightRange;
        lampLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;
        
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
        // Get the renderer
        lampRenderer = lamp.GetComponent<MeshRenderer>();
        
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
} 