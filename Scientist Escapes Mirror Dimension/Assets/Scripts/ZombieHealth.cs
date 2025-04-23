using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
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
        // Ensure the GameObject has the "Zombie" tag
        if (gameObject.tag != "Zombie")
        {
            Debug.LogWarning($"GameObject '{name}' should have the 'Zombie' tag for health system to work correctly.", this);
            // Optionally force the tag: gameObject.tag = "Zombie";
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
        Debug.Log($"{name} took {amount} damage, health remaining: {currentHealth}");

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

        Debug.Log($"{name} has died.");

        // --- Death Feedback & Cleanup --- 
        // Play death sound
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        // Trigger death animation if exists
        // if (animator != null) animator.SetTrigger("Die");

        // Disable components that might interfere (like movement scripts, colliders)
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // Example: Disable EnemyPatrol script if it exists
        EnemyPatrol patrolScript = GetComponent<EnemyPatrol>();
        if (patrolScript != null) patrolScript.enabled = false;
        
        // Disable NavMeshAgent if exists
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if(agent != null) agent.enabled = false;

        // Destroy the GameObject after a delay (e.g., 2 seconds to let sound/animation play)
        Destroy(gameObject, 2.0f);
        // -----------------------------
    }
} 