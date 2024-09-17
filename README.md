## Fantasy Snake

A Snake game with Fantasy RPG mixed. The player controls a growing line of Heroes to fight against Monster.

### Technical Decision
- For the game's visual and design approach, I decided to use a 2.5D style over the traditional 2D to make the gaming more interesting by placing 2D sprites within a 3D environment.

- For the flexibility I also add the option to customize initial settings via a JSON configuration file and all of the data are stored and by using ScriptableObject.

- Players can control the game using either a keyboard or a controller, by using Unity's new input system.

- The grid board is created and managed by the `GridManager` with the `SpawnObjectUtility` class to help spawning items on the grid.

- Every item in the game (except grid), inherits from the `WorldItem` class, which includes the `OnHit` method. These items are mainly categorized into three classes:

    - The `Character` class, with the two subclasses: the `Hero` and `Monster` classes.

    - The `Potion` class, which is inherited  by three subclasses: `HealingPotion`, `AttackUpPotion`, and `DamageUpPotion`.

    - The `Obstacle` class.

- To manage hero classes, I chose to use ScriptableObjects instead of enums. ScriptableObjects are more adept at handling custom data and offer a streamlined process for adding new classes in the future.

- The progression of game stages is managed by the GameState system. Each state has `OnStateIn` and `OnStateOut` methods, which are called whenever there is a transition between states.

- The game's audio is managed by the `SoundManager`, which plays sound based on pre-configured ScriptableObjects. While this method of sound management may not be the most efficient especially when compared to the flexibility of using Addressables with a reference table for asset loading, it is a fast solution that is suitable for smaller-scale projects with tight development timelines.

- Developed a custom animation system within the `CharacterAnimationUtility` to fully control animations via script, the 2D sprite animations that doesn’t require complex transitions, but just calling Play function.

- Decoupled the `GridTile` from the `WorldItem` to minimize dependencies, by having the `PlayerActionManager` to communicate between the player's character (Heroes) and the grid system.

- The most challenging part when implementing the `PlayerActionManager` is the character movement. I have created 2 queues that parallel each other in opposite directions: The first one is for the `Heros`(player) and the other is used to store `GridTile`. To move the player It just dequeues both queues.

- By the rule of `The player cannot move in the opposite direction, At all times.`, To mimic the snake game and offer a smooth gameplay, I decided to allow the player to move to the opposite direction when there is only 1 hero in the party.

- To enhance visual clarity, especially when a monster or hero takes no damage, I have created a status popup text. This feature will help players visualize the events occurring during gameplay.

- The core gameplay elements, along with additional features, are now complete. This includes everything except for the enhancement of monsters over time and level progression. Additional features such as settings, animations, and sound have also been added.


*Note: When create this project the latest Unity's latest LTS is 2022.3.45f1*

---


### Assets Used
[Board Game Icons](https://kenney.nl/assets/board-game-icons)    
[Fantasy UI Borders](https://kenney.nl/assets/fantasy-ui-borders)    
[Impact Sounds](https://kenney.nl/assets/impact-sounds)    
[Rogue Fantasy Castle](https://assetstore.unity.com/packages/2d/environments/rogue-fantasy-castle-164725)    
[Bandits - Pixel Art](https://assetstore.unity.com/packages/2d/characters/bandits-pixel-art-104130)    
[迫り来る危機 - 8bit](https://www.youtube.com/watch?v=NTeyaHTXjvM)    
[チャレンジ - 8bit #2](https://www.youtube.com/watch?v=76Ulg79AIXw)    
[ピコピコ・リパブリック賛歌 - 8bit](https://www.youtube.com/watch?v=lCSEr1R7Kac)    
