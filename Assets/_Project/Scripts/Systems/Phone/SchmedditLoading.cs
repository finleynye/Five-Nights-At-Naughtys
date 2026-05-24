using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchmedditLoading : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward * (Time.deltaTime * 250));
    }
}
