using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveAI : MonoBehaviour
{
    [Header("Player")] 
    [SerializeField] private CalculateStress playerStress;
    [SerializeField] private Transform player;
    
    [Header("Move Positions")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private Transform startPos;
    [SerializeField] private AnimationClip[] roomAnimations;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator doorAnim;

    [Header("Movement")]
    [SerializeField] private float timeBetweenMoves;
    [SerializeField] private float decreaseAmount;
    [SerializeField] private float difficultyMultiplier;
    private readonly int maxTBM = 150; //maxTimeBetweenMoves
    private bool leftElevator = false;

    private void Start()
        => this.transform.position = startPos.position;

    private void Update()
    {
        if (!NightCycle.isNightActive) return;
    
        timeBetweenMoves = Mathf.Clamp(timeBetweenMoves, 0, maxTBM); // never go below 0 or above maxTMB (150)
        timeBetweenMoves -= Time.deltaTime * (decreaseAmount * difficultyMultiplier);
        // makes it so time between moves gets shorter each night, increasing difficulty

        //when it hits 0, attempt a move
        if (timeBetweenMoves > 0) return;
        var completeMove = Random.Range(1, 3);
        
        if (!leftElevator || completeMove is 2) // force douglas out of the elevator
            Move();

        timeBetweenMoves = maxTBM;
        leftElevator = true;
    }

    private void Move()
    {
        if (!leftElevator)
        {
            doorAnim.SetTrigger("OpenDoors");
            
        }
        
        var douglasIndex = Random.Range(0, waypoints.Count);
        this.transform.position = waypoints[douglasIndex].position;
        
        //anim.Play(roomAnimations[douglasIndex].ToString());
        anim.SetInteger("RoomTrigger", douglasIndex);

        //Update player stress meter
        var distToPlayer = this.transform.position - player.position;
        var stressCalc = ((1 - distToPlayer.magnitude / 100) * 10) - distToPlayer.magnitude / 10;
        //playerStress.UpdateStress(stressCalc);
    }

    /*[Header("Player Info")] 
    [SerializeField] private Transform player;
    [SerializeField] private CalculateStress stress;

    [Header("Move Positions")] 
    [SerializeField] private List<Transform> canMoveTo;
    [SerializeField] private LayerMask movePoints;
    
    //[Header("Determine Movement")]
    
    private void Start()
    {
       //kys 
    }

    private void Update()
    {
        
    }*/
}
