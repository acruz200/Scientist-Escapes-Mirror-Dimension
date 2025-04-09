/* ENEMY ANIMATOR SETUP GUIDE
 * 
 * This file contains instructions on how to set up the Mecanim animator controller for the enemy.
 * Follow these steps to create the animator in Unity:
 * 
 * 1. Creating the Animator Controller:
 * -------------------------------
 * a) In the Project window, right-click > Create > Animator Controller
 * b) Name it "EnemyAnimator"
 * c) Drag this animator controller onto your enemy GameObject in the Inspector
 * 
 * 2. Setting up Parameters:
 * ---------------------
 * Add the following parameters to your animator:
 * - IsPatrolling (bool)
 * - IsChasing (bool)
 * - IsAttacking (bool)
 * - IsTakingCover (bool)
 * - IsStunned (bool)
 * - Speed (float)
 * 
 * 3. Creating Animation States:
 * ------------------------
 * Create the following animation states:
 * - Idle
 * - Patrol
 * - Chase
 * - Attack
 * - TakeCover
 * - Stunned
 * 
 * 4. Setting up Transitions:
 * ---------------------
 * Create transitions between states based on the parameters:
 * - Idle -> Patrol: When IsPatrolling is true
 * - Patrol -> Chase: When IsChasing is true
 * - Chase -> Attack: When IsAttacking is true
 * - Any State -> TakeCover: When IsTakingCover is true
 * - Any State -> Stunned: When IsStunned is true
 * - TakeCover/Stunned -> Idle: When all state bools are false
 * 
 * 5. Creating a Blend Tree for Movement:
 * ---------------------------------
 * a) Create a new state called "Movement"
 * b) In the Inspector, set Motion to "Blend Tree"
 * c) Add your walk/run animations to the blend tree
 * d) Set the blend parameter to "Speed"
 * e) Configure the thresholds for each animation (e.g., 0 for idle, 1 for walk, 3 for run)
 * 
 * 6. Connecting the Blend Tree:
 * ------------------------
 * - Make the Movement state transition to/from other states based on Speed > 0
 * - Set the Movement state as a layer that blends with other states
 * 
 * 7. Testing the Animator:
 * -------------------
 * a) Enter Play mode
 * b) Use the Animator window to see the state transitions
 * c) Adjust transition settings for smooth animations
 * 
 * Note: You'll need appropriate animation clips for each state.
 * If you don't have animations, you can use the Unity Animation window
 * to create simple animations or download free animation packs from the Asset Store.
 */

// This is a guide script only - it doesn't need to be attached to any GameObject
using UnityEngine;

// Instructions only - this script does nothing when added to a GameObject
public class EnemyAnimatorSetup : MonoBehaviour
{
    // This script contains only setup instructions
    void Start()
    {
        Debug.Log("This is a setup guide script, not meant to be attached to a GameObject. Please read the instructions inside the script file.");
        
        // Disable this script
        enabled = false;
    }
} 