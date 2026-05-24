using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderWaypoint : MonoBehaviour
{
    public DouglasWaypoints thisWaypoint;
    
    private void Start()
    {
        thisWaypoint.currentPositionName = this.name;
        thisWaypoint.currentPosition = this.transform;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Douglas"))
        {
            var douglas = other.GetComponent<DouglasMove>();
            douglas.currentWaypoint = thisWaypoint;
            Debug.Log("Douglas moved");
        }
    }
}
