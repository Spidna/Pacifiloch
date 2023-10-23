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

    // TODO maybe move Awake() to AbAttack
    private void Awake()
    {
        // Be sure AtkStages are defined
        if (cooldown == null)
            Debug.LogWarning("Cooldown AtkStage undefined.");
        if (search == null)
            Debug.LogWarning("Search AtkStage undefined.");
        if (windup == null)
            Debug.LogWarning("Windup AtkStage undefined.");
        if (execute == null)
            Debug.LogWarning("Execute AtkStage undefined.");
        if (recoil == null)
            Debug.LogWarning("Recoil AtkStage undefined.");

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
                Debug.LogWarning("ERR: " + this + " missing offenceBox" + i);
            }
        }
    }

    private void Update()
    {
        // Increment CD counter, function does nothing if off Cooldown
        curAtkStage.countCD(this, Time.deltaTime);
    }

    /// <summary>
    /// Attempt execution of current stage of attack sequence
    /// </summary>
    /// <param name="dTime">accepted DeltaTime</param>
    /// <returns>true if attack went off, false if it shouldn't</returns>
    public override bool atk(float dTime, Vector3 target)
    {
        // True means attack progressed this tick
        // False means attack is unusable
        bool wentOff = curAtkStage.call(this, dTime, target);
        

        return wentOff;
    }

    public override void startSearch()
    {
        // Check if attack can reach target
        animator.SetBool("Swimming", true);

        resetProgressTime();
        curAtkStage = search;
    }
    public override void startWindup()
    {
        // Start Windup
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeWindup");

        resetProgressTime();
        curAtkStage = windup;
    }
    public override void startExecution()
    {
        // BRING THE PAIN
        resetProgressTime();
        animator.SetBool("Swimming", false);
        animator.SetTrigger("Charging");
        setOffenceBox(0, true);

        curAtkStage = execute;
    }
    public override void startRecoil()
    {
        // Recover from attack
        resetProgressTime();
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeRecoil");
        setOffenceBox(0, false);

        curAtkStage = recoil;
    }
    public override void startCooldown()
    {
        // Start cooldown
        resetProgressTime();
        animator.SetBool("Swimming", true);

        curAtkStage = cooldown;
    }

    /// <summary>
    /// Force the attack into the begining of Recoil
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelFull ()
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
