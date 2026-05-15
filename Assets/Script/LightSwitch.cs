using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Transform cam;
    public float playerActivateDistance;
    bool isActive = false;
    public LayerMask Light;

    public AudioSource Switch;
    public bool LightOn;
    void Start()
    {
        LightOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isActive = Physics.Raycast(ray, out hit, playerActivateDistance, Light);

        if (Input.GetMouseButtonDown(0) && isActive == true)
            if (hit.transform.gameObject.GetComponentInChildren<Light>() != null)
            {
                LightOn = !LightOn;
                Switch.Play();
                hit.transform.GetChild(0).GetComponent<Light>().enabled = LightOn;
            }
        
        //Debug.Log(isActive);
    }
}
