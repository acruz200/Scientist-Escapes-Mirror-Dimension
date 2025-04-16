# Scripts Documentation

## BackgroundMusicController
Controls the game's background music system.

### Features
- Toggles background music with 'M' key
- Visual UI indicator in top-right corner
- Persists music state using PlayerPrefs
- Smooth volume transitions

### Usage
1. Attach to an AudioManager GameObject
2. Assign music icon sprites in Inspector:
   - Music On Sprite
   - Music Off Sprite
3. Set up UI Image component for the icon

## PlasmaBulletShooter
Handles the player's plasma gun mechanics.

### Features
- Configurable cooldown system (default: 1 second)
- Visual cooldown indicator
- Color-based status feedback:
  - Blue: Ready to shoot
  - Red: On cooldown
- Right-click to fire
- Includes particle effects and sound

### Usage
1. Attach to player's weapon
2. Configure in Inspector:
   - Assign bullet prefab
   - Set cooldown duration
   - Add cooldown indicator sprites
3. Set up UI Image for cooldown display

### Technical Notes
- Uses Unity's UI system for visual feedback
- Implements sprite-swapping for status indication
- Manages cooldown timing via Time.time comparison 