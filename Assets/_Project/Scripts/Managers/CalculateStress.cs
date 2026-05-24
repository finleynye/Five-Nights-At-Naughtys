using UnityEngine;
using TMPro;

public class CalculateStress : MonoBehaviour
{
    [Header("Phone Display")]
    public static float stress;
    [SerializeField] private TMP_Text stressLevel;
    [SerializeField] private Animator HeartAnim;

    [Header("BPM Limits")]
    [SerializeField] private float minBpm = 65f;
    [SerializeField] private float baseBpm = 80f;
    [SerializeField] private float maxBpm = 170f;

    [Header("Douglas Distance BPM")]
    [SerializeField] private Transform douglasTarget;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float safeDistance = 25f;
    [SerializeField] private float dangerDistance = 5f;
    [SerializeField] private float maxDistanceBpmIncrease = 55f;
    [SerializeField] private AnimationCurve distanceBpmCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Task Stress BPM")]
    [SerializeField] private float failedTaskBpmPenalty = 15f;
    [SerializeField] private float completedTaskBpmRelief = 10f;
    [SerializeField] private float maxTaskStressBpm = 45f;
    [SerializeField] private float taskStressDecayPerSecond = 1f;

    [Header("Smoothing")]
    [SerializeField] private float bpmSmoothingSpeed = 4f;

    [Header("Debug")]
    [SerializeField] private bool debugBpm;
    [SerializeField] private float debugLogInterval = 1f;

    private static CalculateStress instance;
    private static int bpm;
    private float currentBpm;
    private float targetBpm;
    private float currentDouglasDistance;
    private float distanceBpm;
    private float taskStressBpm;
    private float debugTimer;

    private void Awake()
    {
        instance = this;
        stress = Mathf.Clamp(stress, 0f, maxTaskStressBpm);
    }

    private void Start()
    {
        FindDistanceTargetsIfMissing();

        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        currentBpm = Mathf.Clamp(baseBpm + stress, minBpm, maxBpm);
        targetBpm = currentBpm;
        bpm = Mathf.RoundToInt(currentBpm);
    }

    private void Update()
    {
        DecayTaskStress();

        targetBpm = CalculateTargetBpm();
        ApplyBpmSmoothing();
        UpdatePhoneDisplay();
        LogBpmDebugInfo();
    }

    //entry point used by existing tasks and phone apps
    public static void UpdateStress(float toAdd)
    {
        if (instance == null)
        {
            stress = Mathf.Clamp(stress + toAdd, 0f, 100f);
            return;
        }

        if (toAdd >= 0f)
            instance.OnTaskFailed(toAdd);
        else
            instance.OnTaskCompleted(Mathf.Abs(toAdd));
    }

    //entry point used by existing UnityEvents
    public void DecreaseStress(float toDecrease)
    {
        OnTaskCompleted(toDecrease);
    }

    public void OnTaskFailed()
    {
        OnTaskFailed(failedTaskBpmPenalty);
    }

    public void OnTaskFailed(float bpmPenalty)
    {
        //failed tasks add readable pressure, but the total task stress is capped
        stress = Mathf.Clamp(stress + Mathf.Abs(bpmPenalty), 0f, maxTaskStressBpm);
    }

    public void OnTaskCompleted()
    {
        OnTaskCompleted(completedTaskBpmRelief);
    }

    public void OnTaskCompleted(float bpmRelief)
    {
        //completed tasks relieve accumulated stress so BPM can recover
        stress = Mathf.Max(0f, stress - Mathf.Abs(bpmRelief));
    }

    private float CalculateDistanceBpm()
    {
        if (douglasTarget == null || playerTarget == null)
            return 0f;

        currentDouglasDistance = Vector3.Distance(douglasTarget.position, playerTarget.position);

        if (currentDouglasDistance >= safeDistance)
            return 0f;

        //distance is mapped from safe to danger, then shaped by an Inspector curve
        var dangerPercent = Mathf.InverseLerp(safeDistance, dangerDistance, currentDouglasDistance);
        var curvedDanger = distanceBpmCurve.Evaluate(Mathf.Clamp01(dangerPercent));
        return Mathf.Clamp(curvedDanger * maxDistanceBpmIncrease, 0f, maxDistanceBpmIncrease);
    }

    private float CalculateTaskStressBpm()
    {
        return Mathf.Clamp(stress, 0f, maxTaskStressBpm);
    }

    private float CalculateTargetBpm()
    {
        distanceBpm = CalculateDistanceBpm();
        taskStressBpm = CalculateTaskStressBpm();

        //base + Douglas danger + task stress = the readable BPM target
        var calculatedBpm = baseBpm + distanceBpm + taskStressBpm;
        return Mathf.Clamp(calculatedBpm, minBpm, maxBpm);
    }

    private void ApplyBpmSmoothing()
    {
        currentBpm = Mathf.Lerp(currentBpm, targetBpm, Time.deltaTime * bpmSmoothingSpeed);
        currentBpm = Mathf.Clamp(currentBpm, minBpm, maxBpm);
        bpm = Mathf.RoundToInt(currentBpm);
    }

    private void DecayTaskStress()
    {
        if (taskStressDecayPerSecond <= 0f || stress <= 0f)
            return;

        stress = Mathf.Max(0f, stress - taskStressDecayPerSecond * Time.deltaTime);
    }

    private void UpdatePhoneDisplay()
    {
        if (stressLevel != null)
            stressLevel.text = $"{bpm} BPM";

        if (HeartAnim != null)
            HeartAnim.SetInteger("HeartRate", bpm);
    }

    private void FindDistanceTargetsIfMissing()
    {
        if (douglasTarget != null && playerTarget != null)
            return;

        var douglasMove = FindObjectOfType<DouglasMove>();
        if (douglasMove == null)
            return;

        if (douglasTarget == null)
            douglasTarget = douglasMove.transform;

        if (playerTarget == null)
            playerTarget = douglasMove.player;
    }

    private void LogBpmDebugInfo()
    {
        if (!debugBpm)
            return;

        debugTimer -= Time.deltaTime;
        if (debugTimer > 0f)
            return;

        debugTimer = Mathf.Max(0.1f, debugLogInterval);
        Debug.Log($"Phone BPM | Distance: {currentDouglasDistance:F1}, Distance BPM: {distanceBpm:F1}, Task Stress BPM: {taskStressBpm:F1}, Target: {targetBpm:F1}, Current: {currentBpm:F1}");
    }

    private bool ValidateReferences()
    {
        var isValid = true;

        if (stressLevel == null)
        {
            Debug.LogError($"{nameof(CalculateStress)} is missing {nameof(stressLevel)} reference.", this);
            isValid = false;
        }

        if (HeartAnim == null)
        {
            Debug.LogError($"{nameof(CalculateStress)} is missing {nameof(HeartAnim)} reference.", this);
            isValid = false;
        }

        if (douglasTarget == null)
            Debug.LogWarning($"{nameof(CalculateStress)} has no Douglas target. Distance BPM will stay at 0.", this);

        if (playerTarget == null)
            Debug.LogWarning($"{nameof(CalculateStress)} has no player target. Distance BPM will stay at 0.", this);

        return isValid;
    }

    //for andy adventure level
    //time player's run, then if they die do some funky maths on the player time then add that to stress
}   //making player want to finish game as soon as possible
