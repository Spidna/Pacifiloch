using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WildMovement : MonoBehaviour
{
    [Tooltip("Lower value allows faster rotation??")]
    protected float smoothDamp;
    [Tooltip("My Regidbody")]
    protected Rigidbody rb;
    [Tooltip("How fast I move")]
    protected float speed;

    protected Vector3 curVel;

    public abstract void Move();

    private GameObject _target;
    public abstract GameObject target { get; }
}
