/* PLASMA BULLET PREFAB SETUP GUIDE
 * 
 * This file contains instructions on how to set up the plasma bullet prefab for your game.
 * Follow these steps to create the prefab in Unity:
 * 
 * 1. Creating the Bullet Prefab:
 * ----------------------------
 * a) In the Unity Editor, right-click in the Project window > Create > 3D Object > Capsule
 * b) Rename it to "PlasmaBullet"
 * c) Scale it to make it thin: In the Inspector, set Scale to X:0.1, Y:0.1, Z:0.5
 * d) Adjust the Capsule Collider component: Set Is Trigger to true, Radius to 0.1, Height to 0.5, Direction to Z-axis (2)
 * 
 * 2. Creating a Bullet Material:
 * ----------------------------
 * a) Right-click in the Project window > Create > Material
 * b) Rename it to "PlasmaBulletMaterial"
 * c) Set the Shader to "Standard"
 * d) Set Albedo color to blue (R:0, G:0.3, B:1)
 * e) Enable Emission and set emission color to bright blue (R:0, G:0.5, B:2)
 * f) Apply this material to your PlasmaBullet object
 * 
 * 3. Adding Components:
 * -------------------
 * a) Add a Rigidbody component to the PlasmaBullet:
 *    - Set Use Gravity to false
 *    - Set Collision Detection to Continuous Dynamic
 * 
 * b) Add a Light component:
 *    - Set Color to blue
 *    - Set Intensity to 2
 *    - Set Range to 2
 * 
 * 4. Creating the Prefab:
 * --------------------
 * a) Drag the PlasmaBullet from the Hierarchy into the Project window (preferably into a Prefabs folder)
 * b) This creates a prefab that can be instantiated by the PlasmaBulletShooter script
 * 
 * 5. Setting up the PlasmaBulletShooter:
 * -----------------------------------
 * a) Attach the PlasmaBulletShooter script to your player or weapon object
 * b) Drag the PlasmaBullet prefab into the "Bullet Prefab" slot in the Inspector
 * c) Create an empty child GameObject at the end of your weapon and name it "BulletSpawnPoint"
 * d) Drag this BulletSpawnPoint into the "Bullet Spawn Point" slot in the Inspector
 * 
 * 6. Tagging:
 * --------
 * a) Make sure your player GameObject has the tag "Player"
 * b) For ground detection, either:
 *    - Tag your ground objects with the "Ground" tag
 *    - The script will automatically detect ground-like surfaces based on orientation
 * 
 * That's it! Now you should be able to shoot plasma bullets with right-click, and they will
 * propel the player when they hit the ground.
 */

// This is a guide script only - it doesn't need to be attached to any GameObject
using UnityEngine;

// Instructions only - this script does nothing when added to a GameObject
public class PlasmaBulletSetup : MonoBehaviour
{
    // This script contains only setup instructions
    void Start()
    {
        Debug.Log("This is a setup guide script, not meant to be attached to a GameObject. Please read the instructions inside the script file.");
        
        // Disable this script
        enabled = false;
    }
} 