using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHurtbox : MonoBehaviour
{
    public event System.Action ToDoOnTrigger;
    public List<Collider> colliders;
    
    // Last enemy struck
    private GenEnemy victim;
    public GenEnemy getVictim() { return victim; }


    private void OnTriggerEnter(Collider other)
    {
        
    }


}
