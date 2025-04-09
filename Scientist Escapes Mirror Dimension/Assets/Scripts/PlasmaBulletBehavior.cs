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
        // Start with a small scale effect
        transform.localScale = new Vector3(0.05f, 0.05f, 0.25f);
        StartCoroutine(ScaleUpBullet());
        
        // Find the player object - we'll need this to apply force to it
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        // Destroy the bullet after its lifetime expires
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
        
        // Add a subtle pulsing effect for plasma appearance
        float pulseScale = 1f + 0.05f * Mathf.Sin(Time.time * 20f);
        transform.localScale = new Vector3(
            transform.localScale.x * pulseScale,
            transform.localScale.y * pulseScale,
            transform.localScale.z
        );
    }
    
    IEnumerator ScaleUpBullet()
    {
        // Quickly scale the bullet to its intended size for a "charging" effect
        float elapsed = 0f;
        float duration = 0.1f;
        Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.5f);
        Vector3 initialScale = transform.localScale;
        
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Don't process collision with the player or other bullets
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            return;
        }
        
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
        
        // Destroy the bullet on impact
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