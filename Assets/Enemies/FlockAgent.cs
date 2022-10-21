using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    [Tooltip("The field in which other agents can be seen")]
    [SerializeField] private float angleFOV;
    private List<FlockAgent> cohesionNeighbours = new List<FlockAgent>();
    private Flock assignedFlock;

    public Transform myTransform { get; set; } // Quick access for faster computing

    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void MoveUnit()
    {
        Vector3 cohesionVector = CalculateCohesionVector();
    }

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear(); // Start fresh
        FlockAgent[] allAgents = assignedFlock.allAgents; // Collect agents from current flock for faster calcs
        for (int i = 0; i < allAgents.Length; i++)
        {
            FlockAgent curAgent = allAgents[i];
            if (curAgent != this) // Skip checking self
            {
                // If agent is close enough make them a neighbour
                float neighbourDist = Vector3.SqrMagnitude(curAgent.transform.position - transform.position);
                if (neighbourDist <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbours.Add(curAgent);
                }
            }
        }
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        // Without neighbours there's nothing to return
        if (cohesionNeighbours.Count == 0)
            return cohesionVector;

        // Find neighbours and average out ?positions?
        int neighboursSeen = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            if (IsInFOV(cohesionNeighbours[i].myTransform.position))
            {
                neighboursSeen++;
                cohesionVector += cohesionNeighbours[i].myTransform.position;
            }
        }

        // With no neighbours seen there's no point in making further calculations
        if (neighboursSeen == 0)
        {
            return cohesionVector;
        }

        // Calculate cohesion vector
        cohesionVector /= neighboursSeen;
        cohesionVector -= myTransform.position; // convert to local space
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    private bool IsInFOV(Vector3 neighbourPos)
    {
        return Vector3.Angle(myTransform.forward, neighbourPos - myTransform.position) <= angleFOV;
    }
}
