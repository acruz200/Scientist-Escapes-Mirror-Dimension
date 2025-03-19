using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBulletShooter : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 30f;
    public float bulletLifetime = 3f;
    public Color bulletColor = Color.blue;

    [Header("Shooting Settings")]
    public float shootCooldown = 0.2f;
    private float lastShotTime;
    
    [Header("Effects")]
    public bool showMuzzleFlash = true;
    public float muzzleFlashDuration = 0.05f;
    
    // Reference to the player
    private GameObject playerObject;
    private Rigidbody playerRigidbody;

    void Start()
    {
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
        
        // Ensure bullet spawn point exists
        if (bulletSpawnPoint == null)
        {
            bulletSpawnPoint = transform;
        }
        
        // Get player rigidbody for future reference
        if (playerObject != null)
        {
            playerRigidbody = playerObject.GetComponent<Rigidbody>();
            
            // Ensure the player has the Player tag for bullet detection
            if (!playerObject.CompareTag("Player"))
            {
                playerObject.tag = "Player";
                Debug.Log("Set player tag to 'Player' for proper bullet detection");
            }
        }
    }

    void Update()
    {
        // Check for right mouse button click with cooldown
        if (Input.GetMouseButtonDown(1) && Time.time > lastShotTime + shootCooldown)
        {
            ShootPlasmaBullet();
            lastShotTime = Time.time;
        }
    }

    void ShootPlasmaBullet()
    {
        // Show muzzle flash effect
        if (showMuzzleFlash)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
        
        // Create bullet at spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        
        // Set bullet scale to make it thin
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
        
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
        
        // Set bullet color
        Renderer renderer = bullet.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a new material to avoid changing shared materials
            Material bulletMaterial = new Material(Shader.Find("Standard"));
            bulletMaterial.color = bulletColor;
            
            // Add emission for a plasma effect
            bulletMaterial.EnableKeyword("_EMISSION");
            bulletMaterial.SetColor("_EmissionColor", bulletColor * 2f);
            
            renderer.material = bulletMaterial;
        }
        
        // Add a light component for glow effect
        Light bulletLight = bullet.AddComponent<Light>();
        bulletLight.color = bulletColor;
        bulletLight.intensity = 2f;
        bulletLight.range = 2f;
        
        // Add a rigidbody to move the bullet and make it trigger collisions
        CapsuleCollider capsuleCollider = bullet.GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            capsuleCollider = bullet.AddComponent<CapsuleCollider>();
        }
        capsuleCollider.isTrigger = true;
        capsuleCollider.radius = 0.1f;
        capsuleCollider.height = 0.5f;
        capsuleCollider.direction = 2; // Z-axis
        
        // Add the bullet behavior script
        PlasmaBulletBehavior bulletBehavior = bullet.AddComponent<PlasmaBulletBehavior>();
        bulletBehavior.lifetime = bulletLifetime;
        
        // Apply a small recoil force to the player when shooting
        if (playerRigidbody != null)
        {
            playerRigidbody.AddForce(-bulletSpawnPoint.forward * 10f, ForceMode.Impulse);
        }
    }
    
    IEnumerator ShowMuzzleFlash()
    {
        // Create a temporary light at the muzzle
        GameObject muzzleFlash = new GameObject("MuzzleFlash");
        muzzleFlash.transform.position = bulletSpawnPoint.position;
        muzzleFlash.transform.parent = bulletSpawnPoint;
        
        Light flashLight = muzzleFlash.AddComponent<Light>();
        flashLight.color = bulletColor;
        flashLight.intensity = 3f;
        flashLight.range = 3f;
        
        // Show the flash for a short duration
        yield return new WaitForSeconds(muzzleFlashDuration);
        
        // Remove the flash
        Destroy(muzzleFlash);
    }
} 