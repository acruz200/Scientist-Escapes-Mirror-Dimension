# Sun Controller

This script creates a sun object in your game world that can serve as both a visual element and a light source.

## Setup Instructions

1. Create a new empty GameObject in your scene and name it "Sun"
2. Position the Sun object high in the sky (recommended Y position: 50-100 units)
3. Add the `SunController` script to the Sun GameObject
4. Adjust the settings in the Inspector to customize the sun's appearance and behavior

## Features

- **Visual Sun**: Creates a glowing yellow sphere that represents the sun
- **Dynamic Lighting**: Optionally creates a directional light that follows the sun
- **Day/Night Cycle**: Sun can rotate across the sky to simulate day and night
- **Customizable Appearance**: Adjust the sun's color, size, and intensity
- **Shadow Casting**: Option to enable or disable shadows from the sun's light

## Inspector Settings

### Sun Settings
- **Sun Intensity**: Controls how bright the sun appears (default: 1.5)
- **Sun Color**: The color of the sun (default: warm yellow)
- **Sun Size**: The size of the sun sphere (default: 1)
- **Rotate With Time**: Enable to make the sun move across the sky (default: true)
- **Day Length**: How long a full day/night cycle takes in seconds (default: 600 seconds / 10 minutes)

### Light Settings
- **Create Light**: Enable to create a directional light that follows the sun (default: true)
- **Light Intensity**: Brightness of the directional light (default: 1.2)
- **Light Color**: Color of the directional light (default: warm white)
- **Cast Shadows**: Enable to make the light cast shadows (default: true)

## Usage Tips

- For a more realistic sun, increase the Sun Size and decrease the Sun Intensity
- Adjust the Day Length to control how quickly the sun moves across the sky
- If you don't want the sun to move, disable the Rotate With Time option
- The sun's light will automatically point in the opposite direction of the sun's position

## Script API

The SunController script provides these public methods that can be called from other scripts:

- `SetTimeOfDay(float timeOfDay)`: Set the time of day (0-1, where 0 is sunrise and 0.5 is noon)
- `SetSunIntensity(float intensity)`: Change the sun's visual intensity
- `SetLightIntensity(float intensity)`: Change the directional light's intensity 