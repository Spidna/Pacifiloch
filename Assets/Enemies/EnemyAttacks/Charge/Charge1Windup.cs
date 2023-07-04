using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge1Windup : AbStage
{

    public override bool call(AbAttack myStuff)
    {
        //TODO Continue coding from here once animations implimented

        // Not on Cooldown, let the move slector know
        return true;
    }
}
