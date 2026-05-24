using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankSpin : MonoBehaviour
{
    public float fanSpeed = 250f;
    void Update()
    {
        transform.Rotate(new Vector3(fanSpeed * Time.deltaTime, 0f, 0f));
    }
}
