using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Creates persistent setup managers and applies scene-level defaults.
/// Keep gameplay, balancing, and horror-loop logic in feature systems, not here.
/// </summary>
public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper Instance { get; private set; }

    [SerializeField] private GameSceneReferences sceneReferences;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureBootstrapperExists()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject bootstrapperObject = new GameObject(nameof(GameBootstrapper));
        bootstrapperObject.AddComponent<GameBootstrapper>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureManager<GameStateManager>();
        EnsureManager<CursorManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        ApplySceneSetup(SceneManager.GetActiveScene());
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneSetup(scene);
    }

    private void ApplySceneSetup(Scene scene)
    {
        GameStateManager.Instance.SetState(GetStateForScene(scene.name));
        CursorManager.Instance.ApplySceneDefaults(scene);
        ValidateSceneName(scene.name);
    }

    private GameState GetStateForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return GameState.MainMenu;
            case "CutScene":
                return GameState.Cutscene;
            case "MainScene":
                return GameState.Gameplay;
            case "Deaths":
                return GameState.Death;
            case "WinScreen":
                return GameState.Win;
            default:
                return GameState.Testing;
        }
    }

    private void ValidateSceneName(string sceneName)
    {
        if (sceneReferences == null)
        {
            return;
        }

        bool knownScene =
            sceneName == sceneReferences.mainMenuSceneName ||
            sceneName == sceneReferences.cutsceneSceneName ||
            sceneName == sceneReferences.gameplaySceneName ||
            sceneName == sceneReferences.deathSceneName ||
            sceneName == sceneReferences.winSceneName;

        if (!knownScene)
        {
            Debug.LogWarning($"Loaded scene '{sceneName}' is not listed in GameSceneReferences.", this);
        }
    }

    private void EnsureManager<T>() where T : Component
    {
        if (FindObjectOfType<T>() != null)
        {
            return;
        }

        GameObject managerObject = new GameObject(typeof(T).Name);
        managerObject.transform.SetParent(transform);
        managerObject.AddComponent<T>();
    }
}
