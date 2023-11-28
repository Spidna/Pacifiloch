using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Parent script for player's Hurtbox behaviour
/// Only deviation I can think of is the Single Collider type vs Multi Collider
/// </summary>
public class MultiHurtbox : SingleHurtbox
{
    //public event System.Action ToDoOnTrigger;
    //public List<Collider> colliders;

    // Last enemy struck
    //private GenEnemy victim;
    public GenEnemy getVictim() { return victim; }

    private void OnTriggerEnter(Collider other)
    {
        // Do nothing unless an enemy
        if (other.tag != "WildTarget")
            return;

        // Collect GenEnemy component for functions to deal damage
        if (!other.TryGetComponent(out victim))
        {
            Debug.Log("ERROR: " + this + " struck " + other + " which has no GenEnemy script attached.");
            return;
        }


        // Execute collected events
        ToDoOnTrigger();
    }

    // Enable or Disable all my hurtbox colliders
    public void EnableColliders()
    {
        foreach(Collider col in colliders)
        {
            col.enabled = true;
        }
    }
    public void DisableColliders()
    {
        foreach(Collider col in colliders)
        {
            col.enabled = false;
        }
    }
}
