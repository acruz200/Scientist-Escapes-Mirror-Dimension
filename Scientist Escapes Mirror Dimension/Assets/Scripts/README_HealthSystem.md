# Health System

This system provides a health bar UI and player health management functionality.

## Setup Instructions

1. **Add the PlayerHealth script to your player character** (the GameObject with the PlayerMovement script).
2. The health bar UI will be automatically created and displayed in the top-left corner of the screen.

## Features

- **Health Bar UI**: Visual representation of current health with color-coding:
  - Green: Above 60% health
  - Yellow: Between 30-60% health
  - Red: Below 30% health
- **Health Text**: Shows the current health value out of maximum (e.g., "75/100")
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

For testing purposes, the PlayerHealth script includes debug keys:
- Press the T key to take 10 damage
- Press the H key to heal 10 health

## Customization

You can modify the following settings in the Inspector:
- **Max Health**: The player's maximum health (default: 100)
- **Current Health**: The player's current health (automatically set to max health at start) 