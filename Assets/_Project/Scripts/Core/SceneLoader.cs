using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Thin scene loading wrapper for UI buttons and future setup code. It does not own gameplay flow.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("SceneLoader cannot load an empty scene name.", this);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadCutscene()
    {
        SceneManager.LoadScene("CutScene");
    }

    public void LoadDeath()
    {
        SceneManager.LoadScene("Deaths");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
