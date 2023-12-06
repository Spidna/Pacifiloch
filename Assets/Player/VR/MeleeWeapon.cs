using System.Collections;
using UnityEngine;

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

    }
    /// <summary>
    /// Setup all the functions that will be called when this weapon deals damage
    /// </summary>
    private void setupOnTrigger()
    {
        theHurt.triggerEvents += dealDmg;
    }

    private float calcDamage()
    {
        return Vector3.SqrMagnitude(rb.velocity) * power;
    }

    public void dealDmg(Durability target, Vector3 contactPoint)
    {
        target.dmgNdisplace(calcDamage(), contactPoint, knockBack);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
