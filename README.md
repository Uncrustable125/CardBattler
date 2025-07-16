# CardBattler
A turn-based deck-building game made in Unity.


## Features
- Enemy waves
- Card effects

## Controls
- Left-click to play cards

✅ TODO / TASKS
- [ ] Refactor and clean up `Enemy`, `Player`, and `Encounter` scripts
- [ ] Remove unused variables and redundant logic
- [ ] Investigate and fix bugs in `TargetingCard.cs`

🐞 KNOWN BUGS
- NullReferenceException in TargetingCard.cs:89
  Cause: Likely unassigned reference in `Update()`
  Status: Needs debugging

🧠 PLANNED FEATURES / IMPROVEMENTS
- Add HP bar system for player and enemies
- Integrate animations for characters and actions
- Display icons for buffs/debuffs
- Implement Trinket mechanics and effects
