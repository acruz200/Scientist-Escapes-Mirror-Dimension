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
        Debug.Log("[PlayerHealth] Die() method called.", this);
        
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
        Debug.Log("[PlayerHealth] Entering script disabling loop.", this);
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            string scriptName = script.GetType().Name;
            Debug.Log($"[PlayerHealth] Checking script: {scriptName}", this);
            if (script != this && scriptName != "UIManager" && scriptName != "PlayerAnimationController") // Also skip PlayerAnimationController
            {
                Debug.Log($"[PlayerHealth] Disabling script: {scriptName}", this);
                script.enabled = false;
                Debug.Log($"[PlayerHealth] Successfully disabled script: {scriptName}", this);
            }
            else
            {
                Debug.Log($"[PlayerHealth] Skipping script: {scriptName}", this);
            }
        }
        Debug.Log("[PlayerHealth] Exited script disabling loop.", this);

        // Add extra log here
        Debug.Log("[PlayerHealth] Reached point just before starting coroutine.", this);

        // Show game over screen
        Debug.Log("[PlayerHealth] Starting ShowGameOver coroutine.", this);
        StartCoroutine(ShowGameOver());
    }
    
    private IEnumerator ShowGameOver()
    {
        Debug.Log("[PlayerHealth] ShowGameOver coroutine started. Waiting for delay... Time.timeScale = " + Time.timeScale, this);
        // Wait a short delay before showing game over
        yield return new WaitForSeconds(1.5f);

        Debug.Log("[PlayerHealth] ShowGameOver coroutine finished delay. Finding GameOverManager...", this);
        // Ensure time is running for the Game Over UI animations/interactions
        Time.timeScale = 1f;

        // Find or create GameOverManager
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogWarning("[PlayerHealth] GameOverManager not found via FindObjectOfType. Attempting to create.", this);
            GameObject gameOverObj = new GameObject("GameOverManager");
            gameOverManager = gameOverObj.AddComponent<GameOverManager>();
        }
        
        // Show game over screen
        if (gameOverManager != null)
        {
            Debug.Log("[PlayerHealth] Calling gameOverManager.ShowGameOver()", this);
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("[PlayerHealth] Failed to find or create GameOverManager. Cannot show game over screen.", this);
        }
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