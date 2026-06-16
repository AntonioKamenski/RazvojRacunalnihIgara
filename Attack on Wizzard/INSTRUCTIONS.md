# Attack on Wizard — Unity Setup Instructions

---

## Stage 1 — Core Systems

### Step 1.1 — Layers Setup (do this first)
1. Edit > Project Settings > Tags and Layers
2. Add these layers:
   - `Player`
   - `Enemy`
   - `Projectile`
   - `Minimap` (for minimap-only icons)
3. Edit > Project Settings > Physics 2D — disable collisions between:
   - `Projectile` vs `Projectile`
   - `Enemy` vs `Enemy` (optional, prevents enemies pushing each other)

### Step 1.2 — Element & Damage Scripts
> Ask Claude to generate: `ElementType.cs`, `DamageInfo.cs`, `BleedHandler.cs`

### Step 1.3 — Base Enemy Setup
> Ask Claude to generate: `EnemyBase.cs`

1. Create a folder `Assets/Prefabs/Enemies`
2. Create a new 2D Sprite GameObject → name it `Enemy_Grunt`
3. Add components:
   - Rigidbody2D (Gravity Scale = 0, Freeze Rotation Z)
   - Capsule Collider 2D
   - Set Layer to `Enemy`
   - Attach `EnemyBase.cs`
4. Create a world-space Canvas child for the HP bar:
   - Canvas: Render Mode = World Space, scale to ~(0.01, 0.01, 1)
   - Add a Slider inside it → this is the HP bar
   - Add a second Slider for bleed buildup (different colour)
5. Save as a Prefab

### Step 1.4 — Base Player Setup
> Ask Claude to generate: `PlayerBase.cs` (replaces current PlayerController.cs)

1. Assign the Player GameObject to the `Player` layer
2. Attach `PlayerBase.cs`
3. Remove old `PlayerController.cs` component

### Step 1.5 — UI — Player Health Bar
1. In GameScene Canvas, add a Slider → name `PlayerHealthBar`
2. Anchor: top-center or top-left
3. The `PlayerBase.cs` script will reference this slider

---

## Stage 2 — Enemies

### Step 2.1 — Enemy AI
> Ask Claude to generate: `EnemyAI.cs` (melee), `RangedEnemyAI.cs`

1. Add `EnemyAI.cs` to all melee enemy prefabs
2. Add `RangedEnemyAI.cs` to Skeleton Archer, Hex Witch, Spitter prefabs

### Step 2.2 — Create All 10 Enemy Prefabs
For each enemy:
1. Duplicate the base Enemy prefab
2. Assign the enemy's unique sprite
3. In `EnemyBase.cs` Inspector, set:
   - Max HP
   - Move Speed
   - Damage
   - Element Weakness
   - Weakness Multiplier (default 1.5)
   - Resistance Multiplier (default 0.5)
   - Bleed Buildup Threshold
   - Bleed Damage Per Tick
   - Attack Type (Melee / Ranged)
4. Save each as its own Prefab in `Assets/Prefabs/Enemies`

### Step 2.3 — Enemy Spawner
> Ask Claude to generate: `EnemySpawner.cs`

1. Create empty GameObject in GameScene → name `EnemySpawner`
2. Attach `EnemySpawner.cs`
3. Assign enemy prefabs to the spawner's list in Inspector
4. Set spawn radius (should be just outside camera view)

---

## Stage 3 — Player Classes

### Step 3.1 — Class Data & Attacks
> Ask Claude to generate: `PlayerClass.cs`, `WarriorAttack.cs`, `RogueAttack.cs`, `MageAttack.cs`, `ArcherAttack.cs`

1. Create folder `Assets/Prefabs/Projectiles`
2. For Mage magic orb: create Sprite + Circle Collider 2D + Rigidbody2D → save as `MagicOrb` prefab
3. For Archer arrow: create Sprite + Capsule Collider 2D + Rigidbody2D → save as `Arrow` prefab

### Step 3.2 — Boss Preview Screen
**Scripts already generated:** `BossData.cs`, `BossPreviewManager.cs`

**Scene order in Build Settings:** MainMenu → **BossPreview** → ClassSelect → GameScene

1. **Create BossData assets** — one per boss:
   - Right-click Assets > Create > AttackOnWizard > Boss Data
   - Create 4 assets: `BD_InfernalDrake`, `BD_GlacialColossus`, `BD_StormTitan`, `BD_VoidWraith`
   - Fill in each: Boss Name, Description (lore text), Boss Image (a portrait sprite), Weak Element, Boss Prefab

2. **Create the BossPreview scene:**
   - File > New Scene → save as `BossPreview`
   - Add to Build Settings immediately after MainMenu
   - Add an EventSystem + Canvas, then inside the Canvas:

   | Object | Component | Notes |
   |--------|-----------|-------|
   | `BossImage` | Image | Large, center-screen |
   | `BossNameText` | TextMeshProUGUI | Large font, above or below image |
   | `WeakElementText` | TextMeshProUGUI | Script sets colour automatically |
   | `DescriptionText` | TextMeshProUGUI | Smaller font, lore/flavour text |

   The screen auto-advances to ClassSelect after 7 seconds (no button needed).

