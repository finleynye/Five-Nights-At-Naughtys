using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class DouglasWaypoints
{
    public string currentPositionName;
    public Transform currentPosition;
    public Vector3 waypointRotation;
    public AnimationClip roomAnimation;
    public Transform[] possibleMoves;
    public int identifier;
    public int colliderAxis = 1; // 0 = x, 1 = y, 2 = z
    public Vector3 colliderOffset;

    [Header("JumpScare")]
    public JumpScare jumpscare;
    public bool canJumpscare;
    
    public void Move(DouglasMove caller, bool manualOverride = false, Transform forceMoveTo = null, bool addStress = true)
    {
        switch (manualOverride)
        {
            case false:
                {
                    //so he needs to start in the elevator, and pick a random point from his currentPoint's possible moves list
                    //then he needs to go to that new point, and set it to his current
                    //then wash rinse repeat ideally

                    var nextPosition = Random.Range(0, possibleMoves.Length);
                    caller.transform.position = possibleMoves[nextPosition].position;
                    break;
                }
            case true:
                {
                    //this will let the player manually force douglas to divert away from the filing room, and go to a set position
                    if (forceMoveTo is not null)
                        caller.transform.position = forceMoveTo.position;
                    break;
                }
        }

        //phone BPM now reads Douglas distance every frame, so movement should not permanently add stress
    }
}
