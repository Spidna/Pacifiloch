using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF = GlobalFunctions;

public class ThrustWeapon : MonoBehaviour
{
    [Tooltip("Class which handles colliders and their trigger checks")]
    [SerializeField] protected SingleHurtbox theHurt;
    [Tooltip("Current velocity of me.")]
    [SerializeField] protected Rigidbody rb;
    [Tooltip("Multiplied by sqrVelocity to calulate damage")]
    [SerializeField] public float power;
    [Tooltip("Basic damage added to my damage")]
    [SerializeField] public float baseDmg;
    [Tooltip("Multiplied by power to calculate shove distance")]
    [SerializeField] public float knockBack;
    [Tooltip("How much damage I theoretically will do this frame")]
    [SerializeField] protected float curDmg;
    [Tooltip("Where I was last frame (calcStall) frames")]
    protected Queue<Vector3> prevPosition;
    [Tooltip("How many frames of movement are considered for curDmg")]
    [SerializeField] protected int calcStall;

    [Header("SFX")]
    [Tooltip("Sound made when this weapon deals damage")]
    [SerializeField] public AudioClip HitMarker;
    [Tooltip("Sound made when this weapon hits but deals no damage")]
    [SerializeField] public AudioClip hitBlocked;



    //private void OnTriggerEnter(Collider other)
    //{
    //    // // Kill collision check if not an enemy
    //    // if (other.tag != "WildTarget")
    //    //     return;
    //
    //    
    //}

    // Start is called before the first frame update
    void Start()
    {
        setupOnTrigger();
        prepPositionQueue();
    }
    /// <summary>
    /// Setup all the functions that will be called when this weapon deals damage
    /// </summary>
    protected void setupOnTrigger()
    {
        theHurt.setupOnTrigger(dealDmg);
    }

    /// <summary>
    /// Call once/FixedUpdate to find how fast this weapon moved
    /// </summary>
    protected void calcDmg()
    {
        // Magnitude I travelled between frames
        curDmg =
            (GF.VecTimes((prevPosition.Dequeue() - transform.position), transform.up)
            ).magnitude;
        prevPosition.Enqueue(transform.position);
        // Times power to make damage
        curDmg *= power;
    }
    public float getDmg()
    {
        return curDmg;
    }
    /// <summary>
    /// Setup a base queue of recent positions
    /// </summary>
    protected void prepPositionQueue()
    {
        prevPosition = new Queue<Vector3>();
        for (int i = 0; i < calcStall; i++)
            prevPosition.Enqueue(transform.position);
    }

    /// <summary>
    /// Called by HurtBox when in contact with enemy
    /// </summary>
    /// <param name="target">Who is taking the damage</param>
    /// <param name="contactPoint">Where we made contact</param>
    public void dealDmg(Durability target, Vector3 contactPoint)
    {
        // 
        target.dmgNdisplace(getDmg(), contactPoint, knockBack, HitMarker, hitBlocked);

    }
    /// <summary>
    /// Check if the weapon is being thrust with enough force to be worth enabling
    /// </summary>
    protected void enough2Collide()
    {

        if (getDmg() > 1f)
        {
            // Add base damage to round it out
            curDmg += baseDmg;
            // Turn back on the damaging colliders on this weapon
            theHurt.EnableColliders();
        }
        else
        {
            // Not enough damage to be worth making collision checks
            theHurt.DisableColliders();
        }
    }


    void FixedUpdate()
    {
        // Calculate theoretical damage for this frame
        calcDmg();
        // Check if that's enough to enable hurtbox
        enough2Collide();
    }
}
