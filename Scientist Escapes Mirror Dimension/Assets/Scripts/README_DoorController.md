# Door Controller

This script allows you to create interactive doors that swing open and closed when the player interacts with them.

## Setup Instructions

1. **Add the DoorController script to your door object** (e.g., "Door_03").
2. Configure the door settings in the Inspector to match your door's orientation and desired behavior.

## Features

- **Smooth Door Animation**: Doors swing open and closed with a smooth animation
- **Player Interaction**: Players can open/close doors when in range
- **Customizable Door Behavior**: Adjust open angle, speed, and direction
- **Visual Range Indicator**: See the interaction range and door swing direction in the Unity Editor

## How to Use

### Basic Setup
1. Select your door object in the Hierarchy
2. Add the DoorController script to it
3. Configure the door settings in the Inspector:
   - **Open Angle**: How far the door swings open (default: 90 degrees)
   - **Open Speed**: How fast the door opens/closes (default: 2)
   - **Open Forward**: Whether the door opens forward or backward (default: true)
   - **Rotation Axis**: Axis around which the door rotates (default: up)

### Door Orientation
For the door to swing correctly, make sure:
1. The door's pivot point is at the hinge (where it should rotate from)
2. The door's forward direction (blue arrow in Scene view) faces the direction it should swing
3. If the door swings incorrectly, try toggling the "Open Forward" option

### Interaction Settings
- **Can Be Toggled**: Enable/disable player interaction with the door
- **Toggle Key**: Key to press to open/close the door (default: E)
- **Interaction Distance**: How close the player needs to be to interact (default: 3 units)
- **Prompt Message**: Custom message to display when player is in range

### Script Control
You can also control the door from other scripts:

```csharp
// Get reference to the door controller
DoorController doorController = doorObject.GetComponent<DoorController>();

// Open the door
doorController.OpenDoor();

// Close the door
doorController.CloseDoor();

// Change the open angle
doorController.SetOpenAngle(120f); // Opens wider
```

## Tips

- If your door model has a complex hierarchy, make sure to add the script to the root object that should rotate
- For doors that should slide instead of swing, consider creating a separate SlidingDoorController script
- You can create locked doors by setting "Can Be Toggled" to false and controlling the door through other game events 