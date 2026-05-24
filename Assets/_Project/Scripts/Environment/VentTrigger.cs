using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentTrigger : MonoBehaviour
{
    [SerializeField] private int ventCount;
    [SerializeField] private Animator VentAnim;
    [SerializeField] private AudioSource ventNoise;

    [SerializeField] private DouglasWaypoints thisWaypoint;
    [SerializeField] private Transform[] moveTo;

    private void Start()
        => thisWaypoint = GetComponent<ColliderWaypoint>().thisWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Douglas"))
        {
            if (other.GetComponent<DouglasMove>().currentWaypoint.identifier is 10 or 11)
            {
                var douglas = other.GetComponent<DouglasMove>();

                ventNoise.Play();
                ventCount++;
                if (ventCount == 3)
                {
                    VentAnim.SetTrigger("VentOpen");
                    
                }
                else if (ventCount < 3)
                {
                    StartCoroutine(douglas.OverrideMove(moveTo[Random.Range(0, moveTo.Length)], 5));
                }
            
            }
        }
    }
}

