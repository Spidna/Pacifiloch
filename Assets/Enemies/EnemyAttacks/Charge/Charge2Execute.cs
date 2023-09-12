using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge2Execute : AbStage
{
    public override bool call(AbAttack myStuff)
    {
        myStuff.setOffenceBox(0, true);

        //Vector3 chargeForce = myStuff.rb.transform.forward * 

        //myStuff.rb.AddForce();


        // Not on Cooldown, let the move slector know
        return true;
    }
}
