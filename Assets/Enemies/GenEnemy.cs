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

    [Tooltip("Distance from targetP")]
    private float targetDist = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Decide what to do
    void decisions()
    {
        // If the target is close enough to attack, then do so.
        targetDist = (transform.position - moveScript.target.transform.position).magnitude;
        if (_atkRange >= targetDist)
        {
            doAtkSimple();
        }
        // If not in range to attack, move towards target
        else
        {
            moveScript.Move();
        }
    }

    // Initiate attack sequence
    void doAtkSimple()
    {
        // !TODO!
        // Implement attack animation and trigger
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        decisions();
    }


}