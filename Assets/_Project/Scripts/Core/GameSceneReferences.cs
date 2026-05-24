using UnityEngine;

/// <summary>
/// Central names for scenes that are loaded from code. Keep this aligned with Build Settings.
/// </summary>
[CreateAssetMenu(fileName = "GameSceneReferences", menuName = "Five Nights At Naughtys/Game Scene References")]
public class GameSceneReferences : ScriptableObject
{
    [Header("Build Scene Names")]
    public string mainMenuSceneName = "MainMenu";
    public string cutsceneSceneName = "CutScene";
    public string gameplaySceneName = "MainScene";
    public string deathSceneName = "Deaths";
    public string winSceneName = "WinScreen";
}
