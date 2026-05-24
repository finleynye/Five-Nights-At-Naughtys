using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private float rotationAmt = 15;
    [SerializeField] private float startingYPos;
    [SerializeField] private float lookDown = 20;
    private float _moveTimer = 5;
    private Camera _thisCam;

    private void Start()
        => _thisCam = GetComponent<Camera>();

    private void LateUpdate()
        => _moveTimer += Time.deltaTime * speed;
    
    //WHAT THE FUCK DOES THIS DO!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void FixedUpdate()
    {
        var angle = Mathf.Sin(Time.fixedDeltaTime * _moveTimer) * rotationAmt + startingYPos;
        _thisCam.transform.eulerAngles = new Vector3(lookDown, angle, 0);
    }
}
