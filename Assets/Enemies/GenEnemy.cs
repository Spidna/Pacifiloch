using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEnemy : MonoBehaviour
{
    [Header("Personal stats")]
    [Tooltip("Current hitpoints")]
    [SerializeField] private float _hp;
    [Tooltip("Maximum hitpoints")]
    [SerializeField] private float _maxHp;
    [Tooltip("Range to trigger attack from")]
    [SerializeField] private float _atkRange;

    [Header("References")]
    [Tooltip("Abstraction, My movement style")]
    [SerializeField] private WildMovement moveScript;
    [Tooltip("Abstraction, My attacks")]
    [SerializeField] private List<AbAttack> myAttacks;
    [Tooltip("Weighting for attack likelihood")]
    [SerializeField] private List<float> atkWeights;
    [Tooltip("Sum of atkWeights")]
    [SerializeField] private float weightSum;

    [Tooltip("Distance from targetP")]
    private float targetDist = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Decide what to do
    void decisions()
    {
        // Calculate distance to target
        targetDist = (transform.position - moveScript.target.transform.position).magnitude;

        // Check attack triggers and return false if an attack is used
        if (pickAtk(moveScript.assignedFlock.atkSync))
            // If unable to attack, move towards target
            moveScript.Move();
    }

    // Initiate attack sequence, return true if movement behaviour should continue
    private float weightProgress;
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
                // Check if attack should activate
                if()
                {

                }
            }
        }/// Check which attack should be triggered

        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        decisions();
    }


}