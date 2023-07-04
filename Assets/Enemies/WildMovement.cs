using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WildMovement : MonoBehaviour
{
    [Tooltip("Lower value allows faster rotation??")]
    [SerializeField] protected float smoothDamp;
    [Tooltip("My Rigidbody")]
    [SerializeField] protected Rigidbody rb;
    [Tooltip("How fast I move")]
    [SerializeField] protected float speed;

    public Flock assignedFlock;


    protected Vector3 curVel;

    public abstract void Move();

    //private GameObject _target;
    //public abstract GameObject target { get; }
}
