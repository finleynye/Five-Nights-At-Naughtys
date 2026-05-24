using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Owns scene-level cursor defaults. Feature scripts may still temporarily override cursor state for UI.
/// </summary>
public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

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

    public void ApplySceneDefaults(Scene scene)
    {
        switch (scene.name)
        {
            case "MainScene":
                SetGameplayCursor();
                break;
            case "MainMenu":
            case "Deaths":
            case "WinScreen":
                SetMenuCursor();
                break;
            default:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    public void SetGameplayCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetMenuCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
