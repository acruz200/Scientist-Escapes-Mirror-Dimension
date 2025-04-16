# UIManager Script Documentation

## Overview
The UIManager script handles all UI elements in the game, including health bar, interaction prompts, dialogue, and visual feedback effects.

## Features

### Health Bar
- Visual health representation with color coding
- Numerical health display
- Customizable size and position
- Automatic updates when health changes

### Interaction Prompts
- Contextual prompts for player interactions
- Customizable message display
- Automatic show/hide based on player proximity
- Centered at bottom of screen

### Dialogue System
- Support for multi-line dialogue
- Customizable display duration
- Automatic text progression
- Centered at bottom of screen

### Damage Flash
- Visual feedback when player takes damage
- Red screen overlay effect
- Configurable duration (default: 0.2 seconds)
- Semi-transparent overlay (30% opacity)
- Smooth fade in/out animation

## Technical Details

### UI Components
- All UI elements are created dynamically if not assigned
- Uses Canvas for proper UI rendering
- Supports both TextMeshPro and standard UI elements
- Automatic scaling and positioning

### Damage Flash Implementation
- Full-screen overlay using Image component
- Smooth alpha transitions using coroutines
- Non-intrusive design (semi-transparent)
- Automatic cleanup after animation

## Usage

### Basic Setup
1. Add the UIManager script to an empty GameObject
2. The script will automatically create necessary UI components
3. Assign the UIManager reference to other scripts that need UI functionality

### Health Bar
```csharp
// Update health bar
uiManager.UpdateHealthBar(healthPercentage);

// Set health bar scale
uiManager.SetHealthBarScale(scaleFactor);
```

### Interaction Prompts
```csharp
// Show prompt
uiManager.ShowInteractionPrompt("Press E to interact");

// Hide prompt
uiManager.HideInteractionPrompt();
```

### Dialogue
```csharp
// Show dialogue
uiManager.ShowDialogue("Hello, world!");

// Hide dialogue
uiManager.HideDialogue();
```

### Damage Flash
The damage flash is automatically triggered when the player takes damage through the PlayerHealth script. No manual triggering is needed.

## Customization
All UI elements can be customized in the Inspector:
- Health bar position and size
- Prompt and dialogue text appearance
- Damage flash color and duration
- Overall UI scaling 