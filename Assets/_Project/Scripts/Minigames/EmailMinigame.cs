using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EmailMinigame : MonoBehaviour
{
    public GameObject[] emails; // Array of email GameObjects
    [SerializeField] private GameObject emailPopup;
    [SerializeField] private float popupMultiplier;

    [Header("Quota")] 
    public int quotaToHit;
    public int currentQuota;
    public Slider quotaUI;

    private bool[] _isDeleted; // Array to track deleted status
    private float[] _respawnTimers; // Array to store respawn timers

    [Header("Stress")]
    [SerializeField] private CalculateStress calculateStress;

    private void Start()
    {
        quotaToHit = (SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount * 10) + 5;
        quotaUI.maxValue = quotaToHit;
        
        var emailCount = emails.Length;
        _isDeleted = new bool[emailCount];
        _respawnTimers = new float[emailCount];

        for (var i = 0; i < emailCount; i++)
        {
            _isDeleted[i] = false;
            _respawnTimers[i] = 0f;
        }
    }

    private void Update()
    {
        quotaUI.value = currentQuota;
        
        if (currentQuota >= quotaToHit) return;
        var emailCount = emails.Length;

        for (var i = 0; i < emailCount; i++)
        {
            emailPopup.SetActive(AnyEmailActive());
            
            if (_isDeleted[i])
            {
                _respawnTimers[i] -= Time.deltaTime / popupMultiplier;

                if (_respawnTimers[i] <= 0f)
                {
                    emails[i].SetActive(true);
                    _isDeleted[i] = false;
                }
            }
        }

        if(currentQuota >= quotaToHit)
        {
            calculateStress.DecreaseStress(Random.Range(15, 25));
        }
            
    }
    
    private bool AnyEmailActive()
        => emails.Any(email => email.activeSelf);
    

    public void DeleteEmail(int index)
    {
        if (index >= 0 && index < emails.Length)
        {
            emails[index].SetActive(false);
            _isDeleted[index] = true;
            _respawnTimers[index] = Random.Range(0.5f, 3f);

            currentQuota++;
        }
    }
}
