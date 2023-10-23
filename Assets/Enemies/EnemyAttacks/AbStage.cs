using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data",
//    menuName = "ScriptableObjects/Abstage", order = 2)]
public abstract class AbStage : ScriptableObject
{
    /// <summary>
    /// Abstraction which can be used to call whatever attack stage is in progress
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of atk</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    /// <returns>true if successful & continues, false if stage is over</returns>
    public abstract bool call(AbAttack myStuff, float dTime, Vector3 target);

    /// <summary>
    /// This will be called within AttackStage to make Cooldown continue counting. 
    /// </summary>
    /// <param name="myStuff">Information from attack who calls this</param>
    /// <param name="dTime">Update progress of Cooldown</param>
    /// <param name="target">Unused half the time cuz target unimportant when breathing</param>
    public virtual void countCD(AbAttack myStuff, float dTime)
    { 
        // Do nothing unless on Cooldown
    }
}
