using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheath : MonoBehaviour
{
    //public Transform transform; Available thru MonoBehaviour
    // We'll use the rotation of my transform to calculate the angle we store at
    [Tooltip("What is stored in this sheath")]
    public Sheathable myContent;

    /// <summary>
    /// Store the passed object in this sheath
    /// </summary>
    /// <param name="toStore">What is being stored here</param>
    public void sheathHere(Sheathable toStore)
    {

    }

    public void unSheathHere()
    {

    }
}
