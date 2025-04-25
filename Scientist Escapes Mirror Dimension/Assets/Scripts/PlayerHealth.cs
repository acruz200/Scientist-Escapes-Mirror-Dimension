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
    private bool isDead = false; // Flag to prevent multiple deaths
    
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
        // If player is already dead, don't process further damage
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        UpdateHealthBar();
        
        // Show damage flash
        if (uiManager != null)
        {
            uiManager.ShowDamageFlash();
        }
        
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
        // Prevent Die() from being called multiple times
        if (isDead) return;
        isDead = true; // Set the flag

        // Handle player death
        Debug.Log("Player died!");
        
        // Play death animation
        PlayerAnimationController animController = GetComponent<PlayerAnimationController>();
        if (animController != null)
        {
            animController.PlayDeathAnimation();
        }
        
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

        // Ensure time is running for the Game Over UI animations/interactions
        Time.timeScale = 1f;

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
        // Test healing (press H key)
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }
    }
} 