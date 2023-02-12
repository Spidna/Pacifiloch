using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge1Windup : AbStage
{

    public override bool call(AbAttack myStuff)
    {


        // Not on Cooldown, let the move slector know
        return true;
    }
}