3. Create empty GameObject → name `BossPreviewManager`, attach `BossPreviewManager.cs`
   - **Bosses** array → drag all 4 BossData assets in
   - Wire all Image/Text/Button references in Inspector
   - **Display Duration** defaults to 7s — adjust in Inspector if needed

4. Ensure the scene has a `Camera` and `EventSystem` (with **InputSystemUIInputModule**)

### Step 3.3 — Class Selection Screen
> Ask Claude to generate: `ClassSelectManager.cs`

1. Create new Scene: `ClassSelect`
2. Add it to Build Settings (after BossPreview, before GameScene)
3. Add 4 buttons (one per class) and a panel showing class stats
4. Modify `GameManager.cs` to store the chosen class and pass it to GameScene

### Step 3.3 — Passive Abilities
> Ask Claude to generate: `PassiveAbilityHandler.cs`

1. Attach to Player — reads chosen class from GameManager
2. Enables the correct passive at runtime

---

## Stage 4 — Upgrades

### Step 4.1 — Upgrade Definitions
> Ask Claude to generate: `UpgradeData.cs` (ScriptableObject), `UpgradeManager.cs`

1. Create folder `Assets/ScriptableObjects/Upgrades`
2. For each of the 15 upgrades, create a ScriptableObject asset:
   - Right-click Assets > Create > AttackOnWizard > Upgrade
3. Fill in: name, description, icon, upgrade type (active/passive), stats

### Step 4.2 — Upgrade UI Panel
1. In GameScene Canvas, create a Panel → name `UpgradePanel` (SetActive OFF by default)
2. Add 3 child buttons for the 3 upgrade choices
3. Each button has: icon (Image), name (TMP text), description (TMP text)
4. Wire references in `UpgradeManager.cs`

### Step 4.3 — Upgrade Prefabs (Active Attacks)
For each active auto-attack upgrade, create a prefab:
- `SpinningBlades` — orbiting sprite with Collider, attached to Player
- `FireAura` — particle system + trigger collider around player
- etc.
> Ask Claude to generate each auto-attack script individually

---

## Stage 5 — Bosses

**Scripts already generated:** `BossHUDController.cs`, `BossBase.cs`, `InfernalDrake.cs`,
`GlacialColossus.cs`, `StormTitan.cs`, `VoidWraith.cs`, `BossArenaPortal.cs`, `InstantAoEDamage.cs`

### Step 5.1 — Boss Prefabs
1. Create folder `Assets/Prefabs/Bosses`
2. For each boss, create a new 2D Sprite GameObject with a large sprite:
   - Add: `Rigidbody2D` (Gravity Scale=0, Freeze Rotation Z), `Polygon Collider 2D` or `Box Collider 2D`
   - Set Layer to `Enemy`
   - Attach the matching boss script (`InfernalDrake`, `GlacialColossus`, etc.)
   - Attach `BleedHandler` (required by EnemyBase)
   - Add a child **World Space Canvas** with two Sliders (HP bar + bleed bar), same as regular enemies
3. In each boss's Inspector, assign its projectile prefab (e.g., a fireball sprite with `EnemyProjectile`)
4. For **InfernalDrake**: also assign `minionPrefab` (a small enemy prefab — any grunt prefab works)
5. Save each as a Prefab in `Assets/Prefabs/Bosses`

### Step 5.2 — Boss HUD UI
1. In GameScene **Canvas**, add a Panel at top-center → name it `BossHealthPanel`
2. Set its default state: **SetActive OFF**
3. Inside the panel:
   - Add a **Slider** (stretched wide, near top) → name `BossHealthSlider`
     - Min Value = 0, Max Value = 1, Interactable OFF
   - Add a **TMP Text** above or inside the slider → name `BossNameText`
4. Create empty GameObject in GameScene → name `BossHUD`
5. Attach `BossHUDController.cs`
6. In Inspector, wire:
   - **Panel** → BossHealthPanel
   - **Health Slider** → BossHealthSlider
   - **Boss Name Text** → BossNameText

### Step 5.3 — Boss Arena Portals
1. In GameScene, pick 4 spots on the map (spread them out — corners or open areas)
2. For each, create an empty GameObject → name e.g. `Portal_InfernalDrake`
3. Add `Box Collider 2D` (Is Trigger = ON), size to ~5×5 units
4. Attach `BossArenaPortal.cs`
5. In Inspector:
   - **Boss Prefab** → the matching boss prefab
   - **Boss Spawn Point** → create a child Transform at the center of the arena, assign it here
   - **Arena Walls** → create 4 thin cube/sprite GameObjects forming a box around the arena area,
     add them to this list (they start disabled, activate on boss spawn)
   - **Portal Visual** → optional sprite/particle that disappears when boss spawns

### Step 5.4 — Enemy Resistances Before Boss (optional)
The `EnemySpawner` can be configured to spawn extra-resistant enemies in the area near each
portal — this is not scripted automatically. To simulate it, create enemy prefab variants with
higher `resistanceMultiplier` and add them to the spawner's weighted list only in the boss zone.

