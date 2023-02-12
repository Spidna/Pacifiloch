using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge3Recoil : AbStage
{
    public override bool call(AbAttack myStuff)
    {



        // Not on Cooldown, let the move slector know
        return true;
    }
}
