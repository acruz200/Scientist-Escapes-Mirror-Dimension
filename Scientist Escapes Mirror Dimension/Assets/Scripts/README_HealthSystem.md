# Health System

This system provides a health bar UI, player health management functionality, and visual feedback for damage.

## Setup Instructions

1. **Add the PlayerHealth script to your player character** (the GameObject with the PlayerMovement script).
2. The health bar UI will be automatically created and displayed in the top-left corner of the screen.
3. The damage flash effect will be automatically created and displayed when taking damage.

## Features

- **Health Bar UI**: Visual representation of current health with color-coding:
  - Green: Above 60% health
  - Yellow: Between 30-60% health
  - Red: Below 30% health
- **Health Text**: Shows the current health value out of maximum (e.g., "75/100")
- **Damage Flash**: Visual feedback when taking damage:
  - Red screen flash effect
  - 0.2 second duration
  - Semi-transparent overlay
- **Damage and Healing Functions**: Can be called from other scripts

## How to Use

### Taking Damage
To make the player take damage from another script (e.g., enemy attacks):

```csharp
// Get reference to the player's health component
PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

// Apply damage
playerHealth.TakeDamage(damageAmount);
```

### Healing
To heal the player:

```csharp
// Get reference to the player's health component
PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

// Apply healing
playerHealth.Heal(healAmount);
```

## Testing

For testing purposes, the PlayerHealth script includes a debug key:
- Press the H key to heal 10 health

## Customization

You can modify the following settings in the Inspector:
- **Max Health**: The player's maximum health (default: 100)
- **Current Health**: The player's current health (automatically set to max health at start)
- **Health Bar Scale**: Controls the size of the health bar UI (default: 4.0) 