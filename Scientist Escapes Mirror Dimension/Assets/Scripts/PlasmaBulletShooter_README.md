# PlasmaBulletShooter Script Documentation

## Overview
The PlasmaBulletShooter script handles the creation and behavior of plasma bullets in the game, including shooting mechanics, visual effects, audio feedback, and weapon status indicators.

## Features

### Bullet Settings
- **bulletPrefab**: The GameObject prefab used for creating bullets
- **bulletSpawnPoint**: Transform where bullets are instantiated
- **bulletSpeed**: Speed at which bullets travel (default: 30f)
- **bulletLifetime**: How long bullets exist before being destroyed (default: 3f)
- **bulletColor**: Color of the bullets (default: blue)

### Shooting Settings
- **shootCooldown**: Time between shots (default: 0.70f)
- Right mouse button trigger for shooting
- Cooldown system to prevent rapid firing
- Visual cooldown indicator in bottom-right corner:
  - Blue circle when gun is ready to shoot
  - Red circle when gun is on cooldown

### Visual Effects
- **showMuzzleFlash**: Toggle for muzzle flash effect
- **muzzleFlashDuration**: How long the muzzle flash lasts (default: 0.05f)
- Dynamic light creation for muzzle flash
- Bullet scaling and visual properties:
  - Thin bullet shape (0.1f, 0.1f, 0.5f)
  - Custom material with emission for plasma effect
  - Light component for glow effect

### Audio Features
- **shootSound**: Audio clip played when shooting
- Sound playback starts at 0.35 seconds into the clip
- Plays for 0.60 seconds
- Stops any existing sound before playing a new one
- Sound plays exactly when bullet is created

### Physics and Collision
- Bullets use Rigidbody for movement
- Continuous dynamic collision detection
- No gravity effect on bullets
- Capsule collider for bullet collision (trigger mode)
- Bullet tag for collision detection

### Recoil System
- **recoilForce**: Force applied to player when shooting (default: 2.0f)
- **recoilUpwardForce**: Vertical component of recoil (default: 0.5f)
- Random torque for realistic recoil effect
- Notifies player movement script of shooting

### Player Integration
- Automatically finds player object
- Can be attached to player or weapon
- Ensures proper player tag assignment
- Integrates with player movement system

## Technical Details

### Start Method
- Sets up AudioSource component
- Finds and validates player object
- Creates bullet spawn point if not assigned
- Configures player references and tags
- Initializes cooldown indicator UI

### Update Method
- Checks for right mouse button input
- Enforces shooting cooldown
- Updates cooldown indicator UI state
- Triggers shooting when conditions are met

### ShootPlasmaBullet Method
- Creates bullet instance
- Applies visual effects
- Plays shooting sound
- Sets up bullet physics
- Applies recoil forces
- Configures bullet properties
- Updates cooldown indicator to red state

### Coroutines
- **ShowMuzzleFlash**: Creates and manages muzzle flash effect
- **PlayShootSound**: Handles shooting sound playback with specific timing

## Usage
1. Attach script to a GameObject (player or weapon)
2. Assign bullet prefab
3. Set up bullet spawn point
4. Configure audio clips and visual settings
5. Set up cooldown indicator UI:
   - Assign readySprite (blue circle) and cooldownSprite (red circle)
   - Position the cooldownIndicator Image component in the bottom-right corner
6. Adjust physics and recoil values as needed 