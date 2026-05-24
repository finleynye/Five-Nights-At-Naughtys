# Five Nights At Naughtys Project Setup Notes

This setup pass reorganises the Unity project without changing gameplay behaviour, balancing, AI rules, or the horror loop.

## Folder Layout

- `_Project/Art` contains first-party art, images, materials, models, textures, videos, UI fonts, and animations.
- `_Project/Audio` contains existing music, SFX, ambience, mixer, and voice/call assets. Existing subfolders were preserved for reference safety.
- `_Project/Prefabs` contains first-party prefabs and gameplay setup prefabs.
- `_Project/Scenes` contains the project scenes grouped by purpose.
- `_Project/Scripts` contains first-party scripts grouped by responsibility.
- `_Project/Settings` contains render pipeline and project asset settings.
- `_Project/Timeline` contains Timeline assets and death/cutscene animation assets.
- `_Project/Content` contains feature-specific content such as Schmeddit posts and Andy's Adventure.
- `ThirdParty` contains imported packages/assets including TextMesh Pro, Ready Player Me, Plugins, ProBuilder Data, Pixel UI Pack, Dialogue Speaker, VHS assets, Polygon Horror Mansion, Simple Office Interiors, and TutorialInfo.
- `ThirdParty/Ready Player Me/LegacySettings` contains a leftover Ready Player Me settings asset that collided with another `Resources/Settings/CoreSettings.asset` during the move. It was preserved rather than deleted.

## Scenes

- `_Project/Scenes/Menus/MainMenu.unity`: Start here. Main menu and game entry point.
- `_Project/Scenes/Cutscenes/CutScene.unity`: Opening cutscene scene loaded from the menu.
- `_Project/Scenes/Main/MainScene.unity`: Main gameplay/night scene.
- `_Project/Scenes/Deaths/Deaths.unity`: Death sequence scene.
- `_Project/Scenes/Deaths/WinScreen.unity`: Win/end scene.
- `_Project/Scenes/Testing/BS Scenes/New Scene.unity`: Existing test/prototype scene. Kept because purpose is unclear.
- `_Project/Scenes/Testing/BS Scenes/Test.unity`: Existing test/prototype scene. Kept because purpose is unclear.

## Build Settings

Build Settings should include these scenes in this order:

1. `Assets/_Project/Scenes/Menus/MainMenu.unity`
2. `Assets/_Project/Scenes/Cutscenes/CutScene.unity`
3. `Assets/_Project/Scenes/Main/MainScene.unity`
4. `Assets/_Project/Scenes/Deaths/Deaths.unity`
5. `Assets/_Project/Scenes/Deaths/WinScreen.unity`

The current menu scripts still load scenes by name, so the scene asset names were preserved.

## Setup Layer

The new setup scripts live in `_Project/Scripts/Core`:

- `GameBootstrapper`: creates persistent setup managers and applies scene-level defaults.
- `GameStateManager`: tracks coarse scene/game state only.
- `CursorManager`: applies menu/gameplay cursor defaults while allowing feature scripts to temporarily override them.
- `SceneLoader`: thin helper for future UI scene loading.
- `GameSceneReferences`: optional ScriptableObject for documenting expected scene names.

These scripts are intentionally small. Do not move night progression, AI movement, jumpscare timing, PC/phone minigames, or balance logic into the bootstrapper.

## Validation

Use `Tools/Five Nights At Naughtys/Validate Project Setup` in the Unity editor to check:

- Build Settings scene paths.
- Missing script components on `_Project` prefabs.
- Duplicate setup manager instances in open scenes.

## Known Fragile Areas

- `SetingsMenu.cs` currently contains class `SettingsMenu`. The file was not renamed because existing Unity references should be preserved first.
- `TrailerOpen.cs` contains class `VentController`, and `VentController.cs` contains class `TrailerOpen`. These were left as-is for reference safety.
- Ready Player Me has both active `Resources/Settings` assets and preserved `LegacySettings` assets. Review before deleting either settings asset.
- `com.readyplayerme.webview` was removed from `Packages/manifest.json` because Unity was failing to resolve its Git dependency and no first-party scripts currently reference it. Re-add it only if mobile/in-editor avatar creation via WebView becomes an active feature.
- Ready Player Me Core is pinned to `v3.3.0` to avoid pulling `com.unity.cloud.gltfast@6.0.1`, which caused `GltfImportBase` compile errors in this Unity 2022.2 project.
- Some legacy top-level folders may remain empty after the move until Unity refreshes or removes them locally.
- `ProjectSettings/ProjectSettings.asset` still references Unity's old `Assets/Scenes/SampleScene.unity` template default. This appears unrelated to playable scenes and was left untouched.
- `Packages/manifest.json` and `Packages/packages-lock.json` had pre-existing local changes before this setup pass.

## Manual Test Checklist

1. Open the project in Unity 2022.2.5f1.
2. Let Unity reimport moved assets.
3. Open `_Project/Scenes/Menus/MainMenu.unity`.
4. Press Play.
5. Start/load the main gameplay scene.
6. Confirm player and camera input still works.
7. Confirm phone, PC, Schmeddit, and email/minigame UI still opens where already implemented.
8. Confirm flashlight and torch charging still work.
9. Confirm night cycle, stress, heartbeat, jumpscare, death, and win systems initialise.
10. Confirm death, win, menu, and cutscene transitions still work.
11. Run the setup validator from the Tools menu.
12. Confirm there are no Missing Script warnings in the main playable scenes.
