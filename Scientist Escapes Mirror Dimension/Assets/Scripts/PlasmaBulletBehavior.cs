using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBulletBehavior : MonoBehaviour
{
    [Header("Bullet Properties")]
    public float lifetime = 3f;
    public float damage = 10f;
    
    [Header("Player Movement")]
    public float playerPropulsionForce = 2f;
    public float groundDetectionRange = 2f;
    public float maxPlayerVelocity = 20f;
    
    private float timer = 0f;
    private GameObject player;
    
    void Start()
    {
        // Use a larger, fixed scale for better visibility
        transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        
        // Find the player object - we'll need this to apply force to it
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Make sure the renderer is visible
        Renderer bulletRenderer = GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            // Ensure the material is properly set up with emission
            bulletRenderer.material.EnableKeyword("_EMISSION");
            bulletRenderer.material.SetColor("_EmissionColor", Color.blue * 2.0f);
        }
        
        // Ensure the light component is properly configured
        Light bulletLight = GetComponent<Light>();
        if (bulletLight != null)
        {
            bulletLight.color = Color.blue;
            bulletLight.intensity = 2f;
            bulletLight.range = 3f;
        }
    }
    
    void Update()
    {
        // Destroy the bullet after its lifetime expires
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Don't process collision with the player or other bullets
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            return;
        }

        // --- Damage Zombie --- 
        if (other.CompareTag("Zombie"))
        {
            ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                // Use the damage value defined in this bullet script (default is 10f)
                // Let's change it to 20f as requested
                float damageToDeal = 20f; // Or you could use the public damage variable
                zombieHealth.TakeDamage(damageToDeal);

                // Destroy the bullet immediately after damaging the zombie
                Destroy(gameObject);
                return; // Stop further processing for this collision
            }
        }
        // ------------------
        
        // If we hit the ground, propel the player upward
        if (other.CompareTag("Ground") || IsGroundSurface(other))
        {
            PropelPlayer();
        }
        
        // Example of checking for an enemy with a Health component
        /*
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        */
        
        // Destroy the bullet on impact with anything else (other than Player/Bullet/Zombie)
        Destroy(gameObject);
    }
    
    private bool IsGroundSurface(Collider collider)
    {
        // Check if this is likely to be the ground by determining if it's below the player
        // and has a large flat surface
        if (player == null)
            return false;
            
        // Simple heuristic: if the collision is below the player and not a wall
        bool isBelow = collider.transform.position.y < player.transform.position.y;
        
        // Use a raycast to check if the normal is pointing upward (like a floor would)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDetectionRange))
        {
            float dotProduct = Vector3.Dot(hit.normal, Vector3.up);
            return isBelow && dotProduct > 0.7f; // 0.7 is roughly a 45-degree angle
        }
        
        return false;
    }
    
    private void PropelPlayer()
    {
        if (player != null)
        {
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Calculate force direction (opposite of bullet direction)
                Vector3 forceDirection = -transform.forward;
                
                // Ensure the force has a significant upward component
                if (forceDirection.y < 0.3f)
                {
                    forceDirection.y = 0.3f;
                    forceDirection = forceDirection.normalized;
                }
                
                // Apply the force to the player
                playerRb.AddForce(forceDirection * playerPropulsionForce, ForceMode.Impulse);
                
                // Limit the player's velocity to prevent flying too far
                if (playerRb.linearVelocity.magnitude > maxPlayerVelocity)
                {
                    playerRb.linearVelocity = playerRb.linearVelocity.normalized * maxPlayerVelocity;
                }
                
                // Add some visual feedback
                CreatePropulsionEffect(player.transform.position);
            }
        }
    }
    
    private void CreatePropulsionEffect(Vector3 position)
    {
        // In a real implementation, you might instantiate a particle effect here
        // For now, we'll just add a temporary light flash
        GameObject flashEffect = new GameObject("PropulsionFlash");
        flashEffect.transform.position = position;
        
        Light flashLight = flashEffect.AddComponent<Light>();
        flashLight.color = Color.blue;
        flashLight.intensity = 3f;
        flashLight.range = 5f;
        
        // Destroy the flash effect after a short time
        Destroy(flashEffect, 0.2f);
    }
} 