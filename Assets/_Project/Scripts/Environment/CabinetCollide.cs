using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetCollide : MonoBehaviour
{
    [SerializeField] private AudioSource bang;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cabinet"))
        {
            bang.Play();
            Debug.Log("kicked");
        }
    }


    public void KickSFX()
    {
        bang.Play();
    }
}
