using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbStage // :Monobehaviour
{
    /// <summary>
    /// Abstraction which can be used to call whatever attack stage is in progress
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <returns>true if successful & continues, false if stage is over</returns>
    public abstract bool call(AbAttack myStuff);
}
