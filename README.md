# TMPTAS
 The Masters Pupil TAS Tool

## Setup
- Install [MelonLoader](https://melonwiki.xyz/#/?id=automated-installation) (I'd recommend making a duplicate of your Masters Pupil folder to keep a clean and modded build seperate) 
- Grab a release or build manually
- Put built `TMPTAS.dll` in modded The Masters Pupil `/mods/` directory

### Release
Grab the latest [here](https://github.com/ZeppelinGames/TMPTAS/releases)

### Manual build
- Build TMPTAS solution
  - If `Dependancies` are missing (you will have a lot of errors):
    - Open solution explorer
    - Dropdown the solution till you see `Dependancies`
    - Right-click `Dependancies` and select `Add Project Reference` (make sure all old dependancies are cleared)
    - Click browse
    - Add `MelonLoader.dll` from directory `The Master's Pupil\MelonLoader\net6\`
    - Add `Assembly-CSharp.dll` from directory `The Master's Pupil\TheMastersPupil-v1.2_Data\Managed\`
    - Add `Unity.InputSystem` from directory `The Master's Pupil\TheMastersPupil-v1.2_Data\Managed\`
    - Select all Assembiles in directory `The Master's Pupil\TheMastersPupil-v1.2_Data\Managed\` in the namespace `UnityEngine` (starts with UnityEngine.)

## Features/Controls
Show ingame colliders, player movement direction and player ground check.  
Yellow colliders are *triggers* and won't cause the player to collide with them but usually run events.  
Blue colliders are not triggers (there's no physics matrix checking yet, player may or may not collide)

**KeyPad0:** Resume/Pause time  
**KeyPad1:** Set time speed to x0.5  
**KeyPad2:** Set time speed to x0.25  
**KeyPad3:** Set time speed to x0.1  

**KeyPad4:** Quick change scene backwards  
**KeyPad6:** Quick change scene forwards  

**KeyPad9:** Step frame by frame  
