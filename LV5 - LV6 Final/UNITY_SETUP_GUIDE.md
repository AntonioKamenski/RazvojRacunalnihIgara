# Unity Setup Guide — Space Shooter (Waves + Boss + Powerups)

This guide walks you through every step needed to connect the scripts to Unity.
Follow the steps in order. Every inspector field mentioned must be filled in.

---

## OVERVIEW OF GAME FLOW

```
Start Screen  →  [Press Start]
Wave 1        →  [All enemies killed]
Powerup Pick  →  [Choose 1 of 2]
Wave 2        →  [All enemies killed]
Powerup Pick  →  [Choose 1 of 2]
Wave 3        →  [All enemies killed]
Powerup Pick  →  [Choose 1 of 2]
Boss Fight    →  [Boss HP = 0]  →  Win Screen
              →  [Player HP = 0]  →  Lose Screen  (can happen at any point)
```

---

## PART 1 — PHYSICS LAYERS & TAGS

This prevents player lasers from hitting the player and enemy lasers from hitting enemies.

### 1.1 Create Layers

Go to **Edit → Project Settings → Tags and Layers**.

Under **Layers**, add the following in the first available slots:
- `Player`
- `Enemy`
- `PlayerProjectile`
- `EnemyProjectile`

### 1.2 Configure Collision Matrix

Go to **Edit → Project Settings → Physics 2D**, scroll to the **Layer Collision Matrix** at the bottom.

Uncheck (disable collision between):
- `Player` vs `Player`
- `Player` vs `PlayerProjectile`
- `Enemy` vs `Enemy`
- `Enemy` vs `EnemyProjectile`
- `PlayerProjectile` vs `PlayerProjectile`
- `EnemyProjectile` vs `EnemyProjectile`
- `PlayerProjectile` vs `EnemyProjectile`

Keep checked (enable collision between):
- `Player` vs `EnemyProjectile`   ← player takes damage from enemy lasers
- `Enemy` vs `PlayerProjectile`   ← enemies take damage from player lasers
- `Enemy` vs `Player`             ← optional (if you want contact damage)

---

## PART 2 — PREFABS

### 2.1 Player Laser Prefab

1. Create an empty GameObject, name it `PlayerLaser`.
2. Add a **Sprite Renderer** — assign a small vertical laser sprite.
3. Add a **Rigidbody2D**: Gravity Scale = 0, Collision Detection = Continuous.
4. Add a **BoxCollider2D**: IsTrigger = **false**, fit it to the sprite.
5. Add the **DamageDealer** script: set `damage = 100`.
6. Set the Layer to **PlayerProjectile**.
7. Drag to `Assets/Prefabs/` to create the prefab.

### 2.2 Enemy Laser Prefab

1. Create an empty GameObject, name it `EnemyLaser`.
2. Add **Sprite Renderer** — use a different color sprite than the player laser.
3. Add **Rigidbody2D**: Gravity Scale = 0, Collision Detection = Continuous.
4. Add **BoxCollider2D**: IsTrigger = **false**.
5. Add **DamageDealer** script: `damage = 50`.
6. Set Layer to **EnemyProjectile**.
7. Save to `Assets/Prefabs/EnemyLaser.prefab`.

### 2.3 Boss Laser Prefab

1. Duplicate the EnemyLaser prefab, rename to `BossLaser`.
2. Optionally change the sprite color to make it visually distinct.
3. Set Layer to **EnemyProjectile**.
4. **DamageDealer** `damage = 75` (or higher for the boss).
5. Save to `Assets/Prefabs/BossLaser.prefab`.

### 2.4 Enemy Prefab

1. Create an empty GameObject, name it `Enemy`.
2. Add **Sprite Renderer** — assign an enemy ship sprite.
3. Add **Rigidbody2D**: Gravity Scale = 0, Body Type = Kinematic.
4. Add **PolygonCollider2D** (or BoxCollider2D): IsTrigger = **true**.
5. Add the **Enemy** script:
   - `Health` = 300
   - `Projectile Prefab` = EnemyLaser prefab
   - `Projectile Speed` = 10
   - `Min Time Between Shots` = 1.0
   - `Max Time Between Shots` = 3.0
6. Add the **EnemyPath** script (leave Wave Config empty — it is assigned at runtime).
7. Set Layer to **Enemy**.
8. Save to `Assets/Prefabs/Enemy.prefab`.

### 2.5 Boss Prefab

This is the most detailed prefab.

