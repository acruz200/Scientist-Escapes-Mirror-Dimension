# Lamp Switch Sound Implementation Guide

I've added audio functionality to the LampController script. Here's how to set it up:

## Setup

1. **Assign Sound Files:**
   - Select each lamp GameObject in your scene
   - In the Inspector, under the "Audio Settings" section of the LampController component:
   - Assign a sound file to the "Switch On Sound" field
   - Assign a sound file to the "Switch Off Sound" field

2. **Adjust Volume (Optional):**
   - Use the "Sound Volume" slider to adjust how loud the switch sounds play
   - Default is 0.7 (70% volume)

## Finding Switch Sounds

For light switch sounds, you'll need to find or create appropriate audio files. Here are some options:

1. **Record your own** - Use your phone to record a real light switch click
2. **Download free sounds** - Find switch sounds on sites like:
   - [Freesound.org](https://freesound.org/search/?q=light+switch)
   - [Mixkit](https://mixkit.co/free-sound-effects/switch/)
   - [Zapsplat](https://www.zapsplat.com/sound-effect-category/switches-and-buttons/)

3. **Recommended free sounds:**
   - "Click" sounds work well for both on and off states
   - Short electrical "buzz" sounds can work for the on state
   - Quick "tick" sounds work for both states

## Audio Settings Explained

- **Spatial Blend**: Set to 1.0 for full 3D sound (volume changes based on player distance)
- **Min Distance**: How close the player needs to be for full volume (default: 1.5 meters)
- **Max Distance**: Distance at which the sound becomes inaudible (default: 8 meters)

## Testing

Press E to turn lamps on/off when near them, and you should hear the corresponding sounds play.

## Troubleshooting

- If no sound plays, check that:
  - Audio clips are properly assigned
  - Lamp GameObject has an AudioSource component
  - Audio is not muted in your game settings
  - Sound volume is above 0

- If sounds are too quiet:
  - Increase the "Sound Volume" value
  - Decrease the "Min Distance" value
  - Ensure the audio file itself is loud enough

## For Street Lamps

If you're using the StreetLampController script, you may need to add similar audio functionality to that script as well. The implementation would be very similar to what's been done for the LampController. 