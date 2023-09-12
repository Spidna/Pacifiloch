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
    public void setOffenceBox (int index, bool state)
    {
        offenceBox[index].enabled = state;
    }
    public void disableOffenceBoxes()
    {
        for (int i = 0; i < offenceBox.Length; i++)
            offenceBox[i].enabled = false;
    }

    // Stored here because cooldown doesn't do anything
    [SerializeField] protected AbStage cooldown;

    [Header("Values")]
    [Tooltip("Time remaining on current attack stage")]
    [SerializeField] public float progressTime;

    [Tooltip("Duration of Windup in seconds")]
    [SerializeField] protected float maxWindup;
    [Tooltip("Duration of Execution in seconds")]
    [SerializeField] protected float maxExecute;
    [Tooltip("Duration of Recoil in seconds")]
    [SerializeField] protected float maxRecoil;
    [Tooltip("Duration of Cooldown in seconds")]
    [SerializeField] protected float maxCooldown;

    [Tooltip("windup, execution, recoil, cooldown")]
    [SerializeField] protected AbStage curAtkStage;
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
    /// Set progressTime to 0
    /// </summary>
    public void resetProgress()
    {
        progressTime = 0f;
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
}
