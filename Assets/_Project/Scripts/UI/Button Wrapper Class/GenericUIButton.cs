using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TextOnlyUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Button State")]
    [SerializeField] private bool isClickable = true;

    [Tooltip("Use this for titles or decorative text that should animate but not be clickable.")]
    [SerializeField] private bool useUnclickableIdleBehaviour = false;

    [Header("Text Colours")]
    [SerializeField] private Color normalColour = new Color32(232, 232, 221, 255);
    [SerializeField] private Color hoverColour = new Color32(214, 179, 35, 255);
    [SerializeField] private Color clickColour = new Color32(240, 211, 74, 255);
    [SerializeField] private Color disabledColour = new Color32(80, 84, 90, 255);

    [Header("Unclickable Text Colour")]
    [SerializeField] private Color unclickableIdleColour = new Color32(232, 232, 221, 255);

    [Header("Text Behaviour")]
    [SerializeField] private bool useHoverScale = true;
    [SerializeField] private float hoverScaleAmount = 1.08f;
    [SerializeField] private float scaleSpeed = 10f;

    [Header("Idle Behaviour")]
    [SerializeField] private bool useIdlePulse = false;
    [SerializeField] private float idlePulseSpeed = 2f;
    [SerializeField] private float idlePulseAmount = 0.04f;

    [Header("Unclickable Idle Behaviour")]
    [SerializeField] private bool useUnclickablePulse = true;
    [SerializeField] private float unclickablePulseSpeed = 1.2f;
    [SerializeField] private float unclickablePulseAmount = 0.015f;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Click Event")]
    public UnityEvent onClick;

    private TMP_Text buttonText;

    private Vector3 baseScale;
    private Vector3 targetScale;

    private Coroutine colourRoutine;
    private Coroutine clickRoutine;

    private bool isHovered;

    private void Awake()
    {
        buttonText = GetComponent<TMP_Text>();

        //only clickable text needs to block raycasts.
        buttonText.raycastTarget = isClickable;

        baseScale = transform.localScale;
        targetScale = baseScale;

        ApplyClickableState();
    }

    private void Update()
    {
        HandleScale();
        HandleIdlePulse();
        HandleUnclickableIdlePulse();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable)
            return;

        isHovered = true;

        PlaySound(hoverSound);
        SetTextColour(hoverColour);

        if (useHoverScale)
            targetScale = baseScale * hoverScaleAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable)
            return;

        isHovered = false;

        SetTextColour(normalColour);
        targetScale = baseScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable)
            return;

        PlaySound(clickSound);

        if (clickRoutine != null)
            StopCoroutine(clickRoutine);

        clickRoutine = StartCoroutine(ClickFlash());

        onClick?.Invoke();
    }

    private void HandleScale()
    {
        if (useIdlePulse && isClickable && !isHovered)
            return;

        if (useUnclickableIdleBehaviour && !isClickable)
            return;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * scaleSpeed
        );
    }

    private void HandleIdlePulse()
    {
        if (!useIdlePulse || !isClickable || isHovered)
            return;

        float pulse = Mathf.Sin(Time.unscaledTime * idlePulseSpeed) * idlePulseAmount;
        transform.localScale = baseScale + Vector3.one * pulse;
    }

    private void HandleUnclickableIdlePulse()
    {
        if (!useUnclickableIdleBehaviour || isClickable || !useUnclickablePulse)
            return;

        float pulse = Mathf.Sin(Time.unscaledTime * unclickablePulseSpeed) * unclickablePulseAmount;
        transform.localScale = baseScale + Vector3.one * pulse;
    }

    private IEnumerator ClickFlash()
    {
        SetTextColour(clickColour);

        yield return new WaitForSecondsRealtime(0.08f);

        if (isHovered)
            SetTextColour(hoverColour);
        else
            SetTextColour(normalColour);
    }

    private void SetTextColour(Color targetColour)
    {
        if (colourRoutine != null)
            StopCoroutine(colourRoutine);

        colourRoutine = StartCoroutine(FadeTextColour(targetColour));
    }

    private IEnumerator FadeTextColour(Color targetColour)
    {
        Color startColour = buttonText.color;
        float timer = 0f;
        float duration = 0.12f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;

            buttonText.color = Color.Lerp(startColour, targetColour, t);

            yield return null;
        }

        buttonText.color = targetColour;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    private void ApplyClickableState()
    {
        isHovered = false;
        targetScale = baseScale;

        if (buttonText != null)
            buttonText.raycastTarget = isClickable;

        if (isClickable)
        {
            SetTextColour(normalColour);
        }
        else
        {
            if (useUnclickableIdleBehaviour)
                SetTextColour(unclickableIdleColour);
            else
                SetTextColour(disabledColour);

            transform.localScale = baseScale;
        }
    }

    public void SetClickable(bool value)
    {
        isClickable = value;
        ApplyClickableState();
    }

    public void SetUnclickableIdleMode(bool value)
    {
        useUnclickableIdleBehaviour = value;
        ApplyClickableState();
    }

    public void EnableButton()
    {
        SetClickable(true);
    }

    public void DisableButton()
    {
        SetClickable(false);
    }
}