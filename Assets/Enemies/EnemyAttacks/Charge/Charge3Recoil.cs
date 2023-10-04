using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge3Recoil : AbStage
{
    /// <summary>
    /// Harmlessly recover from the attack
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // added add dTime here to prevent Cooldown overflow
        myStuff.addTime(dTime);

        if (myStuff.getProgressTime() > myStuff.getMaxRecoil())
        {
            myStuff.startCooldown(target);
        }

        // Not on Cooldown, let the move slector know
        return true;
    }
}
