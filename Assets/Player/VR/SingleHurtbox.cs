using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHurtbox : MonoBehaviour
{
    public event System.Action<Durability, Vector3> triggerEvents;

    [Tooltip("Pen Pineapple Apple Pen")]
    [SerializeField] new private Collider collider;
    [Tooltip("Have I been disabled from outside?")]
    [SerializeField] public bool exDisabled;
    [Tooltip("How much longer this hurtbox is paused.")]
    [SerializeField] private float pauseLeft;
    [Tooltip("How long this weapon pauses between damage")]
    [SerializeField] private float dmgPause;

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tag: " + other.tag);
        // Do nothing unless an enemy
        if (other.tag != "WildTarget")
            return;

        // Collect GenEnemy component for functions to deal damage
        if (!other.TryGetComponent(out victim))
        {
            Debug.Log("ERROR: " + this + " struck " + other + " which has no Durability script attached.");
            return;
        }

        // Pause my damage output if I cannot cleave
        pauseIfNoCleave?.Invoke();

        // Gather contactpoint
        Vector3 contactPoint = other.ClosestPoint(collider.transform.position);
        // Execute collected events
        TriggerEvents(victim, contactPoint);
    }


    private void FixedUpdate()
    {
        // Count down collider pause if paused
        colliderCD?.Invoke();
    }

    // Enable or Disable all my hurtbox colliders
    /// <summary>
    /// Only enables colliders if they aren't on cooldown
    /// </summary>
    public virtual void EnableColliders()
    {
        if (pauseLeft <= 0f)
        {
            pauseLeft = 0f;
            collider.enabled = true;
            // stop counting down collider cooldown
            colliderCD = null;
        }
        else
        {
            // Redundancy to ensure the cooldown is called
            colliderCD = ColliderCD;
        }

    }
    public virtual void DisableColliders()
    { collider.enabled = false; }

    // Temporarily disable colliders, like when a weapon is incapable of cleaving
    [Tooltip("Pause hurtbox if I can't cleave, do nothing otherwise")]
    private event System.Action pauseIfNoCleave;
    public void EnableCleave()
    { pauseIfNoCleave = null; }
    /// <summary>
    /// Make this hurtbox disable itself temporarily upon dealing damage
    /// </summary>
    /// <param name="dmgCD">How long I pause my damage output between strikes</param>
    public void DisableCleave(float dmgCD)
    {
        dmgPause = dmgCD;
        pauseIfNoCleave = pauseCollider;
    }
    private void pauseCollider()
    {
        // Begin countdown for collider reenable
        colliderCD = ColliderCD;
        pauseLeft = dmgPause;


        DisableColliders();
    }
    [Tooltip("Carries corresponding function if we're on CD")]
    private event System.Action colliderCD;
    /// <summary>
    /// Count down collider pause time
    /// </summary>
    private void ColliderCD()
    {
        pauseLeft -= Time.deltaTime;

        EnableColliders();
    }
}
