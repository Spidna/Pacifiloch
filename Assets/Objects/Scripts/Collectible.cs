using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider myCollider;


    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
