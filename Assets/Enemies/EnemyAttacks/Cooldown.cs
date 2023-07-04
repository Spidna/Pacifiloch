using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : AbStage
{
    // !TODO! remove this commented out section
    ///[Header("Values")]
    ///[Tooltip("starting cooldown time.")]
    ///[SerializeField] protected float maxCD;
    ///[Tooltip("current cooldown time.")]
    ///[SerializeField] protected float curCD;

    // For accessing progress time
    //[SerializeField] protected AbAttack myAtk;

    //Cooldown(ref AbAttack atk)
    //{
    //    myAtk = atk;
    //}

    //public bool addCD(float t)
    //{
    //    myAtk.progressTime -= t;

    //    if(myAtk.progressTime > 0)
    //    {
    //        // Cooldown timer should continue, so return false
    //        return false;
    //    }
    //    // Cooldown timer should end, so let counter know to remove me
    //    // from list
    //    myAtk.progressTime = myAtk.maxCooldown; // reset cooldown
    //    return true;
    //}



    public override bool call(AbAttack myStuff)
    {
        // Nothing happens while on Cooldown so let the move selector know
        return false;
    }
}
