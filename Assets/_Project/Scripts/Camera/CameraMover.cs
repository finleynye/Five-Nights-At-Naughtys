using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public enum CameraDirection
    {
        None,
        Forward,
        Back,
        Left,
        Right
    }

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private CameraPolishController cameraPolishController;
    [SerializeField] private float leftTransitionLookLockTime = 0.2f;

    private CameraDirection currentDirection = CameraDirection.Forward;
    private float cameraInputLockTimer;

    public bool isBack;
    public Transform player;
    public PlayerLook pluhLook;
    public GameObject instructions;

    [Header("Audio")]
    public AudioSource chairRoll;
    [SerializeField] private float chairRollCrossfadeTime = 0.12f;
    [SerializeField] private Vector2 chairRollPitchRange = new Vector2(0.94f, 1.06f);

    private AudioSource chairRollFadeSource;
    private AudioSource activeChairRollSource;
    private AudioSource inactiveChairRollSource;
    private Coroutine chairRollFadeRoutine;
    private float chairRollDefaultVolume = 1f;

    private void Start()
    {
        if (cameraPolishController == null)
            cameraPolishController = GetComponentInChildren<CameraPolishController>();

        currentDirection = GetAnimatorDirection();
        SetupChairRollAudio();

        if (SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);
    }

    private void Update()
    {
        if (PC.isOn)
            return;

        if (!NightCycle.isNightActive)
            return;

        if (cameraInputLockTimer > 0f)
        {
            cameraInputLockTimer -= Time.deltaTime;
            return;
        }

        CameraDirection direction = GetInputDirection();

        if (direction != CameraDirection.None)
        {
            CompleteTutorialPrompt();
            ApplyCameraDirection(GetNextCameraDirection(direction));
        }
    }

    private CameraDirection GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W))
            return CameraDirection.Forward;

        if (Input.GetKeyDown(KeyCode.A))
            return CameraDirection.Left;

        if (Input.GetKeyDown(KeyCode.S))
            return CameraDirection.Back;

        if (Input.GetKeyDown(KeyCode.D))
            return CameraDirection.Right;

        return CameraDirection.None;
    }

    private CameraDirection GetNextCameraDirection(CameraDirection requestedDirection)
    {
        if (requestedDirection == currentDirection)
            return CameraDirection.None;

        if (IsOppositeLedgeDirection(requestedDirection))
            return CameraDirection.Forward;

        return requestedDirection;
    }

    private bool IsOppositeLedgeDirection(CameraDirection requestedDirection)
    {
        return currentDirection == CameraDirection.Right && requestedDirection == CameraDirection.Left
            || currentDirection == CameraDirection.Left && requestedDirection == CameraDirection.Right;
    }

    private void ApplyCameraDirection(CameraDirection direction)
    {
        if (direction == CameraDirection.None)
            return;

        var previousDirection = currentDirection;
        ResetAnimatorBools();
        cameraPolishController?.PlayTransitionSway(direction);

        if (previousDirection == CameraDirection.Left || direction == CameraDirection.Left)
            cameraInputLockTimer = leftTransitionLookLockTime;

        switch (direction)
        {
            case CameraDirection.Forward:
                anim.SetBool("Idle", true);
                isBack = false;
                PlayCameraMoveSound(direction);

                if (pluhLook != null)
                {
                    pluhLook.SetCameraRestriction(true, false, false, false);
                    if (previousDirection == CameraDirection.Left)
                        pluhLook.BeginCameraTransition(leftTransitionLookLockTime);
                }

                break;

            case CameraDirection.Left:
                anim.SetBool("Left", true);
                isBack = false;
                StopCameraMoveSound();

                if (pluhLook != null)
                {
                    pluhLook.SetCameraRestriction(false, false, true, false);
                    pluhLook.BeginCameraTransition(leftTransitionLookLockTime);
                }

                break;

            case CameraDirection.Right:
                anim.SetBool("Right", true);
                isBack = false;
                PlayCameraMoveSound(direction);
                if (pluhLook != null)
                    pluhLook.SetCameraRestriction(false, false, false, true);

                break;

            case CameraDirection.Back:
                anim.SetBool("Back", true);
                isBack = true;
                PlayCameraMoveSound(direction);
                if (pluhLook != null)
                    pluhLook.SetCameraRestriction(false, true, false, false);

                break;
        }

        currentDirection = direction;
    }

    private void SetupChairRollAudio()
    {
        if (chairRoll == null)
            return;

        chairRollDefaultVolume = chairRoll.volume;
        chairRollFadeSource = gameObject.AddComponent<AudioSource>();
        CopyAudioSourceSettings(chairRoll, chairRollFadeSource);
        chairRollFadeSource.volume = 0f;

        activeChairRollSource = chairRoll;
        inactiveChairRollSource = chairRollFadeSource;
    }

    private void CopyAudioSourceSettings(AudioSource source, AudioSource destination)
    {
        destination.clip = source.clip;
        destination.outputAudioMixerGroup = source.outputAudioMixerGroup;
        destination.playOnAwake = false;
        destination.loop = source.loop;
        destination.priority = source.priority;
        destination.pitch = source.pitch;
        destination.panStereo = source.panStereo;
        destination.spatialBlend = source.spatialBlend;
        destination.reverbZoneMix = source.reverbZoneMix;
        destination.dopplerLevel = source.dopplerLevel;
        destination.spread = source.spread;
        destination.rolloffMode = source.rolloffMode;
        destination.minDistance = source.minDistance;
        destination.maxDistance = source.maxDistance;
    }

    private void PlayCameraMoveSound(CameraDirection direction)
    {
        if (direction == CameraDirection.Left || chairRoll == null)
            return;

        if (chairRollFadeSource == null)
        {
            chairRoll.pitch = GetRandomChairRollPitch();
            chairRoll.Play();
            return;
        }

        if (chairRollFadeRoutine != null)
            StopCoroutine(chairRollFadeRoutine);

        inactiveChairRollSource.pitch = GetRandomChairRollPitch();
        inactiveChairRollSource.volume = 0f;
        inactiveChairRollSource.Play();

        var sourceToFadeOut = activeChairRollSource != null && activeChairRollSource.isPlaying
            ? activeChairRollSource
            : null;

        chairRollFadeRoutine = StartCoroutine(CrossfadeChairRoll(sourceToFadeOut, inactiveChairRollSource));

        var previousActiveSource = activeChairRollSource;
        activeChairRollSource = inactiveChairRollSource;
        inactiveChairRollSource = previousActiveSource;
    }

    private void StopCameraMoveSound()
    {
        if (chairRollFadeRoutine != null)
            StopCoroutine(chairRollFadeRoutine);

        var sourceToFadeOut = activeChairRollSource != null && activeChairRollSource.isPlaying
            ? activeChairRollSource
            : null;

        if (sourceToFadeOut == null)
        {
            chairRollFadeRoutine = null;
            return;
        }

        chairRollFadeRoutine = StartCoroutine(FadeOutChairRoll(sourceToFadeOut));
    }

    private IEnumerator CrossfadeChairRoll(AudioSource sourceToFadeOut, AudioSource sourceToFadeIn)
    {
        var elapsedTime = 0f;
        var fadeDuration = Mathf.Max(0.01f, chairRollCrossfadeTime);
        var fadeOutStartVolume = sourceToFadeOut != null ? sourceToFadeOut.volume : 0f;

        while (elapsedTime < fadeDuration)
        {
            var fadeAmount = elapsedTime / fadeDuration;

            if (sourceToFadeOut != null)
                sourceToFadeOut.volume = Mathf.Lerp(fadeOutStartVolume, 0f, fadeAmount);

            sourceToFadeIn.volume = Mathf.Lerp(0f, chairRollDefaultVolume, fadeAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (sourceToFadeOut != null)
        {
            sourceToFadeOut.Stop();
            sourceToFadeOut.volume = chairRollDefaultVolume;
        }

        sourceToFadeIn.volume = chairRollDefaultVolume;
        chairRollFadeRoutine = null;
    }

    private IEnumerator FadeOutChairRoll(AudioSource sourceToFadeOut)
    {
        var elapsedTime = 0f;
        var fadeDuration = Mathf.Max(0.01f, chairRollCrossfadeTime);
        var fadeOutStartVolume = sourceToFadeOut.volume;

        while (elapsedTime < fadeDuration)
        {
            sourceToFadeOut.volume = Mathf.Lerp(fadeOutStartVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sourceToFadeOut.Stop();
        sourceToFadeOut.volume = chairRollDefaultVolume;
        chairRollFadeRoutine = null;
    }

    private float GetRandomChairRollPitch()
    {
        return Random.Range(chairRollPitchRange.x, chairRollPitchRange.y);
    }

    private CameraDirection GetAnimatorDirection()
    {
        if (anim == null)
            return CameraDirection.Forward;

        if (anim.GetBool("Back"))
            return CameraDirection.Back;

        if (anim.GetBool("Left"))
            return CameraDirection.Left;

        if (anim.GetBool("Right"))
            return CameraDirection.Right;

        return CameraDirection.Forward;
    }

    private void ResetAnimatorBools()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Back", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);
    }

    private void CompleteTutorialPrompt()
    {
        if (instructions == null)
            return;

        Destroy(instructions);

        var saveTutorialData = new SaveTutorialData()
        {
            hasPlayedTutorial = true
        };

        var saveProfile = new SaveProfile<SaveTutorialData>(
            "SaveTutorialData",
            saveTutorialData
        );

        SaveManager.SaveAs(saveProfile, true);
    }

    //I FORGOT WHAT THE FUCK THIS DOES BUT GET RID OF IT. SHIT BREAKS ! !! ! ! 
    public void idle()
    {
        isBack = false;
    }
}
