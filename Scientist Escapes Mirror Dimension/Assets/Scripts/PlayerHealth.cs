using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float invincibilityTime = 1f;
    
    [Header("UI")]
    public Slider healthBar;
    public Image damageFlashImage;
    public float flashDuration = 0.2f;
    
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    
    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        
        // Update UI
        UpdateHealthUI();
    }
    
    void Update()
    {
        // Handle invincibility timer
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityTime)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        // Don't take damage if invincible
        if (isInvincible) return;
        
        // Apply damage
        currentHealth -= damage;
        
        // Clamp health
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        // Update UI
        UpdateHealthUI();
        
        // Show damage flash
        StartCoroutine(ShowDamageFlash());
        
        // Set invincibility
        isInvincible = true;
        invincibilityTimer = 0f;
        
        // Check if player is dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        // Apply healing
        currentHealth += amount;
        
        // Clamp health
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        // Update UI
        UpdateHealthUI();
    }
    
    void UpdateHealthUI()
    {
        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }
    
    IEnumerator ShowDamageFlash()
    {
        // Show damage flash
        if (damageFlashImage != null)
        {
            damageFlashImage.enabled = true;
            damageFlashImage.color = new Color(1f, 0f, 0f, 0.5f);
            
            // Fade out
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / flashDuration);
                damageFlashImage.color = new Color(1f, 0f, 0f, alpha);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            damageFlashImage.enabled = false;
        }
    }
    
    void Die()
    {
        // Implement death behavior
        Debug.Log("Player died!");
        
        // For now, just respawn at starting position
        // In a real game, you might want to show a game over screen or respawn at a checkpoint
        transform.position = Vector3.zero;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
} 