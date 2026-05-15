using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FlashLight : MonoBehaviour
{
    [Header("Flash Attributes")]
    public GameObject Light;
   [HideInInspector] public bool isEquipped;
    private bool flashOn;
    public int equip;
    public LayerMask douglas;
    public Transform forceTo;

    [Header("Audio")]
    public AudioSource Equipped;
    public AudioSource Click;

    [Header("Animations")]
    public Animator anim;

    [Header("Battery Life")]
    public float batteryLife;
    public float maxBatteryLife = 100f;
    public float timeToKill = 1000f;

    [Header("UI")]
    public TMP_Text BatteryHealth;
    public GameObject instructions;
    

    void Start()
    {
        Light.SetActive(false);
        batteryLife = maxBatteryLife;
        
        if(SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);
    }
    private void Update()
    {
        if (PC.isOn) return;
        Equip();
        UnEquip();
        BatteryLife();
        
        if(TorchCharging.isCharging)
            UnEquip();
        
        batteryLife = Mathf.Clamp(batteryLife, 0, 100);

    }
    
    private void Equip()
    {
        if (batteryLife <= 0)
            return;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
            }
            equip++;
            isEquipped = true;
            anim.SetInteger("EquipNum", equip);
            if(equip == 1)
                Equipped.Play();
        }

        if (isEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                flashOn = true;
                Light.SetActive(true);
                Click.Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                flashOn = false;
                Light.SetActive(false);
            }

            //When active shoot out raycast
            //If raycast hits douglas, get his component
            //And force him to go to lift
            if (Physics.Raycast(this.transform.position, -this.transform.forward, out var hit, 5, douglas))
            {
                if (flashOn)
                {
                    var douglasObj = hit.collider.GetComponentInParent<DouglasMove>();
                    var douglasObjs = hit.collider.GetComponent<DouglasMove>();
                   
                    douglasObj.currentWaypoint.Move(douglasObj, true, forceTo);
                    douglasObjs.currentWaypoint.Move(douglasObj, true, forceTo);
                    batteryLife -= Random.Range(5, 10);
                }
            }
        }

    }

    private void UnEquip()
    {
        if (batteryLife <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (equip >= 2)
            {
                isEquipped = false;
                equip = 0;
                anim.SetInteger("EquipNum", equip);
                Light.SetActive(false);
            }
        }
    }

    private void BatteryLife()
    {
        if (isEquipped && flashOn)
            batteryLife -= Time.deltaTime * timeToKill;

        BatteryHealth.text = $"{(int)batteryLife}%";
    }


}
