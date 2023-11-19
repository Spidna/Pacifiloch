using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("For trigger checks")]
    [SerializeField] private List<Collider> offenseColliders; // ! TODO probably redundant !


    // Last enemy struck
    private GenEnemy victim;

    private void OnTriggerEnter(Collider other)
    {
        // Kill collision check if not an enemy
        if (other.tag != "WildTarget")
            return;

        if (!other.TryGetComponent(out victim))
        {
            Debug.Log("ERROR: " + this + " struck " + other + " which has no GenEnemy script attached.");
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
