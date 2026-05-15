using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerOpen : MonoBehaviour
{
    private Animator anim;
    private float repairTime = 3f;
    private bool isRepairing;
    [SerializeField] private GameObject repairText;
    [SerializeField] private PC _pc;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRepairing)
        {
            _pc.crashPercent = 20;
        }
    }

    public void VentClose()
    {
        StartCoroutine(Repairing());
        repairText.SetActive(true);
    }

    public IEnumerator Repairing()
    {
        anim.SetTrigger("idle");
        yield return new WaitForSeconds(repairTime);
        repairText.SetActive(false);
    }
}
