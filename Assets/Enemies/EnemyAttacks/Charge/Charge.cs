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

    private void Awake()
    {
        cooldown = new Cooldown();
        windup = new Charge1Windup();
        execute = new Charge2Execute();
        recoil = new Charge3Recoil();

        progressTime = 0f;
        curAtkStage = cooldown;
    }

    // Return true if attack went off, false if it shouldn't
    public override bool atk(float dTime)
    {
        addTime(dTime);

        curAtkStage.call(this);

        return true;
    }

    // Force the attack into the begining of Recoil.
    public override bool cancelFull()
    {
        curAtkStage = recoil;
        resetProgress();

        return true;
    }

    // Put the attack on Cooldown and stop execution.
    public override bool cancelHard()
    {
        curAtkStage = cooldown;
        resetProgress();

        return true;
    }
}
