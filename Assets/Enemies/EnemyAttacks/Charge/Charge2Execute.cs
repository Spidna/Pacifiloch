using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge2Execute : AbStage
{
    /// <summary>
    /// Abstraction which can be used to call whatever attack stage is in progress
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff)
    {
        // Check if the attack is done
        if (myStuff.getProgressTime() > myStuff.getMaxExecute())
        {
            myStuff.startRecoil();

            return true;
        }

        //Vector3 chargeForce = myStuff.rb.transform.forward * 

        //myStuff.rb.AddForce();


        // Not on Cooldown, let the move slector know
        return true;
    }
}
