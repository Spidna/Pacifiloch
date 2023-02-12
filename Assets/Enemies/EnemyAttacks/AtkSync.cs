using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSync : MonoBehaviour
{

    [Tooltip("The thing that the flock attacks/avoids")]
    // TODO: update to have a list of targets in the event of multiplayer or 'substitutes'
    [SerializeField] public GameObject _target;
    [Tooltip("Tag used to find target(s)")]
    [SerializeField] public string _targetTag;

    [Tooltip("Timer for flock decision making")]
    [SerializeField] public float t;
}
