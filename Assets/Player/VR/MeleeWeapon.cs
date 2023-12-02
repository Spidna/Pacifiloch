using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("Class which handles colliders and their trigger checks")]
    [SerializeField] private SingleHurtbox theHurt;

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

    }
    /// <summary>
    /// Setup all the functions that will be called when this weapon deals damage
    /// </summary>
    private void setupOnTrigger()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
