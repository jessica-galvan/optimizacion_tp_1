ESTRUCTURES
● Custom Update Manager (*) - DONE
● Non Alloc API (*) - DONE
● ObjectPool (*) - DONE 
● Precomputation - DONE
● Caching - DONE
● Hashing - DONE
● Documentar al menos 1 optimización de Heap aplicada. (*) - DONE

● Documentar al menos 1 optimización de Stack aplicada. (*) - NOT SURE??? 
● Lazy computation - NOT SURE???
● Batching - NOT SURE???

● Especialización
● Optimización del “Expected Path”

DONE LOCATIONS:
- Heap aplicada:
	 LevelGrid. on start/awake, Clear the memory of the baked data that will not be used after reconstructing dictionary;

-  ObjectPool
	 Pool Manager for Enemies, Bullets and particles. 

- Custom Update Manager
	CustomUpdateManager.cs

- Non Alloc:
	EntityModel. For CanMoveFoward and EnemyModel.cs to check body collision with player. not bullets.

- Hashing
	The Cell grid system. 

- Caching: 
	any get component on player/enemy. Check EntityModel.Initialize();

- Precomputation: 
	EnemyManager: precalculate the weight.  LevelGrid realSize too (cuz even if the grid looks like 12x12 we add two extra rows for border;

CHECK:
- Batching: static structures. like undestructible walls and floor?
- Lazy Computation: EnemyManager canSpawnEnemy logic??? 

Extras:
- Particles for death and bullet impact with pool manager.
- Slicing of frames so we don't do some logic all the time as it not necessary (Check EnemyController Update and EnemyModel)
- Grid System and Level Grid Editor Tool for easier level set up.
- Pause Menu

