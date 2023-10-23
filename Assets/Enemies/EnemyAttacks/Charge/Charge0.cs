using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "ScriptableObjects/Abstage/Charge/Search", order = 1)]
public class Charge0 : AbStage
{
    /// <summary>
    /// Check if attack is in range and similar checks
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>false if on cooldown, true otherwise</returns>
    public override bool call(AbAttack myStuff, float dTime, Vector3 target)
    {
        // If target is within range, start the Widnup stage
        float distance = Vector3.Distance(myStuff.rb.transform.position, target);
        if (distance < myStuff.getRange())
        {
            myStuff.startWindup();
            // Attack is a go
            return true;
        }

        // Out of range, so no attack
        return false;
    }
}
