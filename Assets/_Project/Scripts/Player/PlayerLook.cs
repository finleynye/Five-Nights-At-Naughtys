using TMPro;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 100f;
    private float _mouseX;
    private float _mouseY;
    private float _rotY, _rotX;
    private float _multiplier = .01f;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform orientation;

    [Header("Camera Restriction")]
    public bool left;
    public bool right;
    public bool idle;
    public bool back;

    [Header("Animation")]
    public Animator cameraAnim;
    [SerializeField] private bool ignoreLookInputDuringCameraTransitions = true;

    private float cameraTransitionTimer;

    private void Start()
    {
        left = cameraAnim.GetBool("Left");
        right = cameraAnim.GetBool("Right");
        idle = cameraAnim.GetBool("Idle");
        back = cameraAnim.GetBool("Back");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (PC.isOn) return;

        var canLook = !ignoreLookInputDuringCameraTransitions || cameraTransitionTimer <= 0f;
        if (cameraTransitionTimer > 0f)
            cameraTransitionTimer -= Time.deltaTime;

        _mouseX = canLook ? Input.GetAxisRaw("Mouse X") : 0f;
        _mouseY = canLook ? Input.GetAxisRaw("Mouse Y") : 0f;

        _rotX -= _mouseY * sensitivity * _multiplier;
        _rotY += _mouseX * sensitivity * _multiplier;

        if (idle)
        {
            _rotX = Mathf.Clamp(_rotX, -60, 30);
            _rotY = Mathf.Clamp(_rotY, -270, -90);

            _camera.transform.localRotation = Quaternion.Euler(_rotX, _rotY, 0);
            orientation.transform.rotation = Quaternion.Euler(0, _rotY, 0);
        }
        else if (back)
        {
            _rotX = Mathf.Clamp(_rotX, -60, 30);
            _rotY = Mathf.Clamp(_rotY, -270, -90);

            _camera.transform.localRotation = Quaternion.Euler(_rotX, _rotY, 0);
            orientation.transform.rotation = Quaternion.Euler(0, _rotY, 0);
        }
        else if (left)
        {
            _rotY = Mathf.Clamp(_rotY, -250, -130);
            _rotX = Mathf.Clamp(_rotX, -60, 15);

            _camera.transform.localRotation = Quaternion.Euler(_rotX, _rotY, 0);
            orientation.transform.rotation = Quaternion.Euler(0, _rotY, 0);


        }
        else if (right)
        {
            _rotY = Mathf.Clamp(_rotY, -270, -57.5f);
            _rotX = Mathf.Clamp(_rotX, -60, 25);

            _camera.transform.localRotation = Quaternion.Euler(_rotX, _rotY, 0);
            orientation.transform.rotation = Quaternion.Euler(0, _rotY, 0);


        }
        else
            return;


        if (PC.isOn == true)
        {
            //This freezes the sens of the player when the PC is on.
            _mouseX = 0f;
            _mouseY = 0f;
        }

        if (PC.isOn == false)
        {
            //This forces the sens back to 100f, need to remember to change it to use a variable of whatever we set the player sens to.
            _mouseX = 100f;
            _mouseY = 100f;
        }

        //ResetView();
    }

    private void ResetView()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _camera.transform.rotation = Quaternion.Euler(0, -180, 0);
        }

    }

    public void OnLedge()
        => left = true;

    public void OffLedge()
        => left = false;

    public void isIdle()
        => idle = true;
    public void isntIdle()
      => idle = false;

    public void isBack()
    => back = true;
    public void isntBack()
      => back = false;

    public void isRight()
    => right = true;
    public void isntRight()
    => right = false;

    public void SetCameraRestriction(bool isIdle, bool isBack, bool isLeft, bool isRight)
    {
        idle = isIdle;
        back = isBack;
        left = isLeft;
        right = isRight;
    }

    public void BeginCameraTransition(float duration)
    {
        cameraTransitionTimer = Mathf.Max(cameraTransitionTimer, duration);
    }
}

