using UnityEngine;

public enum GameState
{
    Unknown,
    MainMenu,
    Cutscene,
    Gameplay,
    Death,
    Win,
    Testing
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Unknown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState state)
    {
        CurrentState = state;
    }
}
