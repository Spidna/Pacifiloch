using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "ScriptableObjects/Abstage/Cooldown", order = 1)]
public class Cooldown : AbStage
{
    /// <summary>
    /// Abstraction which can be used to call whatever attack stage is in progress
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // added add dTime here to prevent Cooldown overflow
        myStuff.addTime(dTime);


        if (myStuff.getProgressTime() > myStuff.getMaxCooldown())
        {
            // End Cooldown
            return true;
        }
        else // Still on cooldown
        {
            // Nothing happens while on Cooldown so let the move selector know
            return false;
        }
    }
}
