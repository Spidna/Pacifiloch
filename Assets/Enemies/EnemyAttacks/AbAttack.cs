using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbAttack : MonoBehaviour
{
    // TODO Maybe move most references into atksync maybe
    [Header("References")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] protected Collider[] offenceBox;
    public Collider getOffenceBox(int index)
    {
        return offenceBox[index];
    }
    public void setOffenceBox(int index, bool state)
    {
        offenceBox[index].enabled = state;
    }
    public void disableOffenceBoxes()
    {
        for (int i = 0; i < offenceBox.Length; i++)
            offenceBox[i].enabled = false;
    }

    [SerializeField] public Animator animator;

    // Stored here because cooldown doesn't do anything
    [SerializeField] protected AbStage cooldown;
    [Tooltip("Check if attack is within range and such.")]
    [SerializeField] protected AbStage search;
    [Tooltip("Telegraphing.")]
    [SerializeField] protected AbStage windup;
    [Tooltip("The event that deals damage")]
    [SerializeField] protected AbStage execute;
    [SerializeField] protected AbStage recoil;
    [Tooltip("windup, execution, recoil, cooldown")]
    [SerializeField] public AbStage curAtkStage;
    [Header("Values")]
    [Tooltip("Time remaining on current attack stage")]
    [SerializeField] public float progressTime;
    public float getProgressTime()
    { return progressTime; }
    public void resetProgressTime()
    { progressTime = 0f; }

    [Tooltip("Duration of Windup in seconds")]
    [SerializeField] protected float maxWindup;
    public float getMaxWindup() { return maxWindup; }
    [Tooltip("Duration of Execution in seconds")]
    [SerializeField] protected float maxExecute;
    public float getMaxExecute() { return maxExecute; }
    [Tooltip("Duration of Recoil in seconds")]
    [SerializeField] protected float maxRecoil;
    public float getMaxRecoil() { return maxRecoil; }
    [Tooltip("Duration of Cooldown in seconds")]
    [SerializeField] protected float maxCooldown;
    public float getMaxCooldown() { return maxCooldown; }

    [Tooltip("How far the attack travels")]
    [SerializeField] protected float magnitude;
    public float getMagnitude() { return magnitude; }
    public float getRange() { return magnitude * 0.8f; }

    [Tooltip("How fast the attack moves the user")]
    public float advanceSpeed;
    [Tooltip("How fast the user turns while in Windup")]
    public float targettingTurnRate;
    [Tooltip("How aggressive the user homes in on the target during execution")]
    public float homingTurnRate;


    //[SerializeField] protected Vector3 curTarget;
    //public Vector3 getTarget()
    //{ return curTarget; }
    //public void updateTarget(Vector3 target)
    //{ curTarget = target; }
    //public void updateTarget(GameObject target)
    //{ curTarget = target.transform.position; }

    /// <summary>
    /// Attempt execution of current stage of attack sequence
    /// </summary>
    /// <param name="dTime">accepted DeltaTime</param>
    /// <returns>true if attack went off, false if it shouldn't</returns>
    public abstract bool atk(float dTime, Vector3 target);

    /// <summary>
    /// Accepts DeltaTime to add to progressTime
    /// </summary>
    public void addTime(float dTime)
    {
        progressTime += dTime;
    }

    /// <summary>
    /// Force the attack into the begining of Recoil.
    /// </summary>
    /// <returns></returns>
    public abstract bool cancelHard(Vector3 target);

    /// <summary>
    /// Put the attack on Cooldown and stop execution.
    /// </summary>
    /// <returns></returns>
    public abstract bool cancelFull(Vector3 target);

    public abstract void startSearch(Vector3 target);
    public abstract void startWindup(Vector3 target);
    public abstract void startExecution(Vector3 target);
    public abstract void startRecoil(Vector3 target);
    public abstract void startCooldown(Vector3 target);

    public abstract bool atkQuery(GameObject target);

}
