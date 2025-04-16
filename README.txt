Scientist Escapes Mirror Dimension - Design Documentation

Project Overview:
A first-person shooter game where a scientist must escape from a mirror dimension using advanced technology and combat skills. The game combines 3D physics, AI behaviors, and Mecanim animations to create an immersive experience.

Design Principles and Rationale:

1. Gameplay Mechanics:
   - First-person perspective for immersive experience
   - Plasma gun mechanics with recoil and bullet physics
   - Mirror dimension theme reflected in level design and enemy behavior
   - Player propulsion system using bullet impacts for vertical movement
   - Crystal-based environmental puzzles and obstacles

2. Technical Implementation:
   - 3D physics system for realistic movement and interactions
   - AI-driven enemy behaviors for dynamic combat
   - Mecanim animations for character and object movements
   - Advanced lighting and particle effects for visual appeal

AI Constructs:

1. Enemy Patrol System
   - Implements pathfinding and patrol behavior
   - Enemies follow predefined paths and search for the player
   - Uses raycasting for player detection
   - Responds to player presence with appropriate behaviors

2. Mirror Guardian AI
   - Advanced state machine implementation (Patrol, Chase, Attack, Retreat)
   - Bayesian network for player movement prediction
   - Custom pathfinding system for complex navigation
   - Visual state changes (material changes, particle effects)
   - Health-based behavior modifications

3. Enemy Combat AI
   - Dynamic combat decision-making
   - Cover-taking and dodging behaviors
   - Damage response system
   - Health-based retreat mechanics

Mecanim Implementations:

1. Player Character Animations
   - Movement animations (walk, run, jump)
   - Combat animations (shooting, reloading)
   - State transitions for smooth movement
   - Blend trees for complex movement combinations

2. Enemy Character Animations
   - Patrol and combat animations
   - State-based animation transitions
   - Damage and death animations
   - Special mirror dimension animations

3. Crystal Object Animation
   - Continuous rotation animation
   - Environmental interaction animations
   - State-based animation control
   - Integration with gameplay mechanics

Component Integration:

1. Physics and AI Integration:
   - Rigidbody components for physical interactions
   - Collider components for detection and collision
   - AI scripts controlling enemy behaviors
   - Physics-based movement and combat

2. Animation and Gameplay Integration:
   - Mecanim controllers for character animations
   - Animation events for gameplay triggers
   - Blend trees for smooth movement transitions
   - State machines for behavior control

3. Visual and Audio Integration:
   - Particle systems for effects
   - Dynamic lighting for atmosphere
   - Sound effects for immersion
   - Visual feedback for player actions

Technical Requirements Met:

1. 3D Physics:
   - Rigidbody components
   - Collider components
   - Force application
   - Physics-based movement

2. AI Techniques:
   - Pathfinding
   - State machines
   - Decision making
   - Behavior trees

3. Mecanim:
   - Character animations
   - State machines
   - Blend trees
   - Animation events

4. Lights and Textures:
   - Dynamic lighting
   - Particle effects
   - Material properties
   - Visual effects

Gameplay Flow:
1. Player starts in the mirror dimension
2. Enemies patrol and search for the player
3. Player can use plasma gun for combat and propulsion
4. Crystals serve as both obstacles and tools
5. Mirror Guardians provide challenging combat encounters
6. Player must navigate through the dimension to escape

Design Rationale:
The game combines traditional FPS mechanics with unique mirror dimension elements:
- Plasma gun provides both combat and movement options
- Crystal puzzles add environmental challenges
- Mirror Guardians create dynamic combat scenarios
- Physics-based movement allows for creative gameplay
- Animations enhance immersion and feedback

Technical Achievements:
1. Complex AI system with multiple behavior states
2. Physics-based gameplay mechanics
3. Advanced animation system with state machines
4. Dynamic lighting and particle effects
5. Integrated combat and movement systems 


UI Changes
Issue: Health Bar Too Small
Fix: Enlarged and repositioned the health bar in the top-left corner to improve visibility and readability for the player.
Issue: No Start Screen

Fix: Added a main menu screen with options to "Start" or "Quit" the game, providing a proper entry point for players.
Issue: No End Game Screen

Fix: Created an end game screen that appears upon player win or death, giving players clear feedback that the game has concluded.
Issue: No Name Labels for Key Entities

Fix: Added floating name tags above important characters:
Player → "Player"

Enemy → "Bob"

Toilet Mob 
These tags are world-space text objects that always face the camera, making testing and identification easier during gameplay.

Sound Design
Commercial Reference: Resident Evil 2 Remake
We analyzed Resident Evil 2 Remake for its immersive use of atmospheric and reactive sounds. Key takeaways:

Footsteps vary by surface type
Enemy growls signal nearby danger
Background ambience uses subtle, low-impact sounds
Sounds are layered and volume-balanced for immersion

Implemented Sounds

Footsteps
File: lego-walking-208360.mp3
Where/When: Plays during player movement (PlayerMovement.cs)
Purpose: Medium-impact sound to provide motion feedback
Notes: Automatically starts/stops based on input; uses one consistent clip

Monster Growl
File: monster-growl-251374.mp3
Where/When: Plays once when enemy begins chasing (EnemyPatrol.cs)
Purpose: High-impact alert to increase player tension
Notes: Non-repeating, balanced volume

Door Sound
File: open-door-sound-247415.mp3
Where/When: Triggers when interacting with doors (DoorController.cs)
Purpose: Medium-impact interaction feedback

Light Switch Toggle
File: switch-150130.mp3
Where/When: Used when toggling lights on/off (LampController.cs, StreetLampController.cs)
Purpose: Low-impact, subtle audio cue
