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
    //public event System.Action triggerEvents;
    [Tooltip("This class but stores individual hitboxes instead")]
    [SerializeField] private List<SingleHurtbox> colliders;

    // Last enemy struck
    //private GenEnemy victim;
    //public GenEnemy getVictim() { return victim; }

    // This is handled in parent class
    ///new protected void OnTriggerEnter(Collider other)
    ///{
    ///    // Do nothing unless an enemy
    ///    if (other.tag != "WildTarget")
    ///        return;
    ///
    ///    // Collect GenEnemy component for functions to deal damage
    ///    GenEnemy nVictim;
    ///    if (!other.TryGetComponent(out nVictim))
    ///    {
    ///        Debug.Log("ERROR: " + this + " struck " + other + " which has no GenEnemy script attached.");
    ///        return;
    ///    }
    ///    setVictim(nVictim);
    ///
    ///
    ///    // Execute collected events
    ///    OnTrigger();
    ///}

    // Enable or Disable all my hurtbox colliders
    public override void EnableColliders()
    {
        // Don't bother looping through alllll the colliders if we're already in agreement
        if (exDisabled == false)
            return;

        exDisabled = false;
        foreach (SingleHurtbox col in colliders)
        {
            col.EnableColliders();
            col.exDisabled = false;
        }
    }
    public override void DisableColliders()
    {
        // Don't bother looping through alllll the colliders if we're already in agreement
        if (exDisabled == true)
            return;

        exDisabled = true;
        foreach (SingleHurtbox col in colliders)
        {
            col.DisableColliders();
            col.exDisabled = true;
        }
    }

    new public void EnableCleave()
    {
        foreach(SingleHurtbox col in colliders)
            col.EnableCleave();
    }
    /// <summary>
    /// Make this hurtbox disable itself temporarily upon dealing damage
    /// </summary>
    /// <param name="dmgCD">How long I pause my damage output between strikes</param>
    new public void DisableCleave(float dmgCD)
    {
        foreach(SingleHurtbox col in colliders)
        {
            DisableCleave(dmgCD);
        }
    }
}
