# Street Lamp Controller

This script automatically lights up all GameObjects tagged with "StreetLamp" in your scene.

## Setup Instructions

1. Make sure all your street lamp GameObjects are tagged with "StreetLamp"
2. Add the `StreetLampController` script to any GameObject in your scene (preferably an empty GameObject named "StreetLampManager")
3. Adjust the settings in the Inspector to customize the appearance and behavior of the lamps

## Features

- **Automatic Setup**: Automatically finds and configures all objects tagged with "StreetLamp"
- **Point Lights**: Adds point lights to each lamp with customizable range and intensity
- **Visual Glow**: Optionally adds an emissive material to make the lamps visually glow
- **Flickering Effect**: Optional realistic flickering effect for the lamps
- **Shadow Casting**: Option to enable or disable shadows from the lamps

## Inspector Settings

### Light Settings
- **Light Intensity**: Brightness of the point light (default: 1.5)
- **Light Color**: Color of the light (default: warm white)
- **Light Range**: How far the light reaches (default: 15)
- **Cast Shadows**: Enable to make the light cast shadows (default: true)
- **Flicker**: Enable to make the light flicker realistically (default: false)
- **Flicker Intensity**: How much the light intensity varies when flickering (default: 0.2)
- **Flicker Speed**: How quickly the flickering occurs (default: 0.1)

### Visual Settings
- **Enable Glow**: Enable to make the lamp visually glow (default: true)
- **Glow Color**: Color of the lamp's glow (default: warm yellow)
- **Glow Intensity**: Brightness of the glow (default: 1.0)

## Usage Tips

- For a more realistic look, enable the Flicker option with low Flicker Intensity
- Adjust the Light Range based on the scale of your scene
- If your lamps already have materials, you may need to manually set up the emission

## Script API

The StreetLampController script provides these public methods that can be called from other scripts:

- `TurnOn()`: Turn on all street lamps
- `TurnOff()`: Turn off all street lamps
- `SetIntensity(float intensity)`: Change the light intensity of all lamps
- `SetGlowIntensity(float intensity)`: Change the glow intensity of all lamps 