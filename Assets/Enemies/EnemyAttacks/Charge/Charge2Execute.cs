using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "ScriptableObjects/Abstage/Charge/Execute", order = 3)]
public class Charge2Execute : AbStage
{
    /// <summary>
    /// CHAAARGE!!
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // added add dTime here to prevent Cooldown overflow
        myStuff.addTime(dTime);
        // Check if the attack is done
        if (myStuff.getProgressTime() > myStuff.getMaxExecute())
        {
            myStuff.startRecoil();

            return true;
        }
        else
        {
            // Charge at target
            Vector3 force = myStuff.rb.transform.forward * dTime * myStuff.advanceSpeed;
            myStuff.rb.AddForce(force, ForceMode.Impulse);
        }

        // Not on Cooldown, let the move slector know
        return true;
    }
}
