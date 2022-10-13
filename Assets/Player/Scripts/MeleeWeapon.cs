using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Tooltip("For trigger checks")]
    [SerializeField] private Collider myCollider;

    private void OnTriggerEnter(Collider other)
    {
        // Kill collision check if not an enemy
        if (other.tag != "WildTarget")
            return;
        
        //other.TryGetComponent(GenEnemy, )
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
