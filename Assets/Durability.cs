using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{
    public float hp;
    public float maxHP;

    // Resistance types maybes
    [Header("Damage Resistance")]
    [Tooltip("Applies to all damage")]
    [SerializeField] protected float genDefense;

    [Header("Animation")]
    [Tooltip("My Animator, for recoil")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rb;
    [Tooltip("Name for idle animation bool")]
    [SerializeField] protected string idleName;
    [Tooltip("Name for damage recoil animation trigger")]
    [SerializeField] protected string recoilName;

    /// <summary>
    /// Recieve damage
    /// </summary>
    /// <param name="dmg">Damage recieved</param>
    /// <returns>true if alive still</returns>
    public bool justDmg(float dmg)
    {
        Debug.Log(hp + " - " + dmg);

        // Apply defense
        dmg -= genDefense;
        // Apply damage
        hp -= dmg;
        // Check if fallen below 0
        if (hp < 0f)
        {
            die();
            // Let caller know I loved them
            return false;
        }
        return true;
    }
    /// <summary>
    /// Recieve damage and displacement, no animation
    /// </summary>
    /// <param name="dmg">Damage recieved, 0 to just push</param>
    /// <param name="displace">Where I'm struck in gloabal space</param>
    /// <param name="magnitude">How much I'm pushed</param>
    public void dmgNdisplace(float dmg, Vector3 displace, float magnitude)
    {
        // Take Damage
        if (!justDmg(dmg))
        {
            // Don't animate recoil if we're dead
            return;
        }


        // Displacement
        // calculate the direction to displace towards
        displace -= transform.position;
        displace = Vector3.Normalize(displace);
        displace *= magnitude;

        rb.AddForce(displace);
    }
    /// <summary>
    /// Recieve damage and animate, but don't displace
    /// </summary>
    /// <param name="dmg">Damage recieved, 0 to just push</param>
    /// <param name="contact">Where I'm struck in gloabal space</param>
    public void dmgNrecoil(float dmg, Vector3 contact)
    {
        if (!justDmg(dmg))
        {
            // Don't animate recoil if we're dead
            return;
        }

    }
    /// <summary>
    /// Recieve damage, displace, and animate
    /// </summary>
    /// <param name="dmg">Damage recieved, 0 to just push</param>
    /// <param name="contact">Where I'm struck in gloabal space</param>
    /// <param name="magnitude">How much I'm pushed</param>
    public void dmgFull(float dmg, Vector3 contact, float magnitude)
    {
        if (!justDmg(dmg))
        {
            // Don't animate recoil if we're dead
            return;
        }

    }

    /// <summary>
    /// Execute animations for damage recoil
    /// </summary>
    /// <param name="contact">Where I'm struck in gloabal space</param>
    protected void animateRecoil(Vector3 contact)
    {

    }

    // Placeholder until implemented
    void die()
    {
        Debug.Log(this.gameObject + " died");
    }
}
