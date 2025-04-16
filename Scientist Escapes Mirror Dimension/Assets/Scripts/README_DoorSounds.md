# Door Sound Implementation Guide

I've added audio functionality to the DoorController script. Here's how to set it up:

## Setup

1. **Assign Sound Files:**
   - Select each door GameObject in your scene
   - In the Inspector, under the "Audio Settings" section of the DoorController component:
   - Assign `open-door-sound-247415.mp3` to the "Door Open Sound" field
   - Find or create a door close sound and assign it to the "Door Close Sound" field

2. **Adjust Volume (Optional):**
   - Use the "Sound Volume" slider to adjust how loud the door sounds play
   - Default is 1.0 (full volume)

## Finding Door Close Sound

You already have a door open sound. For a door close sound, you can:

1. **Use the same sound** - Sometimes the same sound works for both opening and closing
2. **Reverse the open sound** - In an audio editor, reverse the open sound for a natural close sound
3. **Download a free sound** - Find door close sounds on sites like:
   - [Freesound.org](https://freesound.org/search/?q=door+close)
   - [Mixkit](https://mixkit.co/free-sound-effects/door/)
   - [Zapsplat](https://www.zapsplat.com/sound-effect-category/doors/)

## Audio Settings Explained

- **Spatial Blend**: Set to 1.0 for full 3D sound (volume changes based on player distance)
- **Min Distance**: How close the player needs to be for full volume (default: 2 meters)
- **Max Distance**: Distance at which the sound becomes inaudible (default: 10 meters)

## Testing

Press E to open/close doors when near them, and you should hear the corresponding sounds play.

## Troubleshooting

- If no sound plays, check that:
  - Audio clips are properly assigned
  - Door GameObject has an AudioSource component
  - Audio is not muted in your game settings
  - Sound volume is above 0

- If sounds are too quiet:
  - Increase the "Sound Volume" value
  - Decrease the "Min Distance" value
  - Check if the audio file itself is quiet and adjust accordingly 