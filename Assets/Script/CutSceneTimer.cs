using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneTimer : MonoBehaviour
{
    public float seconds;
    public string sceneName;
    void Update()
    {
        StartCoroutine(WaitforEndCut());

    }

    public IEnumerator WaitforEndCut()
    {
        yield return new WaitForSeconds(seconds);

        NightCycle.isNightActive = true;
        SceneManager.LoadScene(sceneName);
    }

    public void OverrideWait()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Win()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
