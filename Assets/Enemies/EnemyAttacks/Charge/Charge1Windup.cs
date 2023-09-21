using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge1Windup : AbStage
{
    /// <summary>
    /// Abstraction which can be used to call whatever attack stage is in progress
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff)
    {
        if (myStuff.getProgressTime() > myStuff.getMaxWindup())
        {
            myStuff.startExecution();
        }

        // Not on Cooldown, let the move slector know
        return true;
    }
}
