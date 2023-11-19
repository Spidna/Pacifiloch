using System;
using UnityEngine;

public class HurtOnTrigger : MonoBehaviour
{
    public event EventHandler ToDoOnTrigger;

    [SerializeField] private MeleeWeapon myWeapon;


}
