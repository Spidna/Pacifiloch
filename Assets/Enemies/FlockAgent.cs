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

    private List<FlockAgent> cohesionNeighbours = new List<FlockAgent>();
    private Flock assignedFlock;
    private Vector3 curVel;
    [SerializeField] private float speed;

    //public Transform myTransform { get; set; } // Quick access for faster computing

    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    // InitializeSpeed is a fake function that is redundant

    public void MoveUnit()
    {
        // Gravitate towards seen neighbours
        FindNeighbours();
        Vector3 cohesionVector = CalculateCohesionVector();
        // Push towards cohesion vector in world space
        Vector3 moveVector = Vector3.SmoothDamp(transform.forward, cohesionVector, ref curVel, smoothDamp);
        moveVector = moveVector.normalized * speed;
        transform.forward = moveVector;
        transform.position += moveVector * Time.deltaTime;
    }

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear(); // Start fresh
        List<FlockAgent> allAgents = assignedFlock.allAgents; // Collect agents from current flock for faster calcs
        for (int i = 0; i < allAgents.Count; i++)
        {
            FlockAgent curAgent = allAgents[i];
            if (curAgent != this) // Skip checking self
            {
                // If agent is close enough, make them a neighbour
                float neighbourDist = Vector3.SqrMagnitude(curAgent.transform.position - transform.position);
                if (neighbourDist <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbours.Add(curAgent);
                }
            }
        }
    }

    // Gravitate towards the middle of seen neighbours
    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;

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
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    private bool IsInFOV(Vector3 neighbourPos)
    {
        return Vector3.Angle(transform.forward, neighbourPos - transform.position) <= angleFOV;
    }
}
