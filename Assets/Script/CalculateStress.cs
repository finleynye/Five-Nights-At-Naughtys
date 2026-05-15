using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalculateStress : MonoBehaviour
{
    [Header("Stress Management")]
    public static float stress;
    [SerializeField] private TMP_Text stressLevel;
    [SerializeField] private Animator HeartAnim;

    private static int bpm;

    private void Start()
      => bpm = (int)stress + 60 + Random.Range(1, 5);
    

    private void Update()
    {
        stressLevel.text = $"{bpm} BPM";
        HeartAnim.SetInteger("HeartRate", bpm);
    }

    public static void UpdateStress(float toAdd)
    {
        stress += toAdd;
        bpm = (int)stress + 60 + Random.Range(1, 5);
        stress = Mathf.Clamp(stress, 0, 100);
    }


    public void DecreaseStress(float toDecrease)
    {
        stress -= toDecrease;
        bpm = (int)stress + 60 + Random.Range(1, 5);
        stress = Mathf.Clamp(stress, 0, 100);
    }



    //for andy adventure level
    //time player's run, then if they die do some funky maths on the player time then add that to stress
}   //making player want to finish game as soon as possible