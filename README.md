# **Make New Way**

## Game
[CLICK HERE TO PLAY](https://y0-0go.itch.io/make-new-way)

10 Levels implemented.

## Technical Description

- Level itself has implements a system that tracks position of the blocks.
    - Used different types of blocks like Player, Movable, Obstacle etc. and managed their positions using Dictionaries. 
- Model-View-Controller-Service(MVC-S) pattern:
    - Level itself is implemented using MVC.
    - LevelView(Monobehaviour) collects data about level, gathers input and manages coroutines
    - LevelModel stores the data about blocks and their positions.
    - LevelController implements all the logic.
- Singletons: 
    - Services like Game Manager Service, Audio Service etc are created as Singletons.
- DoTween Package: used for implementing all the required motion mechanics
- Locked Levels and Unlocking system implemented.
- Undo Feature implemented using Stack.


## Screenshots
![Image](https://github.com/yogesh28-git/MakeNewWay/assets/85812175/998a1147-2d76-4950-9389-1eae074d9dab)
![Image](https://github.com/yogesh28-git/MakeNewWay/assets/85812175/2ace09fc-e5e4-4c82-860f-992c67efae2e)
![Image](https://github.com/yogesh28-git/MakeNewWay/assets/85812175/a314a56d-7519-4dbd-a878-56da0ca600f4)


