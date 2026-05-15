using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DouglasMove : MonoBehaviour
{
    public CalculateStress stress;
    
    [Header("Waypoints")]
    public DouglasWaypoints currentWaypoint;
    public Transform player;
    [SerializeField] private Transform startingPoint;

    [Header("Movement")]
    [SerializeField] private float timeBetweenMoves;
    [SerializeField] private float decreaseAmount;
    [SerializeField] private int moveChance;
    [SerializeField] private LerpCam camZoom;
    [SerializeField, Tooltip("Increases per night")] private float difficultyMultiplier;
    [Tooltip("Higher = lower total stress added")] public float stressMultiplier;
    [SerializeField] private NightCycle nightCycle;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private Animator anim;
    [SerializeField] private int currentDouglasIdentifier;
    
    [Header("AntiDoug App")]
    [SerializeField] private bool canPress;
    [SerializeField] private float countdown = 15;
    [SerializeField] private float timer = 5;
    
    [Header("Computer")]
    [SerializeField] private float range;
    [SerializeField] private PC computer;
    [SerializeField] private LayerMask pcLayer;
    private static readonly int AnimationIdentifier = Animator.StringToHash("Identifier");

    [SerializeField] private bool forceJumpscare;

    private void Start()
    {
        this.transform.position = startingPoint.position;
        difficultyMultiplier = SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount;
    }
    
    private void Update()
    {
        if (!NightCycle.isNightActive) return;

        
        var douglasCol = gameObject.GetComponent<CapsuleCollider>();
        douglasCol.direction = currentWaypoint.colliderAxis;
        douglasCol.center = currentWaypoint.colliderOffset;

        if (!canPress)
        {
            countdown -= timer * Time.deltaTime;
            
            if (countdown <= 0)
            {
                countdown = Random.Range(15, 20);
                canPress = true;
            }
        }
        
        currentDouglasIdentifier = currentWaypoint.identifier;
        anim.SetInteger(AnimationIdentifier, currentDouglasIdentifier);
        transform.rotation = Quaternion.Euler(currentWaypoint.waypointRotation);
        
        timeBetweenMoves = Mathf.Clamp(timeBetweenMoves, 0, 150); //150 = max time between moves
        timeBetweenMoves -= Time.deltaTime * (decreaseAmount * difficultyMultiplier);
        //Debug.Log(RenderSettings.fogDensity);
        
        if (currentWaypoint.canJumpscare || forceJumpscare)
        {
            RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity ,0 , 1);
            RenderSettings.fogDensity += Time.deltaTime;
            camZoom.enabled = false;
            currentWaypoint.jumpscare.Invoke("TriggerJumpscare", Random.Range(3, 5));
        }
        
        if (timeBetweenMoves > 0) return; //only move if its below or equal to 0
        var completeMove = Random.Range(1, moveChance);

        if (completeMove is 1)
        {
            if (Random.Range(1, 5) == 1)
                footstep.Play();
            
            timeBetweenMoves = 150;
            currentWaypoint.Move(this);
        }
        

        computer.douglasManipulation = Physics.CheckSphere(this.transform.position, range, pcLayer) ? 3 : 1;
    }


    public void ForceButtonMove(Transform waypoint)
    {
        if (canPress)
        {
            currentWaypoint.Move(this, true, waypoint, false);
            timeBetweenMoves = 150f;
            canPress = false;
        }
    }

    public IEnumerator OverrideMove(Transform waypoint, float time)
    {
        yield return new WaitForSeconds(time);
        currentWaypoint.Move(this, true, waypoint, false);
        timeBetweenMoves = 150f;
        CalculateStress.UpdateStress(Random.Range(-1f, -5f));
        yield return null;
    }
}