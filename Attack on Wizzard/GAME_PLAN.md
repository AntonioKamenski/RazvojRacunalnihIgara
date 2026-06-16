# Attack on Wizard — Game Plan & Progress Tracker

---

## Element System

| Element   | Player Class Bonus | Boss Weak To |
|-----------|--------------------|--------------|
| Fire      | Warrior            | Ice Boss     |
| Ice       | Mage               | Fire Boss    |
| Lightning | Archer             | Shadow Boss  |
| Shadow    | Rogue              | Lightning Boss |
| Bleed     | Universal (no Mage)| All enemies  |

---

## Stage 1 — Core Systems
- [x] Element enum — `ElementType.cs`
- [x] Damage calculation system — `DamageInfo.cs`, `BleedHandler.cs`
- [x] Base Enemy class — `EnemyBase.cs`
- [x] Base Player class — `PlayerBase.cs` (replaces PlayerController.cs on Player)
- [ ] Health bar UI — Slider in Canvas (Unity setup, see INSTRUCTIONS.md 1.3 / 1.5)
- [ ] Bleed buildup UI bar — Slider in world-space Canvas on enemy prefab (Unity setup, see INSTRUCTIONS.md 1.3)

---

## Stage 2 — Enemy System (10 Enemies)

### Melee Enemies (7)
- [ ] **Grunt** — basic chase, swipe attack | Weak: Fire
- [ ] **Armored Knight** — slow, high HP, shield blocks frontal hits | Weak: Lightning
- [ ] **Berserker** — fast charge attack in a straight line | Weak: Ice
- [ ] **Leaper** — jumps at the player, AoE land slam | Weak: Shadow
- [ ] **Brute** — slow AoE ground pound | Weak: Fire
- [ ] **Swarm Spawner** — spawns 4 small minions that chase player | Weak: Lightning
- [ ] **Phantom** — teleports near the player before striking | Weak: Ice

### Ranged Enemies (3)
- [ ] **Skeleton Archer** — fires arrows at player from distance | Weak: Fire
- [ ] **Hex Witch** — fires tracking magic bolt | Weak: Lightning
- [ ] **Spitter** — spits poison projectiles in a spread | Weak: Ice

### Enemy AI
- [x] Melee tracking — `EnemyAI.cs`
- [x] Ranged maintains distance, fires projectile — `RangedEnemyAI.cs`
- [x] Enemy projectile — `EnemyProjectile.cs`
- [x] Bleed buildup counter (no decay) — `BleedHandler.cs`
- [ ] Pre-boss wave resistant enemies (Stage 5)

### Enemy Spawner
- [x] Spawns outside camera view at map edges — `EnemySpawner.cs`
- [x] Wave system with increasing difficulty
- [ ] Pre-boss wave override (Stage 5)

---

## Stage 3 — Player Classes (4)

- [x] **Warrior** — `WarriorAttack.cs` (AoE swing, Fire, Fortify passive)
- [x] **Rogue** — `RogueAttack.cs` (dual slash, Shadow, Evasion passive)
- [x] **Mage** — `MageAttack.cs` (homing orb, Ice, Chill passive, no bleed)
- [x] **Archer** — `ArcherAttack.cs` (arrow, Lightning, Swiftness passive)
- [x] Passive ability system — `PassiveAbilityHandler.cs`
- [x] Player projectile — `PlayerProjectile.cs`
- [x] Class selection screen — `ClassSelectManager.cs` + ClassSelect scene (Unity setup needed)

---

## Stage 4 — Upgrades (15 Total)

### Auto-Attack Upgrades (8)
- [x] **Arrow Volley** — `ArrowVolleyUpgrade.cs`
- [x] **Spinning Blades** — `SpinningBladesUpgrade.cs`
- [x] **Chain Lightning** — `ChainLightningUpgrade.cs`
- [x] **Fire Aura** — `FireAuraUpgrade.cs`
- [x] **Frost Nova** — `FrostNovaUpgrade.cs`
- [x] **Shadow Daggers** — `ShadowDaggersUpgrade.cs`
- [x] **Poison Cloud** — `PoisonCloudUpgrade.cs` + `PoisonCloudEffect.cs`
- [x] **Boomerang** — `BoomerangUpgrade.cs` + `BoomerangProjectile.cs`

### Passive Upgrades (7)
- [x] Iron Skin, Sharpness, Resilience, Swiftness, Rapidfire, BleedEdge — applied in `UpgradeManager.cs`
- [ ] **Magnetism** — requires XP pickup system (Stage 6)

### Upgrade System
- [x] Timer-based (every 60s), 3 random choices — `UpgradeManager.cs`
- [x] Stacking (LevelUp on existing components)
- [x] Pauses game during selection
- [x] UI — `UpgradeOptionUI.cs` + `UpgradeDefinition.cs` ScriptableObject
- [ ] Unity setup: create 15 UpgradeDefinition assets + upgrade panel UI

---

## Stage 5 — Bosses (4)

- [x] **The Infernal Drake** (Fire Boss) — `InfernalDrake.cs`
  - Weak: Ice | Resistant: Fire
  - Attacks: fire breath (cone), fire nova, summons fire minions (phase 2)
  - Bleed: Yes

- [x] **The Glacial Colossus** (Ice Boss) — `GlacialColossus.cs`
  - Weak: Fire | Resistant: Ice
  - Attacks: ice spike cone, freeze beam (slows player + damages)
  - Bleed: Yes

- [x] **The Storm Titan** (Lightning Boss) — `StormTitan.cs`
  - Weak: Shadow | Resistant: Lightning
  - Attacks: lightning strike (telegraph + AoE), chain bolt, electrified ground zone
  - Bleed: Yes

- [x] **The Void Wraith** (Shadow Boss) — `VoidWraith.cs`
  - Weak: Lightning | Resistant: Shadow
  - Attacks: shadow orb cone, teleport behind player, shadow clone (phase 2)
  - Bleed: Yes

### Boss Systems
- [x] Boss health bar (large, at top of screen) — `BossHUDController.cs`
- [x] Boss phase transitions (behaviour changes at 50% HP) — `BossBase.cs`
- [x] Boss arena trigger (enter zone → boss spawns, exits locked) — `BossArenaPortal.cs`
- [ ] Boss death reward / scene transition (Stage 6 — XP grant wired in BossArenaPortal)
- [ ] Unity setup: boss prefabs, HUD wiring, arena portals in GameScene (see INSTRUCTIONS.md Step 5.1–5.5)

---

## Stage 6 — Polish & UI

- [x] XP bar and level-up system — `XPManager.cs`
- [x] XP pickups dropped by enemies — `XPPickup.cs`
- [x] Timer display + kill counter — `GameStatsTracker.cs`
- [x] Element damage indicators (floating numbers, colour-coded) — `DamagePopup.cs`
- [x] Death screen (time survived, kills, damage dealt) — `GameEndManager.cs`
- [x] Victory notification per boss kill — `GameEndManager.cs` + `BossBase.cs`
- [x] Magnetism upgrade wired to XPManager — `UpgradeManager.cs`
- [ ] Bleed proc visual effect on enemy (particle effect — Unity setup)
- [ ] Settings: volume, resolution
- [ ] Unity setup: all UI panels, XPPickup prefab, DamagePopup prefab (see INSTRUCTIONS.md Stage 6)

---

## Current Status

**Active Stage:** Stage 5 scripts complete — Unity setup needed, then Stage 6
**Last Updated:** 2026-06-08
