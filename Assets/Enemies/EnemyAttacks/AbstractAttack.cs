using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttack : MonoBehaviour
{
    public enum stage
    {
        windup, execution, recoil, cooldown
    }

    [Tooltip("windup, execution, recoil, cooldown durations")]
    [SerializeField] protected float[] atkStage;
    [Tooltip("Time remaining on current attack stage")]
    [SerializeField] protected float progressTime;

    public abstract void atk(ref Rigidbody _rb);
}
