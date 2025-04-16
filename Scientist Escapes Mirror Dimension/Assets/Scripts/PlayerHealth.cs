using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("UI Settings")]
    [SerializeField] private float healthBarScale = 4.0f; // Controls the health bar size
    
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
        
        // Set health bar scale
        uiManager.SetHealthBarScale(healthBarScale);
        
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
        
        // Disable player controls
        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        
        // Disable any player input scripts
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            if (script != this && script.GetType().Name != "UIManager")
            {
                script.enabled = false;
            }
        }
        
        // Show game over screen
        StartCoroutine(ShowGameOver());
    }
    
    private IEnumerator ShowGameOver()
    {
        // Wait a short delay before showing game over
        yield return new WaitForSeconds(1.5f);
        
        // Find or create GameOverManager
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            GameObject gameOverObj = new GameObject("GameOverManager");
            gameOverManager = gameOverObj.AddComponent<GameOverManager>();
        }
        
        // Show game over screen
        gameOverManager.ShowGameOver();
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