using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Required Components
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Animation?))]

public class Charge : AbAttack
{
    //[SerializeField] public Rigidbody rb;
    //float progressTime;

    // Abstage Cooldown has no behaviours so its already defined in parent class
    private AbStage windup;
    private AbStage execute;
    private AbStage recoil;

    private void Start()
    {
        windup = new Charge1Windup();
        execute = new Charge2Execute();
        recoil = new Charge3Recoil();
    }

    public override bool atk(ref AbAttack myStuff, float time)
    {
        addTime(time);

        return true;
    }

    // Force the attack into the begining of Recoil.
    public override bool cancelFull()
    {
        atkStage = recoil;
        resetProgress();

        return true;
    }

    // Put the attack on Cooldown and stop execution.
    public override bool cancelHard()
    {
        atkStage = cooldown;
        resetProgress();

        return true;
    }
}
