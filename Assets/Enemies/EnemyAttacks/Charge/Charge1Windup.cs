using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "ScriptableObjects/Abstage/Charge/Windup", order = 2)]
public class Charge1Windup : AbStage
{
    /// <summary>
    /// Telegraph attack and prepare for action
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // add dTime here to prevent Cooldown overflow
        myStuff.addTime(dTime);
        // If windup is complete, move on to Execution
        if (myStuff.getProgressTime() > myStuff.getMaxWindup())
        {
            myStuff.startExecution(target);
        }
        else
        {
            // Turn towards the target
            Vector3 targetDirection = target - myStuff.rb.position;
            float step = myStuff.targettingTurnRate * dTime;
            Quaternion newRot = Quaternion.LookRotation(targetDirection);
            myStuff.rb.MoveRotation(Quaternion.Slerp(myStuff.rb.rotation, newRot, step));
        }

        // Not on Cooldown, let the move slector know
        return true;
    }
}
