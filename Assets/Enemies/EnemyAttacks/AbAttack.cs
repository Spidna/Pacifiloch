using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody rb;
    // Stored here because cooldown doesn't do anything
    [SerializeField] protected AbStage cooldown;

    [Header("Values")]
    [Tooltip("Time remaining on current attack stage")]
    [SerializeField] public float progressTime;

    [Tooltip("Duration of Windup in seconds")]
    [SerializeField] protected float maxWindup;
    [Tooltip("Duration of Execution in seconds")]
    [SerializeField] protected float maxExecute;
    [Tooltip("Duration of Recoil in seconds")]
    [SerializeField] protected float maxRecoil;
    [Tooltip("Duration of Cooldown in seconds")]
    [SerializeField] public float maxCooldown;

    [Tooltip("windup, execution, recoil, cooldown")]
    [SerializeField] protected AbStage curAtkStage;
    [Tooltip("How far the attack travels")]
    [SerializeField] protected float magnitude;

    public abstract bool atk(float dTime);

    public void addTime(float dTime)
    {
        progressTime += dTime;
    }
    public void resetProgress()
    {
        progressTime = 0f;
    }

    // Force the attack into the begining of Recoil.
    public abstract bool cancelHard();

    // Put the attack on Cooldown and stop execution.
    public abstract bool cancelFull();
}
