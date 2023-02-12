using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbAttack : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;

    [SerializeField] protected AbStage cooldown;


    [Tooltip("windup, execution, recoil, cooldown")]
    [SerializeField] protected AbStage atkStage;
    [Tooltip("Time remaining on current attack stage")]
    [SerializeField] protected float progressTime;
    [Tooltip("How far the attack travels")]
    [SerializeField] protected float magnitude;

    public abstract bool atk(ref AbAttack myStuff, float time);

    public void addTime(float time)
    {
        progressTime += time;
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
