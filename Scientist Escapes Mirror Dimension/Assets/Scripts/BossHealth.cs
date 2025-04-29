using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 500f; // Boss health set to 500
    [SerializeField] private float currentHealth;

    [Header("Effects")]
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    // Add reference to Animator if you have animations
    // [SerializeField] private Animator animator;

    private bool isDead = false;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        // You might want a specific tag for the boss, e.g., "Boss"
        if (gameObject.tag != "Boss") 
        {
            Debug.LogWarning($"GameObject '{name}' should have the 'Boss' tag for the health system.", this);
            // Optionally force the tag: gameObject.tag = "Boss";
        }

        // Get or add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // 3D sound
            audioSource.playOnAwake = false;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // Don't take damage if already dead

        currentHealth -= amount;
        Debug.Log($"{name} (Boss) took {amount} damage, health remaining: {currentHealth}");

        // --- Feedback --- 
        // Play damage particles
        if (damageParticles != null)
        {
            damageParticles.Play();
        }
        // Play damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
        // Trigger damage animation if exists
        // if (animator != null) animator.SetTrigger("TakeDamage");
        // ---------------

        if (currentHealth <= 0f)
        { 
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{name} (Boss) has been defeated!");

        // --- Boss Death Specific Logic --- 
        // Maybe trigger a victory sequence, different score, etc.
        // Example: Add a large score bonus for defeating the boss
        if (ScoreManager.instance != null)
        {
            // You could create a specific function in ScoreManager or just add points directly
            ScoreManager.instance.AddPoints(1000); // e.g., 1000 points for boss kill
        }

        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        // Trigger death animation if exists
        // if (animator != null) animator.SetTrigger("Die");

        // Disable components
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // Disable any movement/AI scripts specific to the boss
        // Example: BossAI bossAIScript = GetComponent<BossAI>();
        // if (bossAIScript != null) bossAIScript.enabled = false;
        
        // Disable NavMeshAgent if exists
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if(agent != null) agent.enabled = false;

        // Destroy the boss object after a delay, or trigger a cutscene
        Destroy(gameObject, 5.0f); // Longer delay for boss death? 
        // -----------------------------
    }
} 