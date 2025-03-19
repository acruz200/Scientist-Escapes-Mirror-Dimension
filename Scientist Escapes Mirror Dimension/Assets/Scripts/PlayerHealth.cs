using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    private UIManager uiManager;
    
    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        
        // Get UI Manager reference
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager == null)
        {
            GameObject uiManagerObj = new GameObject("UIManager");
            uiManager = uiManagerObj.AddComponent<UIManager>();
        }
        
        // Update health bar
        UpdateHealthBar();
    }
    
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        UpdateHealthBar();
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealthBar(currentHealth / maxHealth);
        }
    }
    
    private void Die()
    {
        // Handle player death
        Debug.Log("Player died!");
        
        // You can implement respawn logic or game over screen here
    }
    
    // Debug method to test health system
    void Update()
    {
        // Test taking damage (press T key)
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }
        
        // Test healing (press H key)
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }
    }
} 