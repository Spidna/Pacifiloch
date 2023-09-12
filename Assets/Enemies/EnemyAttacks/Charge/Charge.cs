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
        // Assign functions for automatic calling
        cooldown = new Cooldown();
        windup = new Charge1Windup();
        execute = new Charge2Execute();
        recoil = new Charge3Recoil();

        // Start at cooldown, 3 seconds to prevent premature fires
        progressTime = 0f;
        curAtkStage = cooldown;

        // Be sure offenceBox is disabled at start
        for (int i = 0; i < offenceBox.Length; i++)
        {
            try // easy to forget to assign hitbox so catch this exception
            {
                offenceBox[i].enabled = false;
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("ERR: " + this + " missing offenceBox" + i);
            }
        }
    }

    /// <summary>
    /// Attempt execution of current stage of attack sequence
    /// </summary>
    /// <param name="dTime">accepted DeltaTime</param>
    /// <returns>true if attack went off, false if it shouldn't</returns>
    public override bool atk(float dTime)
    {
        addTime(dTime);

        curAtkStage.call(this);

        return true;
    }

    /// <summary>
    /// Force the attack into the begining of Recoil
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelFull()
    {
        curAtkStage = recoil;
        resetProgress();

        return true;
    }

    /// <summary>
    /// Put the attack on Cooldown and stop execution
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelHard()
    {
        curAtkStage = cooldown;
        resetProgress();

        return true;
    }
}
