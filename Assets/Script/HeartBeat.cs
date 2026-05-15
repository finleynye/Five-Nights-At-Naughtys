using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    private AudioSource _heartBeat;

    private void Start()
        =>_heartBeat = GetComponent<AudioSource>();

    public void PlayHeartBeat()
        =>_heartBeat.Play();
    
}
