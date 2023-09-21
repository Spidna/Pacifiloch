using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEnemy : MonoBehaviour
{
    [Header("Personal stats")]
    [Tooltip("Current hitpoints")]
    [SerializeField] protected float _hp;
    [Tooltip("Maximum hitpoints")]
    [SerializeField] protected float _maxHp;
    [Tooltip("Range to trigger attack from")]
    [SerializeField] protected float _atkRange;

    [Header("References")]
    [Tooltip("Abstraction, My movement style")]
    [SerializeField] protected WildMovement moveScript;
    [Tooltip("Abstraction, My attacks")]
    [SerializeField] protected List<AbAttack> myAttacks;
    [Tooltip("Abstraction, attack In Progress")]
    [SerializeField] protected AbAttack atkIP;
    [Tooltip("Weighting for attack likelihood")]
    [SerializeField] protected List<float> atkWeights;
    [Tooltip("Sum of atkWeights")]
    [SerializeField] protected float weightSum;

    [Tooltip("Distance from targetP")]
    protected float targetDist = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Decide between movement, attacking, or idling behaviours
    /// </summary>
    void decisions()
    {
        // Calculate distance to target
        targetDist = (transform.position - moveScript.assignedFlock.target.transform.position).magnitude;
        // TODO 
        // If an attack is in progress, continue its behaviour
        if (atkIP != null)
        {
            if (atkIP.atk(Time.deltaTime))
            {

            }
        }
        // No attack in progress: Check if an attack should be executed
        else
        {
            // Check attack triggers and recieve false if an attack is used
            if (pickAtk(moveScript.assignedFlock.atkSync))
            {
                // If unable to attack, move towards target
                moveScript.Move();
            }
        }
    }

    // Initiate attack sequence, return false if movement behaviour should continue
    protected float weightProgress;
    /// <summary>
    /// Pick an attack based on a collective metronome
    /// </summary>
    /// <param name="_atkSync">
    /// Information that allows schooling enemies to act together
    /// </param>
    /// <returns>true if successful, false if illegal</returns>
    bool pickAtk(AtkSync _atkSync)
    {
        weightProgress = 0f;
        // !TODO!
        // Implement attack animation and trigger

        // Check which attack should be triggered
        for (int i = 0; i < atkWeights.Count; i++)
        {
            // Check AtkSync time to sync attack patterns
            weightProgress += atkWeights[i];
            if (_atkSync.t < weightProgress)
            {
                // Check if attack should based on cd, range, etc
                if (myAttacks[i].atk(Time.deltaTime))
                {
                    // Set attackInProgress so next frame the attack
                    // behaviour can continue, bypassing pickAtk()
                    atkIP = myAttacks[i];
                    return false; // If attack went off, exit and return false
                }
                else
                {
                    // If attack can't activate, exit and return true
                    // so movement can resume
                    return true;
                }
            }
        }/// Check which attack should be triggered

        return true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        decisions();
    }


}