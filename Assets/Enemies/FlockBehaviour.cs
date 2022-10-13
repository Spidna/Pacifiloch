using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    public abstract Vector3 CalcMove(FlockAgent agent, List<Transform> context, Flock flock);
}
