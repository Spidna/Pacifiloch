using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHurtbox : MonoBehaviour
{
    public event System.Action triggerEvents;
    [SerializeField] new private Collider collider;

    // Last enemy struck
    private GenEnemy victim;
    public GenEnemy getVictim() { return victim; }
    protected void setVictim(GenEnemy nVictim) { victim = nVictim; }

    /// <summary>
    /// Just in case triggerEvents needs to be called elsewhere cuz protection is weird
    /// </summary>
    public void TriggerEvents()
    {
        triggerEvents();
    }

    protected void OnTriggerEnter(Collider other)
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
        TriggerEvents();
    }

    // Enable or Disable all my hurtbox colliders
    public void EnableColliders()
    { collider.enabled = true; }
    public void DisableColliders()
    { collider.enabled = false; }

}
