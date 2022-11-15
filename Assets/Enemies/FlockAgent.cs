using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    [Tooltip("The field in which other agents can be seen")]
    [SerializeField] private float angleFOV;
    [Tooltip("Lower value allows faster rotation??")]
    [SerializeField] private float smoothDamp;
    [SerializeField] private Rigidbody rb;

    private List<FlockAgent> cohesionNeighbours = new List<FlockAgent>();
    private List<FlockAgent> avoidanceNeighbours = new List<FlockAgent>();
    private List<FlockAgent> alignmentNeighbours = new List<FlockAgent>();
    private Flock assignedFlock;
    private Vector3 curVel;
    [SerializeField] private float speed;


    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void MoveUnit()
    {
        // Gravitate towards seen neighbours
        FindNeighbours();
        Vector3 cohesionVector = CalcCohesionVector() * assignedFlock.cohesionWeight;
        // Avoid personal space of neighbours
        Vector3 avoidanceVector = CalcAvoidanceVector() * assignedFlock.avoidanceWeight;
        // Go the same direction as neighbours
        Vector3 alignmentVector = CalcAlignmentVector() * assignedFlock.alignmentWeight;
        // Home in on target (aka player)
        Vector3 targetVector = CalcTargetVector() * assignedFlock.targetWeight;

        // move
        Vector3 moveVector = cohesionVector + avoidanceVector + alignmentVector + targetVector;
        moveVector = moveVector.normalized * speed;
        moveVector = Vector3.SmoothDamp(transform.forward, moveVector, ref curVel, smoothDamp);
        rb.transform.forward = moveVector;
        rb.MovePosition(transform.position + moveVector * Time.deltaTime);
    }

    private void FindNeighbours()
    {
        // Start fresh
        cohesionNeighbours.Clear();
        avoidanceNeighbours.Clear();
        alignmentNeighbours.Clear();
        List<FlockAgent> allAgents = assignedFlock.allAgents; // Collect agents from current flock for faster calcs
        for (int i = 0; i < allAgents.Count; i++)
        {
            FlockAgent curAgent = allAgents[i];
            if (curAgent != this) // Skip checking self
            {
                // If agent is close enough, make them a neighbour
                float neighbourDist = Vector3.SqrMagnitude(curAgent.transform.position - transform.position);
                if (neighbourDist <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                { // Cohesion Neighbour check
                    cohesionNeighbours.Add(curAgent);
                }
                if (neighbourDist <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                { // Avoidance Neighbour check
                    avoidanceNeighbours.Add(curAgent);
                }
                if (neighbourDist <= assignedFlock.alignmentDistance * assignedFlock.alignmentDistance)
                { // Alignment Neighbour check
                    alignmentNeighbours.Add(curAgent);
                }
            }
        }
    }

    // Gravitate towards the middle of seen neighbours
    private Vector3 CalcCohesionVector()
    {
        Vector3 cohesionVector = Vector3.zero;

        // choose neighbours in FOV and average out ?positions?
        int neighboursSeen = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            if (IsInFOV(cohesionNeighbours[i].transform.position))
            {
                neighboursSeen++;
                cohesionVector += cohesionNeighbours[i].transform.position;
            }
        }

        // With no neighbours seen there's no point in making further calculations
        if (neighboursSeen == 0)
        {
            return cohesionVector;
        }

        // Calculate cohesion vector
        cohesionVector /= neighboursSeen;
        cohesionVector -= transform.position; // convert to local space
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }

    // Avoid neighbours' personal space
    private Vector3 CalcAvoidanceVector()
    {
        Vector3 avoidanceVector = Vector3.zero;

        // Average out position of seen neighbours
        int neighboursSeen = 0;
        for (int i = 0; i < avoidanceNeighbours.Count; i++)
        {
            if (IsInFOV(avoidanceNeighbours[i].transform.position))
            {
                neighboursSeen++;
                // Average of the differences between my pos and neighbour pos
                avoidanceVector += (transform.position - avoidanceNeighbours[i].transform.position);
            }
        }

        // With no neighbours seen there's no point in making further calculations
        /// Supposedly redundant
        if (neighboursSeen == 0)
        {
            return avoidanceVector;
        }

        // Calculate cohesion vector
        avoidanceVector /= neighboursSeen;
        avoidanceVector = avoidanceVector.normalized;
        return avoidanceVector;
    }

    // Go the same direction as neighbours
    /// Even if we target player, may as well keep this backup function for later
    private Vector3 CalcAlignmentVector()
    {
        Vector3 alignVector = transform.forward;

        int neighboursSeen = 0;
        for (int i = 0; i < alignmentNeighbours.Count; i++)
        { // Average out direction of seen neighbours
            if (IsInFOV(alignmentNeighbours[i].transform.position))
            {
                neighboursSeen++;
                alignVector += alignmentNeighbours[i].transform.forward;
            }
        }
        // No point without neighbours
        if (alignmentNeighbours.Count <= 0)
        {
            return alignVector;
        }

        alignVector /= neighboursSeen;
        alignVector = alignVector.normalized;
        return alignVector;
    }

    // Go towards target, typically the player
    private Vector3 CalcTargetVector()
    {
        return (assignedFlock.target.transform.position - transform.position).normalized;
    }

    private bool IsInFOV(Vector3 neighbourPos)
    {
        return Vector3.Angle(transform.forward, neighbourPos - transform.position) <= angleFOV;
    }
}
