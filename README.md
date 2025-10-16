# Sunny Land
## 2D Mini Game Design Document
By My Lien Tan

### Game Concept and Inspiration

A 2D Platformer inspired by classic retro platform games like Super Mario Bros. The main character is a little fox traveling in Sunny Land. This is a peaceful, colorful and natural land, evoking a peaceful feeling like walking in a garden next to the sea, with warm sunlight covering the space. In addition to the peaceful scenery, this land also contains many mysteries, such as the animals and hazards that the little fox must overcome to collect 10 cherries. My inspiration came from the desire to recreate the nostalgic feel of platform games while adding personalized creative touches like enemies (pig, frog, bear), sound effects, and UI screens (start, win, lose).

### Core Mechanics
#### <ins>_Movement:_</ins> 
Player can run left/right and jump to traverse the map.

#### <ins>_Collectibles:_</ins>
Cherries are scattered across the level; collecting them increases score. A total score of 10 cherries will be a win.

#### <ins>_Combat/Interaction with Enemies:_</ins>

##### 🔶 Pig enemy 🐷: 
Patrols between two points. Can be defeated by stomping on its head. Touching it from the sides will damage the player. Player will lose 1 heart.

##### 🔶 Frog enemy 🐸: 
Stationary hazard. Can be defeated by stomping but damages from the sides. Player will lose 1 heart.

##### 🔶 Bear enemy 🐻: 
Patrols like the pig, but indestructible. Any contact causes instant game over.

##### 🔶 Hazards: 
Spikes, crates, and falling into the death zone all damage or kill the player. 
Spikes are skulls which will launch to the sky when player touch an area that triggers it. Most of the time, the player cannot avoid the launching of the spike but they can avoid being hurt by it. Touching the spikes when they launch will damage the player and they will lose a heart.
Crates: crates in the game serve as moving platforms that rise and fall vertically. Two crates are placed in the level, each moving at a different speed: one moves quickly to create a timing challenge for the player, while the other moves more slowly and can be used as a tool to reach higher areas, such as platforms where cherries are located. If the player is caught underneath a crate while it descends, they will be “crushed” and lose one heart. This mechanic adds variety to movement and encourages strategic use of the environment. Finally, falling out of the map results in an instant game over, reinforcing the importance of careful platforming and precise jumps.

#### <ins>_UI:_</ins> 
Score counter, heart system for health. Start, win and game over panels along with buttons to start the game or play again.

![Image](https://github.com/user-attachments/assets/5bcbd93c-4812-47fc-a539-78ff97c894d6)

#### <ins>_Feedback:_</ins> 
When the player successfully collects a cherry, a stardust effect will appear. When the player stomps a pig or frog, an explosion effect will appear, signaling that these enemies have been destroyed.

### Win/lose conditions 
#### <ins>_Win Condition:_</ins> 🍒
Collect 10 cherries to complete the level. 
A “You Win!” screen appears with a restart option.
#### <ins>_Lose Condition:_</ins> ☠️
- Lose all lives/hearts.
- Fall off the screen into a void.
- A “Game Over” screen appears with a restart option.

![Image](https://github.com/user-attachments/assets/9f0c6d13-bc70-463b-a8e3-fd8f966b3181)

### What creative additions you implemented
#### <ins>_Animated Character Sprites_</ins>
The player has unique animations for different states:  
- Idle: when standing still.
- Running: when moving horizontally.
- Jumping: when airborne.
- Enemies like pigs, frogs, and bear also have animations such as idle, walking, or running, which bring life and personality to the game world. 

#### <ins>_Background Music_</ins>
A looping soundtrack plays during gameplay, adding atmosphere and enhancing player immersion.
