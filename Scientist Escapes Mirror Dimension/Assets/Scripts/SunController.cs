using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [Header("Sun Settings")]
    [SerializeField] private float sunIntensity = 1.5f;
    [SerializeField] private Color sunColor = new Color(1f, 0.9f, 0.6f, 1f);
    [SerializeField] private float sunSize = 1f;
    [SerializeField] private bool rotateWithTime = true;
    [SerializeField] private float dayLength = 600f; // 10 minutes per day
    
    [Header("Light Settings")]
    [SerializeField] private bool createLight = true;
    [SerializeField] private float lightIntensity = 1.2f;
    [SerializeField] private Color lightColor = new Color(1f, 0.95f, 0.8f, 1f);
    [SerializeField] private bool castShadows = true;
    
    private Light sunLight;
    private Material sunMaterial;
    private float currentTime = 0f;
    
    void Start()
    {
        // Create or get the sun's visual representation
        SetupSunVisual();
        
        // Create or get the directional light
        if (createLight)
        {
            SetupSunLight();
        }
    }
    
    void Update()
    {
        // Update sun position based on time of day
        if (rotateWithTime)
        {
            UpdateSunPosition();
        }
    }
    
    private void SetupSunVisual()
    {
        // Check if we already have a mesh renderer
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        
        if (renderer == null)
        {
            // Add mesh filter and renderer if they don't exist
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            renderer = gameObject.AddComponent<MeshRenderer>();
            
            // Create a sphere mesh
            meshFilter.mesh = CreateSphereMesh();
        }
        
        // Create a new material with the Unlit/Color shader for better control
        if (sunMaterial == null)
        {
            sunMaterial = new Material(Shader.Find("Unlit/Color"));
        }
        
        // Set material properties
        sunMaterial.color = sunColor;
        
        // Apply material to renderer
        renderer.material = sunMaterial;
        
        // Set the sun's scale
        transform.localScale = new Vector3(sunSize, sunSize, sunSize);
    }
    
    private void SetupSunLight()
    {
        // Check if we already have a light
        sunLight = GetComponent<Light>();
        
        if (sunLight == null)
        {
            // Create a directional light
            sunLight = gameObject.AddComponent<Light>();
            sunLight.type = LightType.Directional;
        }
        
        // Configure the light
        sunLight.intensity = lightIntensity;
        sunLight.color = lightColor;
        sunLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;
        
        // Position the light to match the sun's direction
        UpdateLightDirection();
    }
    
    private void UpdateSunPosition()
    {
        // Update time of day
        currentTime += Time.deltaTime;
        if (currentTime >= dayLength)
        {
            currentTime = 0f;
        }
        
        // Calculate sun position based on time of day (0-1)
        float timeOfDay = currentTime / dayLength;
        
        // Sun moves in an arc from east to west
        float sunAngle = timeOfDay * 180f;
        
        // Apply rotation
        transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);
        
        // Update light direction if we have a light
        if (sunLight != null)
        {
            UpdateLightDirection();
        }
    }
    
    private void UpdateLightDirection()
    {
        // Make the light point in the opposite direction of the sun
        sunLight.transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
    }
    
    private Mesh CreateSphereMesh()
    {
        // Create a simple sphere mesh
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh sphereMesh = tempSphere.GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(tempSphere);
        
        return sphereMesh;
    }
    
    // Public methods for external control
    public void SetTimeOfDay(float timeOfDay)
    {
        // timeOfDay should be between 0 and 1
        timeOfDay = Mathf.Clamp01(timeOfDay);
        currentTime = timeOfDay * dayLength;
    }
    
    public void SetSunIntensity(float intensity)
    {
        sunIntensity = intensity;
        if (sunMaterial != null)
        {
            // For Unlit/Color shader, we just update the color with the new intensity
            sunMaterial.color = new Color(
                sunColor.r * sunIntensity,
                sunColor.g * sunIntensity,
                sunColor.b * sunIntensity,
                sunColor.a
            );
        }
    }
    
    public void SetLightIntensity(float intensity)
    {
        lightIntensity = intensity;
        if (sunLight != null)
        {
            sunLight.intensity = lightIntensity;
        }
    }
} 