**Root object:**
1. Create an empty GameObject, name it `Boss`.
2. Add **Sprite Renderer** — use a large enemy sprite, or scale it up (Scale X=3, Y=3).
3. Add **Rigidbody2D**: Gravity Scale = 0, Body Type = Kinematic.
4. Add **BoxCollider2D**: IsTrigger = **true**. Resize to cover the boss sprite.
5. Add the **Boss** script:
   - `Health` = 3000
   - `Move Speed` = 2.5
   - `Left Bound` = -7
   - `Right Bound` = 7
6. Set Layer to **Enemy**.

**Four child shooter objects (add these as children of Boss):**

For each shooter (name them `Shooter1`, `Shooter2`, `Shooter3`, `Shooter4`):
1. Right-click the Boss root → Create Empty → rename to `ShooterN`.
2. Set the positions (local) to spread them across the boss width:
   - Shooter1: Position = (-2.5, -0.5, 0)
   - Shooter2: Position = (-0.8, -0.5, 0)
   - Shooter3: Position = ( 0.8, -0.5, 0)
   - Shooter4: Position = ( 2.5, -0.5, 0)
3. Add **BossLaserShooter** script to each:
   - `Laser Prefab` = BossLaser prefab
   - `Projectile Speed` = 8
   - `Min Time Between Shots` = 0.4
   - `Max Time Between Shots` = 1.8
   - `Spread Angle` (set a different angle on each):
     - Shooter1: `-30`
     - Shooter2: `-10`
     - Shooter3: `10`
     - Shooter4: `30`

7. Drag the Boss root to `Assets/Prefabs/Boss.prefab` to save it.

---

## PART 3 — WAVE PATH PREFABS

Each wave needs a path for enemies to follow.
You need **3 path prefabs** (one per wave).

### For each path (repeat for Path1, Path2, Path3):

1. Create an empty GameObject, name it `EnemyPath_Wave1` (adjust number for each).
2. Add **child empty GameObjects** as waypoints (name them `Waypoint0`, `Waypoint1`, etc.).
   - Position them to define the movement route across the screen.
   - A typical path: start off-screen top-right → sweep left → sweep right → exit off-screen bottom.
   - Example waypoints (adjust to your camera/world size):
     ```
     Waypoint0: (12, 0, 0)    ← enters from right
     Waypoint1: ( 5, 2, 0)
     Waypoint2: (-5, 2, 0)
     Waypoint3: (-5,-2, 0)
     Waypoint4: ( 5,-2, 0)
     Waypoint5: (12,-6, 0)    ← exits screen
     ```
3. Save each path root to `Assets/Prefabs/EnemyPath_Wave1.prefab` etc.
   **Do NOT place these paths in the scene** — they are referenced only by WaveConfig.

---

## PART 4 — WAVE CONFIG SCRIPTABLE OBJECTS

You need 3 WaveConfig assets (one per wave).

1. In the **Project** window, right-click `Assets/` → **Create → Enemy Wave Config**.
2. Name it `Wave1Config`.
3. In the Inspector:
   - `Enemy Prefab` = Enemy prefab
   - `Path Prefab` = EnemyPath_Wave1 prefab
   - `Time Between Spawns` = 0.8
   - `Number Of Enemies` = 6
   - `Move Speed` = 2.0
4. Repeat, creating `Wave2Config` (10 enemies, faster) and `Wave3Config` (14 enemies, faster still):
   - Wave2: `Number Of Enemies` = 10, `Move Speed` = 2.5, `Time Between Spawns` = 0.6
   - Wave3: `Number Of Enemies` = 14, `Move Speed` = 3.0, `Time Between Spawns` = 0.5

---

## PART 5 — SCENE HIERARCHY

Your scene should look like this:

```
Scene
├── GameManager          (Empty GameObject)
├── UIManager            (Empty GameObject)
├── PowerupManager       (Empty GameObject)
├── EnemySpawner         (Empty GameObject)
├── TopCollider          (Empty GameObject — already exists)
├── Player               (Sprite + scripts)
├── Main Camera
└── Canvas               (UI Canvas, Screen Space - Overlay)
    ├── StartScreen      (Panel)
    │   ├── TitleText
    │   └── StartButton
    ├── WinScreen        (Panel)
    │   ├── WinText
    │   └── RestartButton
    ├── LoseScreen       (Panel)
    │   ├── LoseText
    │   └── RestartButton
    ├── PowerupPanel     (Panel)
    │   ├── HeaderText
    │   ├── Button1      (Button)
    │   │   ├── Name1    (TextMeshPro - UGUI child)
    │   │   └── Desc1    (TextMeshPro - UGUI child)
    │   └── Button2      (Button)
    │       ├── Name2    (TextMeshPro - UGUI child)
    │       └── Desc2    (TextMeshPro - UGUI child)
    └── WaveLabel        (TextMeshPro - UGUI, always visible HUD)
```

