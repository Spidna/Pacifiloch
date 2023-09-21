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
    // TODO: possibly move this group to AbAttack, these are abstractions anyway
    protected AbStage windup;
    protected AbStage execute;
    protected AbStage recoil;

    private void Awake()
    {
        // Assign functions for automatic calling
        cooldown = new Cooldown();
        windup = new Charge1Windup();
        execute = new Charge2Execute();
        recoil = new Charge3Recoil();

        // Start at cooldown, to prevent premature fires
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
        addTime(dTime); // figure out cooldown means illegal vs other stage progress

        bool stageDone = curAtkStage.call(this);
        // TODO: Move this nested if statement into the AtkStage classes to improve optimization
        if(stageDone) // If finished with current attack stage, begin next stage
        {
            if(curAtkStage == cooldown)
            { 
            }
            else if(curAtkStage == windup)
            { 
            }
            else if(curAtkStage == execute)
            { 
            }
            else if(curAtkStage == recoil)
            { 
            }
            else
            {
                Debug.Log(this + "curAtkStage has invalid value of: "
                    + curAtkStage);
            }
        }
        else if (curAtkStage == cooldown)
        {
            return false;
        }


        return true;
    }

    public override void startWindup()
    {
        // Start Windup
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeWindup");

        resetProgressTime();
        curAtkStage = windup;
        curAtkStage.call(this);
    }
    public override void startExecution()
    {
        // BRING THE PAIN
        animator.SetBool("Swimming", false);
        animator.SetTrigger("Charging");
        setOffenceBox(0, true);

        curAtkStage = execute;
        curAtkStage.call(this);
    }
    public override void startRecoil()
    {
        // Recover from attack
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeRecoil");
        setOffenceBox(0, false);

        curAtkStage = recoil;
        curAtkStage.call(this);
    }
    public override void startCooldown()
    {
        // Start cooldown
        animator.SetBool("Swimming", true);

        curAtkStage = cooldown;
        curAtkStage.call(this);
    }

    /// <summary>
    /// Force the attack into the begining of Recoil
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelFull()
    {
        startRecoil();
        disableOffenceBoxes();

        return true;
    }

    /// <summary>
    /// Put the attack on Cooldown and stop execution
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelHard()
    {
        startCooldown();
        disableOffenceBoxes();

        return true;
    }
}
