## Fantasy Snake

A Snake game with a Fantasy RPG theme. The player controls a growing line of Heroes to fight against Monster.

### Game Control
- `W`, `A`, `S`, `D` on keyboard or `D-Pad` on gamepad to move.
- `Q`, `E` on keyboard or `left shoulder`, `right shoulder` on gamepad to rotate hero characters in the line.
- `ESC` on the keyboard or `Start` on the gamepad to enter the settings screen.


### Gameplay Mechanics
- Every character, including Heroes and Monsters, have three stats: `Health`, `Attack`, and `Defense`.

- When the player moves a character which can result in collisions with other entities.
  - Colliding with the *Hero line* ends the game.
  - Colliding with a *Collectable Hero* adds them to the end of the line and triggers new hero spawns.
  - Colliding with an *Obstacle* removes the front hero from the line.  
  - Colliding with a *Monster* initiates a battle.

- Battle System
  - Damage calculation using the formula: Damage = (Attacker's Attack - Defender's Defense).
  - Defeating a Monster will remove it from the board and spawn new Monsters.
  - Losing Hero will be removed from the line, then moves the remaining hero forward.

- Characters belong to one of three classes and each class has an advantage over another.
  - `Warrior` deals double damage to `Rogue`
  - `Rogue` deals double damage to `Wizard`
  - `Wizard` deals double damage to `Warrior`.

- Players can collect items that recover health or boost Attack or Defense stats.


*Note: To prevent issues with file loading from Git LFS, use `git clone` rather than downloading a ZIP file*

*Note: The game must be start from the `StartScene`*

*Note: This project is developed using Unity LTS 2022.3.45f1.*

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
