using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "ScriptableObjects/AtkSync", order = 1)]
public class AtkSync : ScriptableObject
{

    //[Tooltip("The thing that the flock attacks/avoids")]
    //// TODO: update to have a list of targets in the event of multiplayer or 'substitutes'
    //[SerializeField] public GameObject _target;
    [Tooltip("Tag used to find target(s)")]
    [SerializeField] public string _targetTag;

    [Tooltip("Timer for flock decision making")]
    [SerializeField] public float t;
}