---

## PART 6 — BUILDING THE CANVAS UI

### 6.1 Create the Canvas

1. GameObject → UI → Canvas. Set **Render Mode** to `Screen Space - Overlay`.
2. Add an **EventSystem** if Unity didn't create one automatically.

### 6.2 Start Screen Panel

1. Right-click Canvas → UI → Panel. Name it `StartScreen`.
2. Set its color to a dark semi-transparent color.
3. Inside it, add:
   - **UI → Text - TextMeshPro** named `TitleText`.
     - Text: `SPACE SHOOTER`
     - Font Size: 72, centered.
   - **UI → Button - TextMeshPro** named `StartButton`.
     - Child text: `START`
     - Position it below the title.

### 6.3 Win Screen Panel

1. Duplicate StartScreen, rename to `WinScreen`.
2. Change TitleText to `YOU WIN!`.
3. Rename StartButton to `RestartButton`, change text to `PLAY AGAIN`.
4. Hide this panel by default: uncheck the checkbox next to the panel name in the Inspector.

### 6.4 Lose Screen Panel

1. Duplicate StartScreen, rename to `LoseScreen`.
2. Change TitleText to `GAME OVER`.
3. Rename the button to `RestartButton`, text = `TRY AGAIN`.
4. Hide by default: uncheck in Inspector.

### 6.5 Powerup Panel

1. Right-click Canvas → UI → Panel. Name it `PowerupPanel`.
2. Add **Text - TextMeshPro** named `HeaderText`. Text: `Choose a Powerup:`. Centered at top.
3. Add two **Button - TextMeshPro** objects, named `Button1` and `Button2`.
   - Inside each button, add two child **Text - TextMeshPro** objects:
     - `Name` (larger font, e.g. 36) — powerup title
     - `Desc` (smaller font, e.g. 24) — powerup description
   - Layout suggestion: place buttons side-by-side in the center.
4. Hide by default: uncheck in Inspector.

### 6.6 Wave Label (HUD)

1. Right-click Canvas → UI → Text - TextMeshPro. Name it `WaveLabel`.
2. Anchor it to top-center.
3. Leave text empty (GameManager will fill it in at runtime).

---

## PART 7 — SCRIPT ASSIGNMENTS

### 7.1 GameManager Object

1. Select the `GameManager` empty GameObject.
2. Add Component → **GameManager** script.
   - No fields to assign — it is a singleton with no serialized references.

### 7.2 UIManager Object

1. Select the `UIManager` empty GameObject.
2. Add Component → **UIManager** script.
3. Assign in the Inspector:
   - `Start Screen` → drag `StartScreen` panel
   - `Win Screen` → drag `WinScreen` panel
   - `Lose Screen` → drag `LoseScreen` panel
   - `Start Button` → drag `StartButton`
   - `Restart Button Win` → drag the RestartButton inside WinScreen
   - `Restart Button Lose` → drag the RestartButton inside LoseScreen
   - `Wave Label` → drag `WaveLabel` TextMeshPro

### 7.3 PowerupManager Object

1. Select the `PowerupManager` empty GameObject.
2. Add Component → **PowerupManager** script.
3. Assign in the Inspector:
   - `Powerup Panel` → drag `PowerupPanel`
   - `Header Text` → drag `HeaderText`
   - `Powerup Button 1` → drag `Button1`
   - `Powerup Name 1` → drag `Name1` (TMP child of Button1)
   - `Powerup Desc 1` → drag `Desc1` (TMP child of Button1)
   - `Powerup Button 2` → drag `Button2`
   - `Powerup Name 2` → drag `Name2` (TMP child of Button2)
   - `Powerup Desc 2` → drag `Desc2` (TMP child of Button2)

### 7.4 EnemySpawner Object

1. Select the `EnemySpawner` empty GameObject.
2. Add Component → **EnemySpawner** script.
3. Assign in the Inspector:
   - `Wave Configs` (List, size = 3):
     - Element 0 = `Wave1Config`
     - Element 1 = `Wave2Config`
     - Element 2 = `Wave3Config`
   - `Boss Prefab` → drag the `Boss` prefab from Assets/Prefabs/
   - `Boss Spawn Position` = (0, 5, 0)  ← adjust to appear at the top of your screen

