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
        myContent = toStore;
        // Align transforms
        toStore.transform.SetParent(this.transform);

        // Tell Sheathable to do cleanup next time it's grabbed
        toStore.unSheath += unSheathHere;
    }

    public void unSheathHere()
    {
        // Don't execute this function until sheathed again
        myContent.unSheath -= unSheathHere;
        // Un store the sheathable
        myContent = null;

    }
}
