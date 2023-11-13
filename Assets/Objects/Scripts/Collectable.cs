using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider myCollider;
    [Tooltip("Information that'll be stored in bag")]
    public ItemDetails myDetails;


    private void Start()
    {
    }

    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handled in Bag
    }
}