### 7.5 Player Object

1. Select your Player sprite in the scene.
2. It should already have **Player** script attached. Verify fields:
   - `Move Speed` = 10
   - `Laser Prefab` → drag `PlayerLaser` prefab
   - `Projectile Speed` = 10
   - `Firing Rate` = 0.1
   - `Health` = 500
3. Set the Player's Layer to **Player**.

### 7.6 TopCollider Object

1. Should already exist and have the **TopColider** script.
2. Ensure it has a **BoxCollider2D** that spans the full top edge of the screen, IsTrigger = true.
3. The layer can remain Default — it just destroys whatever enters it.

---

## PART 8 — BUILD SETTINGS

1. Go to **File → Build Settings**.
2. Click **Add Open Scenes** to add your game scene.
3. Make sure it appears at index 0. This is required for the Restart button (which reloads the scene by name/index).

---

## PART 9 — TEXTMESHPRO

If you get compile errors about `TMPro` namespace not found:

1. Go to **Window → Package Manager**.
2. Search for **TextMeshPro** and install it.
3. After installing, go to **Window → TextMeshPro → Import TMP Essential Resources**.

---

## PART 10 — HOW EVERYTHING CONNECTS (SUMMARY)

```
[GameManager]
  Fires events: OnGameStart, OnWaveComplete, OnNextWave, OnBossFight, OnWin, OnLose

[UIManager]          listens to: OnGameStart, OnWaveComplete, OnBossFight, OnWin, OnLose
[PowerupManager]     listens to: OnWaveComplete
  → calls GameManager.PowerupSelected() after player picks

[EnemySpawner]       listens to: OnGameStart, OnNextWave, OnBossFight
  → spawns enemies per wave
  → listens to Enemy.OnEnemyDied (static event) to count kills
  → calls GameManager.WaveComplete() when all enemies in wave are dead
  → spawns Boss prefab when OnBossFight fires

[Player]
  → calls GameManager.PlayerDied() on death
  → calls GameManager.Instance checks state to block input during screens
  → ApplyPowerup() called by PowerupManager

[Enemy]
  → fires static Enemy.OnEnemyDied event when killed

[Boss]
  → calls GameManager.BossDefeated() on death

[BossLaserShooter] (4 children of Boss)
  → fires independently on timers
```

---

## PART 11 — TESTING CHECKLIST

Work through this list after setup:

- [ ] Press Play: Start Screen appears with title and Start button
- [ ] Click Start: Start Screen disappears, enemies begin spawning
- [ ] Kill all wave-1 enemies: Powerup panel appears with two random options
- [ ] Click a powerup: Panel disappears, wave 2 enemies start spawning
- [ ] Kill all wave-2 enemies: Powerup panel appears again
- [ ] Click a powerup: Wave 3 starts
- [ ] Kill all wave-3 enemies: Powerup panel appears for the last time
- [ ] Click a powerup: Boss appears at the top of the screen
- [ ] Boss fires 4 angled lasers and moves side-to-side
- [ ] Destroy the boss: Win screen appears
- [ ] Die at any point: Lose screen appears
- [ ] Restart button reloads the scene cleanly

---

## COMMON ISSUES

**"No enemies spawning"**
→ Check that WaveConfig assets have `Enemy Prefab` and `Path Prefab` assigned.
→ Confirm `EnemySpawner.Wave Configs` list has all 3 configs.

**"Wave never completes"**
→ Make sure `Enemy.OnEnemyDied` fires: enemies must be killed by a `DamageDealer` hit.
→ Verify player laser Layer = `PlayerProjectile` and enemy Layer = `Enemy` and the Physics 2D matrix allows their collision.

**"Powerup panel does not appear"**
→ Check `PowerupPanel` is assigned in the PowerupManager Inspector.
→ Check `PowerupManager` subscribes to `GameManager.OnWaveComplete` — it does so in `Start()`, so GameManager must be in the scene.

**"Boss does not spawn"**
→ Check `Boss Prefab` is assigned on the EnemySpawner.
→ Confirm exactly 3 WaveConfigs are in the list (TotalWaves constant = 3).

**"Player moves on the Start Screen"**
→ Player.Update() checks GameManager.CurrentState. Make sure GameManager is in the scene before Player.

**"TMPro compile errors"**
→ Import TextMeshPro package (see Part 9).

**"Lasers hit wrong targets"**
→ Configure the Physics 2D Layer Collision Matrix (see Part 1).
