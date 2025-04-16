# BackgroundMusicController Script Documentation

## Overview
The BackgroundMusicController script manages the background music in the game, handling playback timing, looping, volume control, and user interaction.

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

### User Interaction
- Press 'M' key to toggle music on/off
- Visual feedback through music icon in top-right corner:
  - Shows music icon when music is playing
  - Shows muted icon when music is paused
- Music state persists between game sessions

## Technical Details

### Start Method
- Creates and configures AudioSource component
- Sets initial volume to 0.08
- Disables looping for initial playthrough
- Starts the background music playback
- Initializes UI icon state

### PlayBackgroundMusic Coroutine
1. Initial Playthrough:
   - Plays the audio clip
   - Waits for 22.5 seconds
   - Marks initial playthrough as complete

2. Looping Phase:
   - Enables looping
   - Resets audio to start
   - Continues playing indefinitely

### ToggleMusic Method
- Handles M key press detection
- Toggles between play and pause states
- Updates UI icon to reflect current state
- Maintains music position when toggling

## Usage
1. Create an empty GameObject in the scene (not parented to anything)
2. Name it "AudioManager" or similar
3. Attach this script to the GameObject
4. Assign the background music audio clip in the Inspector
5. Set up the music icon UI elements:
   - Assign musicOnSprite and musicOffSprite
   - Position the musicIcon Image component in the top-right corner
6. The music will automatically start playing when the scene begins

## Important Notes
- The GameObject should be at the root level of the scene
- Do not parent it to the camera or any moving objects
- Volume is set to 0.08 for subtle background presence
- The first 22.5 seconds play once, then loop indefinitely
- Music state is preserved when toggling, allowing seamless continuation 

## Audio Credits
- **Background Music**: "Quiet" by This Will Destroy You
- **Implementation Details**:
  - Volume set to 8% for subtle ambient presence
  - First 22.5 seconds played once, then looped
  - Can be toggled with 'M' key 