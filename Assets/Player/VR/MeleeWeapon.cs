using System.Collections;
using UnityEngine;
using GF = GlobalFunctions;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("Class which handles colliders and their trigger checks")]
    [SerializeField] protected SingleHurtbox theHurt;
    [Tooltip("Current velocity of me.")]
    [SerializeField] protected Rigidbody rb;
    [Tooltip("Multiplied by sqrVelocity to calulate damage")]
    [SerializeField] public float power;
    [Tooltip("Multiplied by power to calculate shove distance")]
    [SerializeField] public float knockBack;
    [Tooltip("How much damage I theoretically will do this frame")]
    [SerializeField] protected float curDmg;
    [Tooltip("Where I was last frame")]
    [SerializeField] protected Transform preTransform;

    //[Tooltip("How much min")]


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
        setPreTransform();
    }
    /// <summary>
    /// Setup all the functions that will be called when this weapon deals damage
    /// </summary>
    protected void setupOnTrigger()
    {
        theHurt.triggerEvents += dealDmg;
    }

    /// <summary>
    /// Call once/FixedUpdate to find how fast this weapon moved
    /// </summary>
    protected void calcDmg()
    {

        // sqrMagnitude I travelled between frames
        curDmg =
            (GF.VecTimes(preTransform.position, preTransform.up)
            - GF.VecTimes(transform.position, transform.up)
            ).magnitude;
        setPreTransform();
        // Times power to make damage
        curDmg *= power;
    }
    protected void setPreTransform()
    {
        preTransform.position = transform.position;
        preTransform.rotation = transform.rotation;
    }
    public float getDmg()
    {
        return curDmg;
    }


    public void dealDmg(Durability target, Vector3 contactPoint)
    {
        // 
        target.dmgNdisplace(getDmg(), contactPoint, knockBack);


    }

    private float bestDmg = 0f;
    protected void enough2Collide()
    {
        if (getDmg() > bestDmg)
        {
            bestDmg = getDmg();
            Debug.Log(bestDmg);
        }



        if (getDmg() > 1f)
        {
            theHurt.EnableColliders();
        }
        else
        {
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
