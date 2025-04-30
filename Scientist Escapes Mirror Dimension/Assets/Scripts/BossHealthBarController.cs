using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBarController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the Boss GameObject here.")]
    [SerializeField] private BossHealth bossHealth; // Assign the Boss object or find dynamically
    [Tooltip("Assign the UI Slider component for the health bar.")]
    [SerializeField] private Slider healthSlider;
    [Tooltip("Assign the Canvas Group containing the health bar UI elements.")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [Tooltip("Distance from the boss at which the health bar becomes visible.")]
    [SerializeField] private float activationRadius = 25f;
    [Tooltip("How quickly the health bar fades in/out (0 for instant).")]
    [SerializeField] private float fadeDuration = 0.5f;

    // Internal state
    private Transform playerTransform;
    private bool isVisible = false;
    private bool bossIsAlive = true;

    void Start()
    {
        // Find Player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("BossHealthBarController: Player not found! Make sure the player has the 'Player' tag.", this);
            enabled = false; // Disable script if player not found
            return;
        }

        // Validate References
        if (bossHealth == null)
        {
            Debug.LogError("BossHealthBarController: BossHealth reference not assigned.", this);
            enabled = false;
            return;
        }
        if (healthSlider == null)
        {
            // Try to get it from this GameObject or children if not assigned
            healthSlider = GetComponentInChildren<Slider>();
            if (healthSlider == null)
            {
                Debug.LogError("BossHealthBarController: Health Slider reference not assigned and not found in children.", this);
                enabled = false;
                return;
            }
        }
         if (canvasGroup == null)
        {
            // Try to get it from this GameObject if not assigned
             canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            { 
                Debug.LogError("BossHealthBarController: Canvas Group reference not assigned and not found on this GameObject.", this);
                 enabled = false;
                return;
             }
        }

        // Initial setup
        healthSlider.minValue = 0f;
        healthSlider.maxValue = 1f; // We'll work with percentages
        healthSlider.value = 1f;
        canvasGroup.alpha = 0f; // Start hidden
        isVisible = false;
        bossIsAlive = !bossHealth.IsDead();
    }

    void Update()
    {
        // If boss is dead and bar is still showing, hide it permanently
        if (!bossIsAlive)
        {
            if (isVisible)
            {
                SetVisibility(false);
            }
            return; // No more updates needed
        }

        // Check if the boss died this frame
        if (bossHealth.IsDead())
        {
            bossIsAlive = false;
            SetVisibility(false);
            return;
        }

        // Check distance to player
        float distance = Vector3.Distance(playerTransform.position, bossHealth.transform.position);
        bool shouldBeVisible = distance <= activationRadius;

        // Update visibility if it changed
        if (shouldBeVisible != isVisible)
        {
            SetVisibility(shouldBeVisible);
        }

        // Update the slider value if visible
        if (isVisible)
        {
            float maxHealth = bossHealth.GetMaxHealth();
            if (maxHealth > 0) // Prevent division by zero
            {
                float healthPercent = bossHealth.GetCurrentHealth() / maxHealth;
                // Optionally smooth the transition
                healthSlider.value = Mathf.Lerp(healthSlider.value, healthPercent, Time.deltaTime * 10f);
            }
        }
    }

    void SetVisibility(bool visible)
    {
        isVisible = visible;
        // Use lerping for smooth fade, or set directly if fadeDuration is 0
        StopAllCoroutines(); // Stop previous fade if any
        StartCoroutine(FadeCanvasGroup(canvasGroup, visible ? 1f : 0f, fadeDuration));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        cg.alpha = targetAlpha; // Ensure target alpha is reached
    }
} 