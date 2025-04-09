/* PLASMA BULLET SETUP GUIDE
 * 
 * Follow these steps in Unity to create the bullet prefab:
 * 
 * 1. Creating the Basic Bullet:
 * --------------------------
 * - Hierarchy window > Right-click > 3D Object > Capsule
 * - Rename to "PlasmaBullet"
 * - In Inspector:
 *   - Transform:
 *     Position: X=0, Y=0, Z=0
 *     Rotation: X=0, Y=0, Z=0
 *     Scale: X=0.1, Y=0.1, Z=0.5
 * 
 * 2. Creating the Bullet Material:
 * ---------------------------
 * - Project window > Right-click > Create > Material
 * - Name it "PlasmaBulletMaterial"
 * - In Inspector:
 *   - Shader: Standard
 *   - Albedo Color: Blue (R:0, G:0.5, B:1)
 *   - Smoothness: 0.8
 *   - Enable Emission
 *   - Emission Color: Bright Blue (R:0, G:1, B:2)
 *   - Emission Intensity: 2
 * 
 * 3. Setting up Components:
 * ---------------------
 * a) Rigidbody:
 *    - Add Component > Rigidbody
 *    - Settings:
 *      - Mass: 1
 *      - Drag: 0
 *      - Use Gravity: OFF
 *      - Is Kinematic: OFF
 *      - Interpolate: Interpolate
 *      - Collision Detection: Continuous Dynamic
 *      - Constraints: Freeze Rotation (all axes)
 * 
 * b) Capsule Collider:
 *    - In existing Capsule Collider component:
 *      - Is Trigger: ON
 *      - Center: X=0, Y=0, Z=0
 *      - Radius: 0.1
 *      - Height: 0.5
 *      - Direction: Z-Axis (2)
 * 
 * c) Light:
 *    - Add Component > Light
 *    - Settings:
 *      - Type: Point
 *      - Color: Same blue as material
 *      - Intensity: 2
 *      - Range: 2
 *      - Indirect Multiplier: 1
 * 
 * 4. Creating the Prefab:
 * -------------------
 * - Project window > Create > Folder > Name it "Prefabs"
 * - Drag PlasmaBullet from Hierarchy into Prefabs folder
 * - Delete the original from the scene
 * 
 * 5. Final Setup:
 * -----------
 * - Select SciFiHandGun in Hierarchy
 * - In PlasmaBulletShooter component:
 *   - Drag PlasmaBullet prefab to "Bullet Prefab" slot
 * 
 * 6. Tags Setup:
 * ----------
 * - Select PlasmaBullet prefab in Project window
 * - In Inspector:
 *   - Tag: Set to "Bullet"
 * 
 * Testing:
 * -------
 * 1. Make sure player has "Player" tag
 * 2. Enter Play mode
 * 3. Right-click to shoot
 * 4. Bullets should:
 *    - Glow blue
 *    - Move straight
 *    - Not be affected by gravity
 *    - Propel player when hitting ground
 */

using UnityEngine;

// This is a guide script - it doesn't need to be attached to any GameObject
public class BulletSetup : MonoBehaviour
{
    void Start()
    {
        Debug.Log("This is a setup guide script. Please read the instructions in the script file.");
        enabled = false;
    }
} 