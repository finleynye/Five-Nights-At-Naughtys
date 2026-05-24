using UnityEngine;

public class CameraPolishController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform polishPivot;

    [Header("Idle Bob")]
    [SerializeField] private bool useIdleBob = true;
    [SerializeField] private float idleBobSpeed = 1.1f;
    [SerializeField] private float idleBobAmount = 0.008f;

    [Header("Idle Rotation Sway")]
    [SerializeField] private bool useIdleRotationSway = true;
    [SerializeField] private float idleRotationSwaySpeed = 1.15f;
    [SerializeField] private float idleYawAmount = 0.2f;
    [SerializeField] private float idlePitchAmount = 0.1f;
    [SerializeField] private float idleRollAmount = 0.14f;

    [Header("Transition Sway")]
    [SerializeField] private bool useTransitionSway = true;
    [SerializeField] private float swayDuration = 0.25f;
    [SerializeField] private float swayPositionAmount = 0.02f;
    [SerializeField] private AnimationCurve swayCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    [Header("Transition Rotation Sway")]
    [SerializeField] private bool useTransitionRotationSway = true;
    [SerializeField] private float transitionRotationAmount = 1.0f;
    [SerializeField] private float transitionRotationDuration = 0.25f;
    [SerializeField] private AnimationCurve transitionRotationCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    [Header("Look Lean")]
    [SerializeField] private bool useLookLean = true;
    [SerializeField] private float lookLeanAmount = 1f;
    [SerializeField] private float lookLeanSensitivity = 2.5f;
    [SerializeField] private float lookLeanLerpSpeed = 12f;
    [SerializeField] private bool disableLookLeanWhenPcIsOn = true;
    [SerializeField] private bool invertLookLean;

    [Header("FOV")]
    [SerializeField] private bool useFovKick = true;
    [SerializeField] private float defaultFov = 60f;
    [SerializeField] private float transitionFovIncrease = 2f;
    [SerializeField] private float fovReturnSpeed = 6f;

    [Header("State")]
    [SerializeField] private bool allowEffectsWhenNightInactive;
    [SerializeField] private bool reduceEffectsWhenPcIsOn = true;
    [SerializeField] private bool debugCameraTransforms;

    private Vector3 lastAppliedPosition;
    private Vector3 baseLocalPosition;
    private Quaternion baseLocalRotation = Quaternion.identity;
    private Vector3 smoothedPositionOffset;
    private Quaternion smoothedRotationOffset = Quaternion.identity;

    private CameraMover.CameraDirection currentSwayDirection = CameraMover.CameraDirection.None;
    private float swayTimer;
    private CameraMover.CameraDirection currentRotationSwayDirection = CameraMover.CameraDirection.None;
    private float rotationSwayTimer;
    private float currentLookLean;
    private LerpCam existingFovZoom;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();

        if (polishPivot == null)
            polishPivot = transform;

        existingFovZoom = GetComponent<LerpCam>();

        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        if (defaultFov <= 0f)
            defaultFov = targetCamera.fieldOfView;

        baseLocalPosition = polishPivot.localPosition;
        baseLocalRotation = polishPivot.localRotation;

        if (swayCurve == null || swayCurve.length == 0)
        {
            swayCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0f)
            );
        }

        if (transitionRotationCurve == null || transitionRotationCurve.length == 0)
        {
            transitionRotationCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0f)
            );
        }

        LogDebugTransformWarnings();
    }

    private void LateUpdate()
    {
        var effectStrength = GetEffectStrength();
        var transitionStrength = GetTransitionStrength();
        var transitionRotationStrength = GetTransitionRotationStrength();
        var positionOffset = Vector3.zero;
        var rotationOffset = Quaternion.identity;

        if (effectStrength > 0f)
        {
            positionOffset += CalculateIdleBobPosition(effectStrength);
            rotationOffset *= CalculateIdleRotationSway(effectStrength);
            positionOffset += CalculateSwayPosition(transitionStrength * effectStrength);
            rotationOffset *= CalculateTransitionRotationSway(transitionRotationStrength * effectStrength);
            rotationOffset *= CalculateLookLeanRotation(effectStrength);
        }

        ApplyOffsetSmoothly(positionOffset, rotationOffset);
        UpdateFov(transitionStrength * effectStrength);
    }

    public void PlayTransitionSway(CameraMover.CameraDirection direction)
    {
        if (!useTransitionSway || direction == CameraMover.CameraDirection.None)
            return;

        currentSwayDirection = direction;
        swayTimer = Mathf.Max(0.01f, swayDuration);

        if (useTransitionRotationSway)
        {
            currentRotationSwayDirection = direction;
            rotationSwayTimer = Mathf.Max(0.01f, transitionRotationDuration);
        }
    }

    private float GetEffectStrength()
    {
        if (!allowEffectsWhenNightInactive && !NightCycle.isNightActive)
            return 0f;

        if (PC.isOn)
            return reduceEffectsWhenPcIsOn ? 0.25f : 0f;

        return 1f;
    }

    private float GetTransitionStrength()
    {
        if (swayTimer <= 0f)
            return 0f;

        swayTimer -= Time.deltaTime;

        var normalizedTime = 1f - Mathf.Clamp01(swayTimer / Mathf.Max(0.01f, swayDuration));
        return swayCurve.Evaluate(normalizedTime);
    }

    private float GetTransitionRotationStrength()
    {
        if (rotationSwayTimer <= 0f)
            return 0f;

        rotationSwayTimer -= Time.deltaTime;

        var normalizedTime = 1f - Mathf.Clamp01(rotationSwayTimer / Mathf.Max(0.01f, transitionRotationDuration));
        return transitionRotationCurve.Evaluate(normalizedTime);
    }

    private Vector3 CalculateIdleBobPosition(float strength)
    {
        if (!useIdleBob)
            return Vector3.zero;

        var bobTime = Time.time * idleBobSpeed;
        var y = Mathf.Sin(bobTime) * idleBobAmount;
        var x = Mathf.Cos(bobTime * 0.65f) * idleBobAmount * 0.35f;
        return new Vector3(x, y, 0f) * strength;
    }

    private Quaternion CalculateIdleRotationSway(float strength)
    {
        if (!useIdleRotationSway)
            return Quaternion.identity;

        var swayTime = Time.time * idleRotationSwaySpeed;
        var yaw = Mathf.Sin(swayTime) * idleYawAmount;
        var pitch = Mathf.Sin(swayTime * 0.75f) * idlePitchAmount;
        var roll = Mathf.Cos(swayTime * 0.9f) * idleRollAmount;
        return Quaternion.Euler(pitch * strength, yaw * strength, roll * strength);
    }

    private Vector3 CalculateSwayPosition(float strength)
    {
        if (!useTransitionSway || strength <= 0f)
            return Vector3.zero;

        return GetDirectionPosition(currentSwayDirection) * swayPositionAmount * strength;
    }

    private Quaternion CalculateTransitionRotationSway(float strength)
    {
        if (!useTransitionRotationSway || strength <= 0f)
            return Quaternion.identity;

        return Quaternion.Euler(GetDirectionRotation(currentRotationSwayDirection) * transitionRotationAmount * strength);
    }

    private Quaternion CalculateLookLeanRotation(float strength)
    {
        var targetLean = 0f;

        if (useLookLean && (!disableLookLeanWhenPcIsOn || !PC.isOn))
        {
            var mouseX = Input.GetAxisRaw("Mouse X");
            targetLean = Mathf.Clamp(
                -mouseX * lookLeanSensitivity,
                -lookLeanAmount,
                lookLeanAmount
            );

            if (invertLookLean)
                targetLean *= -1f;

            targetLean *= strength;
        }

        currentLookLean = Mathf.Lerp(
            currentLookLean,
            targetLean,
            Time.deltaTime * lookLeanLerpSpeed
        );

        return Quaternion.Euler(0f, 0f, currentLookLean);
    }

    private Vector3 GetDirectionPosition(CameraMover.CameraDirection direction)
    {
        switch (direction)
        {
            case CameraMover.CameraDirection.Forward:
                return new Vector3(0f, -0.35f, 1f);
            case CameraMover.CameraDirection.Back:
                return new Vector3(0f, 0.25f, -1f);
            case CameraMover.CameraDirection.Left:
                return new Vector3(-1f, 0f, 0f);
            case CameraMover.CameraDirection.Right:
                return new Vector3(1f, 0f, 0f);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetDirectionRotation(CameraMover.CameraDirection direction)
    {
        switch (direction)
        {
            case CameraMover.CameraDirection.Forward:
                return new Vector3(-1f, 0f, 0f);
            case CameraMover.CameraDirection.Back:
                return new Vector3(1f, 0f, 0f);
            case CameraMover.CameraDirection.Left:
                return new Vector3(0f, -0.4f, 1f);
            case CameraMover.CameraDirection.Right:
                return new Vector3(0f, 0.4f, -1f);
            default:
                return Vector3.zero;
        }
    }

    private void ApplyOffsetSmoothly(Vector3 positionOffset, Quaternion rotationOffset)
    {
        smoothedPositionOffset = Vector3.Lerp(
            smoothedPositionOffset,
            positionOffset,
            Time.deltaTime * 10f
        );

        smoothedRotationOffset = Quaternion.Slerp(
            smoothedRotationOffset,
            rotationOffset,
            Time.deltaTime * 10f
        );

        polishPivot.localPosition = baseLocalPosition + smoothedPositionOffset;
        polishPivot.localRotation = baseLocalRotation * smoothedRotationOffset;

        lastAppliedPosition = polishPivot.localPosition;
    }

    private void UpdateFov(float transitionStrength)
    {
        if (!useFovKick || targetCamera == null)
            return;

        if (existingFovZoom != null && (existingFovZoom.zoomIn || existingFovZoom.zoomOut || Input.GetMouseButton(1)))
            return;

        var targetFov = defaultFov + transitionFovIncrease * transitionStrength;
        targetCamera.fieldOfView = Mathf.Lerp(
            targetCamera.fieldOfView,
            targetFov,
            Time.deltaTime * fovReturnSpeed
        );
    }

    private bool ValidateReferences()
    {
        var isValid = true;

        if (targetCamera == null)
        {
            Debug.LogWarning($"{nameof(CameraPolishController)} is missing {nameof(targetCamera)} reference.", this);
            isValid = false;
        }

        if (polishPivot == null)
        {
            Debug.LogWarning($"{nameof(CameraPolishController)} is missing {nameof(polishPivot)} reference.", this);
            isValid = false;
        }

        return isValid;
    }

    private void LogDebugTransformWarnings()
    {
        if (!debugCameraTransforms)
            return;

        if (targetCamera != null && polishPivot == targetCamera.transform)
            Debug.LogWarning($"{nameof(CameraPolishController)} is writing to the same transform as {nameof(targetCamera)}. Use a dedicated polish pivot child if camera look feels choppy.", this);
    }
}
