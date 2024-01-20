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

    [Header("Audio")]
    [Tooltip("Where I make noise from, bruv")]
    [SerializeField] protected AudioSource mySource;

    // Events to subscribe to
    public System.Action onDmg;
    public System.Action onDie;

    /// <summary>
    /// Recieve damage
    /// </summary>
    /// <param name="dmg">Damage recieved</param>
    /// <param name="hitblock">Sound the weapon makes if I defend it fully</param>
    /// <param name="hitMarker">Sound the weapon makes if I take damage</param>
    /// <returns>true if alive still</returns>
    public bool justDmg(float dmg, AudioClip hitMarker, AudioClip hitblock)
    {
        // Apply defense
        dmg -= genDefense;

        // Make sound of being hit
        if (dmg > 0)
        {
            makeNoise(hitMarker);
        }
        else
        {
            makeNoise(hitblock);
            // Ensure we don't deal negative damage and avoid unnecessary follow up
            return true;
        }

        // Apply damage
        hp -= dmg;
        onDmg?.Invoke();
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
    /// Only for use by Player Character
    /// </summary>
    /// <param name="dmg">incoming damage</param>
    /// <returns>true if alive still</returns>
    public bool justDmg(float dmg)
    {
        // Apply defense
        dmg -= genDefense;

        // Make sound of being hit
        if (dmg > 0)
        {
            repeatNoise();
        }
        else
        {
            return true;
        }

        // Apply damage
        hp -= dmg;
        onDmg?.Invoke();
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
    /// <param name="hitblock">Sound the weapon makes if I defend it fully</param>
    /// <param name="hitMarker">Sound the weapon makes if I take damage</param>
    public void dmgNdisplace
        (float dmg, Vector3 displace, float magnitude, AudioClip hitMarker, AudioClip hitblock)
    {
        // Take Damage
        if (!justDmg(dmg, hitMarker, hitblock))
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
    /// <param name="hitblock">Sound the weapon makes if I defend it fully</param>
    /// <param name="hitMarker">Sound the weapon makes if I take damage</param>
    public void dmgNrecoil(float dmg, Vector3 contact, AudioClip hitMarker, AudioClip hitblock)
    {
        if (!justDmg(dmg, hitMarker, hitblock))
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
    /// <param name="hitblock">Sound the weapon makes if I defend it fully</param>
    /// <param name="hitMarker">Sound the weapon makes if I take damage</param>
    public void dmgFull(float dmg, Vector3 contact, float magnitude, AudioClip hitMarker, AudioClip hitblock)
    {
        if (!justDmg(dmg, hitMarker, hitblock))
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

    protected void makeNoise(AudioClip noise)
    {
        // Slightly randomize pitch to avoid sounding toooo samey
        mySource.pitch = Random.value * 0.2f + 0.9f;
        mySource.PlayOneShot(noise);
    }
    protected void repeatNoise()
    {
        // Slightly randomize pitch to avoid sounding toooo samey
        mySource.pitch = Random.value * 0.2f + 0.9f;
        mySource.Play();
    }

    // Placeholder until implemented
    void die()
    {
        onDie?.Invoke();
        Destroy(gameObject, 1f);
    }
}
