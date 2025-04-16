using UnityEngine;
using System.Collections;

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

    [Header("Audio")]
    [SerializeField] private AudioSource shootSound;

    [Header("Recoil")]
    public float recoilForce = 2.0f;
    public float recoilUpwardForce = 0.5f;

    void Update()
    {
        if (Input.GetMouseButton(1) && Time.time >= lastShotTime + shootCooldown)
        {
            ShootPlasmaBullet();
            lastShotTime = Time.time;
        }
    }

    void ShootPlasmaBullet()
    {
        if (bulletPrefab == null)
        {
            return;
        }
        
        // Play the shooting sound
        if (shootSound != null)
        {
            shootSound.Play();
        }
        
        // Show muzzle flash effect
        if (showMuzzleFlash)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
        
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
        
        // Set bullet color
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.material.color = bulletColor;
        }
        
        // Destroy bullet after lifetime
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator ShowMuzzleFlash()
    {
        // Your existing muzzle flash code here
        yield return null;
    }
} 