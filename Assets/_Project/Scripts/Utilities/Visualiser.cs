using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour
{
    [SerializeField] private Color wireColour;
    [SerializeField, Range(0, 2)] private float size;
    
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = wireColour;
            Gizmos.DrawWireSphere(t.position, size);
        }
        
        //Gizmos.color = Color.green;
        //for (var i = 0; i < transform.childCount - 1; i++)
            //Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        
        //Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }
}