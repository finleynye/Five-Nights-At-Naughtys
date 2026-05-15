using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TorchCharging : MonoBehaviour
{
    public FlashLight FL;
    public GameObject ChargerFlash;
    Ray ray;
    
    public LayerMask Torch;
    public Transform player;
    public static bool isCharging;

    public TMP_Text flashBattery;

    public Animator flashAnim;
    public GameObject instructions;
    void ChargeTorch()
    {

        var canCharge = Physics.Raycast(player.transform.position, player.forward, 1000, Torch);

        if (Input.GetKeyDown(KeyCode.F) && canCharge && FL.isEquipped)
        {
            if (instructions is not null)
            {
                Destroy(instructions);
            }
            isCharging = true;
            FL.equip = 0;
            FL.gameObject.SetActive(false);
            ChargerFlash.SetActive(true);
        }
    
        //Debug.Log(isCharging);
    
    }


    IEnumerator Charging()
    {
        if (isCharging)
        {
            yield return new WaitForSeconds(20);
            Debug.Log(isCharging);
           
            FL.gameObject.SetActive(true);
            ChargerFlash.SetActive(false);
            isCharging = false;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        flashBattery.text = "Flash Charger";
        ChargerFlash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChargeTorch();

        if (SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);


        if (isCharging)
        {
            flashBattery.text = $"{(int)FL.batteryLife}%";
            FL.batteryLife += Time.deltaTime * FL.timeToKill;

            if(FL.batteryLife >= FL.maxBatteryLife)
            {
                FL.gameObject.SetActive(true);
                ChargerFlash.SetActive(false);
                FL.equip = 1;
                flashAnim.SetInteger("EquipNum",FL.equip);
                flashBattery.text = "Flash Charger";
                isCharging = false;
            }
        }
    }
}