### Step 5.5 — Projectile Prefabs for Bosses
Each boss fires via `EnemyProjectile`. Create matching prefabs in `Assets/Prefabs/Projectiles`:
- `FireballProj` — red/orange sprite, Circle Collider 2D (trigger), Rigidbody2D, `EnemyProjectile`
- `IceSpikeProj` — blue sprite, same setup
- `LightningBoltProj` — yellow sprite (used by StormTitan for AoE ground strike, add `InstantAoEDamage`)
- `ShadowOrbProj` — dark purple sprite, same setup

---

## Stage 6 — Polish

**Scripts already generated:** `GameStatsTracker.cs`, `XPManager.cs`, `XPPickup.cs`,
`DamagePopup.cs`, `GameEndManager.cs`

### Step 6.1 — Damage Popup Prefab
1. Create folder `Assets/Prefabs/UI`
2. Create new empty GameObject → name `DamagePopup`
3. Add component: **TextMeshPro** (the 3D version — found under Component > Mesh > TextMeshPro,
   NOT the UI/UGUI version)
4. Set font size to ~2, alignment Center, no word-wrap
5. Attach `DamagePopup.cs`
6. Save as Prefab in `Assets/Prefabs/UI/DamagePopup`
7. ⚠ Do NOT assign it to every enemy — it goes on the `GameStatsTracker` object (Step 6.4)

### Step 6.2 — XP Pickup Prefab
1. Create new 2D Sprite GameObject → name `XPPickup` (small glowing orb sprite)
2. Add `CircleCollider2D` (Is Trigger ON, radius 0.3)
3. Attach `XPPickup.cs`
4. Save as Prefab in `Assets/Prefabs/UI/XPPickup`
5. Same as above — assign on the `GameStatsTracker` object (Step 6.4)

### Step 6.3 — HUD & XP Bar
1. In GameScene Canvas, add:
   - **Slider** at bottom-center → name `XPSlider` (Min=0, Max=1, no handle, Interactable OFF)
   - **TMP Text** next to it → name `LevelText` (default "Lv 1")
   - **TMP Text** top-left → name `TimerText`
   - **TMP Text** next to it → name `KillCountText`
2. Create empty GameObject → name `XPManager`, attach `XPManager.cs`
   - Wire **XP Slider** → XPSlider, **Level Text** → LevelText
3. Create empty GameObject → name `GameStatsTracker`, attach `GameStatsTracker.cs`
   - Wire **Timer Text** → TimerText, **Kill Count Text** → KillCountText
   - Wire **Damage Popup Prefab** → the DamagePopup prefab
   - Wire **XP Pickup Prefab** → the XPPickup prefab

### Step 6.4 — Defeat Text & Victory Screen
1. **Defeat text** — in Canvas, add a TMP text:
   - Name it `DefeatText`, text: **"DEFEAT"**
   - Large font, centered on screen, colour white (or red)
   - Set alpha to **0** in the Color field (fully transparent — the script fades it in)
   - No panel needed — just the raw text object

2. **Victory notification** — in Canvas, create a Panel (`victoryPanel`, **SetActive OFF**):
   - Add a TMP text inside it → name `victoryText`
   - Auto-closes after 4 seconds, gameplay continues

3. Create empty GameObject → name `GameEndManager`, attach `GameEndManager.cs`
   - **Defeat Text** → DefeatText
   - **Victory Panel** → victoryPanel
   - **Victory Text** → victoryText
   - Fade In Duration / Hold Duration can be tuned in Inspector (defaults: 1.5s / 7s)

---

## Script Generation Order (recommended)

| Order | Script | Depends On |
|-------|--------|------------|
| 1 | `ElementType.cs` | Nothing |
| 2 | `DamageInfo.cs` | ElementType |
| 3 | `BleedHandler.cs` | DamageInfo |
| 4 | `EnemyBase.cs` | DamageInfo, BleedHandler |
| 5 | `EnemyAI.cs` | EnemyBase |
| 6 | `RangedEnemyAI.cs` | EnemyBase |
| 7 | `EnemySpawner.cs` | EnemyBase |
| 8 | `PlayerBase.cs` | DamageInfo |
| 9 | `PlayerClass.cs` | PlayerBase |
| 10 | `PassiveAbilityHandler.cs` | PlayerClass |
| 11 | `WarriorAttack.cs` | DamageInfo |
| 12 | `RogueAttack.cs` | DamageInfo |
| 13 | `MageAttack.cs` | DamageInfo |
| 14 | `ArcherAttack.cs` | DamageInfo |
| 15 | `UpgradeData.cs` | Nothing |
| 16 | `UpgradeManager.cs` | UpgradeData, PlayerBase |
| 17 | `BossBase.cs` | DamageInfo, BleedHandler |
| 18 | Boss scripts (×4) | BossBase |
| 19 | `DamagePopup.cs` | Nothing |
| 20 | `XPManager.cs` | Nothing |
| 21 | `GameEndManager.cs` | GameManager |
