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
    [Header("References")]
    [SerializeField] protected AbStage search;
    [SerializeField] protected AbStage windup;
    [SerializeField] protected AbStage execute;
    [SerializeField] protected AbStage recoil;

    private void Awake()
    {
        // Assign functions for automatic calling
        cooldown = new Cooldown();
        search0 = new Charge0();
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
    public override bool atk(float dTime, Vector3 target)
    {
        // TODO RELOCATE ADDTIME INTO EACH ATKSTAGE EXCEPT 0
        addTime(dTime);
        // True means attack progressed this tick
        // False means attack is unusable
        bool wentOff = curAtkStage.call(this, dTime, target);
        

        return wentOff;
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
        animator.SetBool("Swimming", false);
        animator.SetTrigger("Charging");
        setOffenceBox(0, true);

        curAtkStage = execute;
        curAtkStage.call(this, 0f, target);
    }
    public override void startRecoil(Vector3 target)
    {
        // Recover from attack
        animator.SetBool("Swimming", false);
        animator.SetTrigger("ChargeRecoil");
        setOffenceBox(0, false);

        curAtkStage = recoil;
        curAtkStage.call(this, 0f, target);
    }
    public override void startCooldown(Vector3 target)
    {
        // Start cooldown
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
