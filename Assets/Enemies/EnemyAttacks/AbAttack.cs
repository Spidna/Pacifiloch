using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbAttack : MonoBehaviour
{
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

    [Tooltip("windup, execution, recoil, cooldown")]
    [SerializeField] protected AbStage curAtkStage;
    public AbStage getCurAtkStage()
    { return curAtkStage; }
    [Tooltip("How far the attack travels")]
    [SerializeField] protected float magnitude;

    public abstract bool atk(float dTime);

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
    public abstract bool cancelHard();

    /// <summary>
    /// Put the attack on Cooldown and stop execution.
    /// </summary>
    /// <returns></returns>
    public abstract bool cancelFull();

    public abstract void startWindup();
    public abstract void startExecution();
    public abstract void startRecoil();
    public abstract void startCooldown();

}
