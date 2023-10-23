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
    /// <returns>true if attack went off, false if it shouldn't</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // Nothing happens while on Cooldown so let the move selector know
        return false;
    }

    /// <summary>
    /// This will be called within AttackStage to make Cooldown continue counting. 
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of Cooldown</param>
    public override void countCD(AbAttack myStuff, float dTime)
    {
        myStuff.addTime(dTime);

        // End Cooldown if we surpass cooldown timer to avoid overflow
        if (myStuff.getProgressTime() > myStuff.getMaxCooldown())
        {
            myStuff.startSearch();
        }
    }
}
