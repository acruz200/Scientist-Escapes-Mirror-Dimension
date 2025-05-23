/*
 * PlasmaBulletShooter
 * 
 * This script handles the plasma gun mechanics, including:
 * - Shooting plasma bullets with right mouse button
 * - Visual cooldown indicator in bottom-right corner
 * - Recoil effects and player movement integration
 * - Visual and audio effects for shooting
 * 
 * Features:
 * - Right-click to fire plasma bullets
 * - Cooldown indicator shows gun status:
 *   - Blue circle: Gun is ready to shoot
 *   - Red circle: Gun is on cooldown
 * - 0.70 second cooldown between shots
 * - Visual muzzle flash effect
 * - Shooting sound effects
 * - Recoil force applied to player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components

public class PlasmaBulletShooter : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 30f;
    public float bulletLifetime = 3f;

    [Header("Shooting Settings")]
    public float shootCooldown = 0.70f;
    private float lastShotTime;
    
    [Header("UI")]
    public Image cooldownIndicator; // Reference to the UI Image for cooldown
    public Sprite readySprite;      // Blue circle sprite
    public Sprite cooldownSprite;   // Red circle sprite
    
    [Header("Effects")]
    public bool showMuzzleFlash = true;
    public float muzzleFlashDuration = 0.05f;
    
    [Header("Audio")]
    public AudioClip shootSound;
    private AudioSource audioSource;
    private const float SHOOT_SOUND_DURATION = 0.60f;
    
    [Header("Recoil")]
    public float recoilForce = 2.0f; // Increased recoil force for more noticeable effect
    public float recoilUpwardForce = 0.5f; // Added upward component to recoil
    
    // Reference to the player
    private GameObject playerObject;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;

    void Start()
    {
        // Add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Find the player object by tag or get the parent if this script is attached to a weapon
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            // If not found by tag, try to get it from the parent hierarchy
            Transform parent = transform.parent;
            while (parent != null)
            {
                if (parent.GetComponent<PlayerMovement>() != null)
                {
                    playerObject = parent.gameObject;
                    break;
                }
                parent = parent.parent;
            }
        }
        
        // Ensure bullet spawn point exists and is properly positioned
        if (bulletSpawnPoint == null)
        {
            // Create a new bullet spawn point as a child of this object
            GameObject spawnPointObj = new GameObject("BulletSpawnPoint");
            spawnPointObj.transform.parent = transform;
          
            spawnPointObj.transform.localPosition = new Vector3(0, 0, 0.5f);
            
            // Set the bullet spawn point reference
            bulletSpawnPoint = spawnPointObj.transform;
        }
        
        // Get player rigidbody for future reference
        if (playerObject != null)
        {
            playerRigidbody = playerObject.GetComponent<Rigidbody>();
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            
            // Ensure the player has the Player tag for bullet detection
            if (!playerObject.CompareTag("Player"))
            {
                playerObject.tag = "Player";
            }
        }
    }

    void Update()
    {
        // Check for left mouse button click with cooldown
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > lastShotTime + shootCooldown)
            {
                ShootPlasmaBullet();
                lastShotTime = Time.time;
            }
        }

        // Update cooldown UI
        if (cooldownIndicator != null)
        {
            // Switch sprites based on cooldown
            if (Time.time > lastShotTime + shootCooldown)
            {
                cooldownIndicator.sprite = readySprite;    // Blue when ready
            }
            else
            {
                cooldownIndicator.sprite = cooldownSprite; // Red when on cooldown
            }
        }
    }

    void ShootPlasmaBullet()
    {
        if (bulletPrefab == null)
        {
            return;
        }
        
        // Show muzzle flash effect
        if (showMuzzleFlash)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
        
        // Play shooting sound when bullet is created
        if (shootSound != null && audioSource != null)
        {
            StartCoroutine(PlayShootSound());
        }
        
        // Create bullet at spawn point - simplified bullet creation with proper scale and visibility
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        
        // Set bullet scale directly to make it more visible
        bullet.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        
        // Add tag to the bullet for collision detection
        bullet.tag = "Bullet";
        
        // Get or add required components
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
        }
        
        // Set bullet properties
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        
        // Add a light component for glow effect if it doesn't exist
        Light bulletLight = bullet.GetComponent<Light>();
        if (bulletLight == null)
        {
            bulletLight = bullet.AddComponent<Light>();
        }
        bulletLight.color = Color.blue;
        bulletLight.intensity = 2f;
        bulletLight.range = 3f;
        
        // Make sure the renderer is visible
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            // Ensure the material is set up with emission
            Material bulletMaterial = bulletRenderer.material;
            bulletMaterial.EnableKeyword("_EMISSION");
            bulletMaterial.SetColor("_EmissionColor", Color.blue * 2.0f);
        }
        
        // Add a rigidbody to move the bullet and make it trigger collisions
        CapsuleCollider capsuleCollider = bullet.GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            capsuleCollider = bullet.AddComponent<CapsuleCollider>();
        }
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = 0.2f;  // Increased from 0.1f
        capsuleCollider.height = 1.0f;  // Increased from 0.5f
        capsuleCollider.direction = 2; // Z-axis
        
        // Add the bullet behavior script
        PlasmaBulletBehavior bulletBehavior = bullet.GetComponent<PlasmaBulletBehavior>();
        if (bulletBehavior == null)
        {
            bulletBehavior = bullet.AddComponent<PlasmaBulletBehavior>();
        }
        bulletBehavior.lifetime = bulletLifetime;
        
        // Apply recoil force to the player
        if (playerRigidbody != null)
        {
            // Apply a force in the opposite direction of shooting with an upward component
            Vector3 recoilDirection = -bulletSpawnPoint.forward + Vector3.up * recoilUpwardForce;
            playerRigidbody.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);
            
            // Add a small random rotation for more realistic recoil
            playerRigidbody.AddTorque(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);
            
            // Notify the player movement script that we've shot
            if (playerMovement != null)
            {
                playerMovement.OnShoot();
            }
        }
    }
    
    IEnumerator ShowMuzzleFlash()
    {
        // Create a temporary light at the muzzle
        GameObject muzzleFlash = new GameObject("MuzzleFlash");
        muzzleFlash.transform.position = bulletSpawnPoint.position;
        muzzleFlash.transform.parent = bulletSpawnPoint;
        
        Light flashLight = muzzleFlash.AddComponent<Light>();
        flashLight.color = Color.blue;
        flashLight.intensity = 3f;
        flashLight.range = 3f;
        
        // Show the flash for a short duration
        yield return new WaitForSeconds(muzzleFlashDuration);
        
        // Remove the flash
        Destroy(muzzleFlash);
    }

    IEnumerator PlayShootSound()
    {
        // Stop any existing sound before starting a new one
        audioSource.Stop();
        audioSource.clip = shootSound;
        // Skip the first 0.35 seconds of the audio
        audioSource.time = 0.35f;
        audioSource.Play();
        yield return new WaitForSeconds(SHOOT_SOUND_DURATION);
        audioSource.Stop();
    }
}