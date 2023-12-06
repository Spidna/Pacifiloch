using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHurtbox : MonoBehaviour
{
    public event System.Action<Durability, Vector3> triggerEvents;
    [SerializeField] new private Collider collider;

    // Last enemy struck
    private Durability victim;
    public Durability getVictim() { return victim; }
    protected void setVictim(Durability nVictim) { victim = nVictim; }

    /// <summary>
    /// Just in case triggerEvents needs to be called elsewhere cuz protection is weird
    /// </summary>
    public void TriggerEvents(Durability targetHP, Vector3 contactPoint)
    {
        triggerEvents(targetHP, contactPoint);
    }
    protected void TriggerEvents(Vector3 contactPoint)
    {
        triggerEvents(victim, contactPoint);
    }

    protected void OnTriggerEnter(Collider other)
    {
        // Do nothing unless an enemy
        if (other.tag != "WildTarget")
            return;

        // Collect GenEnemy component for functions to deal damage
        if (!other.TryGetComponent(out victim))
        {
            Debug.Log("ERROR: " + this + " struck " + other + " which has no Durability script attached.");
            return;
        }

        Vector3 contactPoint = other.ClosestPoint(collider.transform.position);

        // Execute collected events
        TriggerEvents(victim, contactPoint);
    }

    // Enable or Disable all my hurtbox colliders
    public void EnableColliders()
    { collider.enabled = true; }
    public void DisableColliders()
    { collider.enabled = false; }

}
