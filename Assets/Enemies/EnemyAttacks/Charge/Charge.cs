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

    /// <summary>
    /// Attempt execution of current stage of attack sequence
    /// </summary>
    /// <param name="dTime">accepted DeltaTime</param>
    /// <returns>true if attack went off, false if it shouldn't</returns>
    public override bool atk(float dTime, Vector3 target)
    {
        // TODO RELOCATE ADDTIME INTO EACH ATKSTAGE EXCEPT 0
        addTime(dTime);
        // True means attack progressed this tick
        // False means attack is unusable
        bool wentOff = curAtkStage.call(this, dTime, target);
        

        return wentOff;
    }

    public override void startSearch(Vector3 target)
    {
        // Check if attack can reach target
        animator.SetBool("Swimming", true);

        resetProgressTime();
        curAtkStage = search;
        curAtkStage.call(this, 0f, target);
    }
    public override void startWindup(Vector3 target)
    {
        // Start Windup
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeWindup");

        resetProgressTime();
        curAtkStage = windup;
        curAtkStage.call(this, 0f, target);
    }
    public override void startExecution(Vector3 target)
    {
        // BRING THE PAIN
        resetProgressTime();
        animator.SetBool("Swimming", false);
        animator.SetTrigger("Charging");
        setOffenceBox(0, true);

        curAtkStage = execute;
        curAtkStage.call(this, 0f, target);
    }
    public override void startRecoil(Vector3 target)
    {
        // Recover from attack
        resetProgressTime();
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeRecoil");
        setOffenceBox(0, false);

        curAtkStage = recoil;
        curAtkStage.call(this, 0f, target);
    }
    public override void startCooldown(Vector3 target)
    {
        // Start cooldown
        resetProgressTime();
        animator.SetBool("Swimming", true);

        curAtkStage = cooldown;
        curAtkStage.call(this, 0f, target);
    }

    /// <summary>
    /// Force the attack into the begining of Recoil
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelFull (Vector3 target)
    {
        startRecoil(target);
        disableOffenceBoxes();

        return true;
    }

    /// <summary>
    /// Put the attack on Cooldown and stop execution
    /// </summary>
    /// <returns>true if successful</returns>
    public override bool cancelHard(Vector3 target)
    {
        startCooldown(target);
        disableOffenceBoxes();

        return true;
    }

    public override bool atkQuery(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
