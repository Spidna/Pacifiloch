using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{
    public float hp;
    public float maxHP;

    // Resistance types maybes

    /// <summary>
    /// Recieve damage
    /// </summary>
    /// <param name="dmg">Damage recieved</param>
    /// <returns>true if alive still</returns>
    public bool justDmg(float dmg)
    {
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
    /// <param name="displace">How I'm displaced</param>
    void dmgNdisplace(float dmg, Vector3 displace)
    {
        if(!justDmg(dmg))
        {
            return;
        }

       
    }
    /// <summary>
    /// Recieve damage and animate, but don't displace
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="direction"></param>
    void dmgNrecoil(float dmg, Vector3 direction)
    {
        if (!justDmg(dmg))
        {
            return;
        }

    }
    /// <summary>
    /// Recieve damage, displace, and animate
    /// </summary>
    /// <param name="dmg">Damage recieved</param>
    /// <param name="displace">Target for displacement, and animation direction</param>
    void dmgFull(float dmg, Vector3 displace)
    {
        if (!justDmg(dmg))
        {
            return;
        }

    }

    // Placeholder until implemented
    void die()
    {
        Debug.Log(this.gameObject + " died");
    }
}
