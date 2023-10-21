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
            myStuff.startRecoil(target);

            return true;
        }
        else
        {
            // Charge at target
            Vector3 moveVector = (myStuff.rb.transform.position - target).normalized;
            moveVector = moveVector.normalized * myStuff.advanceSpeed * -1f;
            myStuff.rb.transform.forward = moveVector;
            myStuff.rb.MovePosition(myStuff.transform.position + moveVector * Time.deltaTime);
        }

        //Vector3 chargeForce = myStuff.rb.transform.forward * 

        //myStuff.rb.AddForce();


        // Not on Cooldown, let the move slector know
        return true;
    }
}
