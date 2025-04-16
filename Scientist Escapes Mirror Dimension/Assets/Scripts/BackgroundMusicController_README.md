# BackgroundMusicController Script Documentation

## Overview
The BackgroundMusicController script manages the background music in the game, handling playback timing, looping, and volume control.

## Features

### Audio Settings
- **backgroundMusic**: The audio clip to be played as background music
- **volume**: Set to 0.08 (8% volume) for subtle background presence
- **BGM_DURATION**: Set to 22.5 seconds for initial playthrough

### Playback Behavior
- Plays the first 22.5 seconds of the audio clip
- After initial playthrough, loops the same 22.5 seconds indefinitely
- No position-based audio (3D sound disabled)
- Independent of camera/player position

## Technical Details

### Start Method
- Creates and configures AudioSource component
- Sets initial volume to 0.08
- Disables looping for initial playthrough
- Starts the background music playback

### PlayBackgroundMusic Coroutine
1. Initial Playthrough:
   - Plays the audio clip
   - Waits for 22.5 seconds
   - Marks initial playthrough as complete

2. Looping Phase:
   - Enables looping
   - Resets audio to start
   - Continues playing indefinitely

## Usage
1. Create an empty GameObject in the scene (not parented to anything)
2. Name it "AudioManager" or similar
3. Attach this script to the GameObject
4. Assign the background music audio clip in the Inspector
5. The music will automatically start playing when the scene begins

## Important Notes
- The GameObject should be at the root level of the scene
- Do not parent it to the camera or any moving objects
- Volume is set to 0.08 for subtle background presence
- The first 22.5 seconds play once, then loop indefinitely 