# Lamp Controller

This script allows you to control a lamp's light in your game, including turning it on/off and customizing its properties.

## Setup Instructions

1. **Add the LampController script to your lamp object** (e.g., "Lamp_01").
2. The script will automatically find or create a Light component if one doesn't exist.

## Features

- **Automatic Light Creation**: If no Light component is found, one will be created automatically
- **Player Interaction**: Players can toggle the lamp on/off when in range
- **Customizable Light Properties**: Adjust intensity, color, and range in the Inspector
- **Light Position Control**: Easily adjust the light's position relative to the lamp
- **Visual Range Indicator**: See the interaction range and light position in the Unity Editor

## How to Use

### Basic Setup
1. Select your lamp object in the Hierarchy
2. Add the LampController script to it
3. Configure the light settings in the Inspector:
   - **Intensity**: Brightness of the light (default: 1.0)
   - **Light Color**: Color of the light (default: white)
   - **Range**: How far the light reaches (default: 10 units)
   - **Light Offset**: Position of the light relative to the lamp (default: 0, 1.5, 0)

### Adjusting Light Position
You can adjust the light's position in two ways:

1. **In the Inspector**: Modify the "Light Offset" values
   - X: Left/right offset (positive = right)
   - Y: Up/down offset (positive = up)
   - Z: Forward/backward offset (positive = forward)

2. **Via Script**: Call the SetLightPosition method
   ```csharp
   // Get reference to the lamp controller
   LampController lampController = lampObject.GetComponent<LampController>();
   
   // Set a new position (higher up)
   lampController.SetLightPosition(new Vector3(0, 2.5f, 0));
   ```

### Interaction Settings
- **Can Be Toggled**: Enable/disable player interaction with the lamp
- **Toggle Key**: Key to press to toggle the lamp (default: E)
- **Interaction Distance**: How close the player needs to be to interact (default: 3 units)

### Script Control
You can also control the lamp from other scripts:

```csharp
// Get reference to the lamp controller
LampController lampController = lampObject.GetComponent<LampController>();

// Turn the lamp on
lampController.TurnOn();

// Turn the lamp off
lampController.TurnOff();

// Toggle the lamp (on if off, off if on)
lampController.ToggleLight();
```

## Tips

- If your lamp model already has a Light component, the script will use that instead of creating a new one
- You can see the light's position in the Scene view when the lamp is selected (white wire sphere)
- For more realistic lighting, consider using shadows and adjusting the light's intensity based on your scene's lighting 