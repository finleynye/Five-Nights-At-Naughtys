using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpin : MonoBehaviour
{
    public float fanSpeed = 250f;
    void Update()
    {
        transform.Rotate(new Vector3(0f, fanSpeed * Time.deltaTime, 0f));
    }

}
