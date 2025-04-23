// lets make him move
// using __ imports namespace
// Namespaces are collection of classes, data types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehavior is the base class from which every Unity Script Derives
public class PlayerMovement : MonoBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 100f;  // Lowered jump force significantly

    // Added cooldown for movement after shooting
    public float movementCooldownAfterShoot = 0.1f;
    private float lastShootTime = 0f;

    // Added maximum velocity limit
    public float maxVelocity = 30f;

    // Walking sound
    public AudioSource footstepAudio;

    // Double jump variables
    private bool isGrounded = true;
    private int jumpCount = 0;
    private const int MAX_JUMPS = 2;

    Rigidbody rb;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit something below us (ground)
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
            {
                isGrounded = true;
                jumpCount = 0; // Reset jump count when grounded
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if we're no longer touching the ground
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're in cooldown period after shooting
        bool inCooldown = Time.time < lastShootTime + movementCooldownAfterShoot;

        // Movement
        if (Input.GetKey(KeyCode.W) && !inCooldown)
            rb.linearVelocity += this.transform.forward * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S) && !inCooldown)
            rb.linearVelocity -= this.transform.forward * speed * Time.deltaTime;

        // Rotation
        if (Input.GetKey(KeyCode.D))
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.A))
            t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);

        // Jumping with double jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAX_JUMPS)
        {
            rb.AddForce(t.up * force);
            jumpCount++;
        }

        // Velocity clamp
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }

        // Play walking sound
        bool isMoving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && !inCooldown;

        if (isMoving && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
        else if (!isMoving && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }

    // Public method to be called when shooting
    public void OnShoot()
    {
        lastShootTime = Time.time;
    }
}
