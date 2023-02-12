using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : AbStage
{
    public override bool call(AbAttack myStuff)
    {
        // Nothing happens while on Cooldown so let the move selector know
        return false;
    }
}
