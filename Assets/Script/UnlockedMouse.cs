using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedMouse : MonoBehaviour
{

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
