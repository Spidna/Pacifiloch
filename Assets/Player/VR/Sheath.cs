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
        // Cancel Attempted sheathing if occupied
        if (myContent != null)
            return;


        myContent = toStore;
        // Align transforms
        myContent.rb.isKinematic = true;
        myContent.transform.SetParent(this.transform, true);
        myContent.onUpdate += myContent.sheathSucction;

        // Tell Sheathable to do cleanup next time it's grabbed
        toStore.unSheath += unSheathHere;
    }

    public void unSheathHere()
    {
        // Don't execute this function until sheathed again
        myContent.unSheath -= unSheathHere;
        // Remove parenting
        myContent.transform.SetParent(null, true);
        // Tell sheathable to forget about me :,(
        myContent.wipeSheathIndex();
        // Clear this cuz I am scared of what happens if someone grabs
        // the weapon while it's being succ'd
        myContent.onUpdate -= myContent.sheathSucction;

        // Un store the sheathable
        myContent = null;
    }
}
