using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ValidateProjectSetup
{
    [MenuItem("Tools/Five Nights At Naughtys/Validate Project Setup")]
    public static void Validate()
    {
        ValidateBuildSettings();
        ValidatePrefabs();
        ValidateOpenSceneManagers();
        Debug.Log("Project setup validation complete. Check the Console for warnings and errors.");
    }

    private static void ValidateBuildSettings()
    {
        string[] requiredScenes =
        {
            "Assets/_Project/Scenes/Menus/MainMenu.unity",
            "Assets/_Project/Scenes/Cutscenes/CutScene.unity",
            "Assets/_Project/Scenes/Main/MainScene.unity",
            "Assets/_Project/Scenes/Deaths/Deaths.unity",
            "Assets/_Project/Scenes/Deaths/WinScreen.unity"
        };

        HashSet<string> buildScenes = new HashSet<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            buildScenes.Add(scene.path);
            if (!AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path))
            {
                Debug.LogError($"Build Settings scene path is missing: {scene.path}");
            }
        }

        foreach (string requiredScene in requiredScenes)
        {
            if (!buildScenes.Contains(requiredScene))
            {
                Debug.LogWarning($"Required scene is not in Build Settings: {requiredScene}");
            }
        }
    }

    private static void ValidatePrefabs()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Project" });
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            int missingScriptCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(prefab);

            if (missingScriptCount > 0)
            {
                Debug.LogError($"Prefab has {missingScriptCount} missing script component(s): {path}", prefab);
            }
        }
    }

    private static void ValidateOpenSceneManagers()
    {
        ReportDuplicateManagers<GameBootstrapper>();
        ReportDuplicateManagers<GameStateManager>();
        ReportDuplicateManagers<CursorManager>();
    }

    private static void ReportDuplicateManagers<T>() where T : Object
    {
        T[] managers = Object.FindObjectsOfType<T>();
        if (managers.Length > 1)
        {
            Debug.LogWarning($"Found {managers.Length} active {typeof(T).Name} instances in open scenes.");
        }
    }
}
