using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskFanSpin : MonoBehaviour
{
    public float fanSpeed = 250f;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, fanSpeed * Time.deltaTime));
    }
}
