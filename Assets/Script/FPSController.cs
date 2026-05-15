using TMPro;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private float gameTimer;
    [SerializeField] private float framerate;
    [SerializeField] private TMP_Text displayText;
   
    private void Update()
    {


        if (gameTimer > 1)
        {
            framerate = (int)(1 / Time.unscaledDeltaTime);
            gameTimer = 0;
        }
        else gameTimer += Time.deltaTime;

        displayText.text = $"FPS: {framerate}";
    }
}
