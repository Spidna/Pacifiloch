using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheathable : MonoBehaviour
{
    [Tooltip("Usable Sheaths and relevant data")]
    [SerializeField] private SheathsList sheaths;

    [SerializeField] private GameObject me;
    [SerializeField] private ThrustWeapon weapon;

    // Perform unsheathing when grabbed and sheathed
    public System.Action unSheath;

    /// <summary>
    /// Calcs and checks done when this item is let go by an XR controller
    /// </summary>
    public void OnRelease()
    {
        float closestSheathDist = 0f;
        int targetSheath = closestSheath(ref closestSheathDist);
        sheathAt(targetSheath, closestSheathDist);
    }
    /// <summary>
    /// What to do when grabbed
    /// </summary>
    public void OnGrab()
    {
        // Attempt to unsheath
        unSheath?.Invoke();
    }

    /// <summary>
    /// Attempt to store this item at sheaths[i]
    /// </summary>
    /// <param name="index">Which slot to store in</param>
    /// <param name="distance">Distance squared between sheath and me</param>
    /// <returns>true if successful</returns>
    public bool sheathAt(int index, float distance)
    {
        // grab target sheath for quick access
        Sheath target = sheaths.Slot[index];

        // Check for scenerios that we cannot store here
        // Occupied?
        if (target.myContent != null)
        { return false; }
        // Close enough?
        if (distance > sheaths.sucction * sheaths.sucction)
        { return false; }


        // Attach to the sheath
        target.sheathHere(this);

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="closestDistance">reference to the SqrMagnitude of distance of closest sheath</param>
    /// <returns>The index of the closest sheath</returns>
    private int closestSheath(ref float closestDistance)
    {
        closestDistance = Vector3.SqrMagnitude(me.transform.position - sheaths.Slot[0].transform.position);

        return closestSheath(1, 0, ref closestDistance);
    }
    private int closestSheath(int i, int closestIndex, ref float closestDistance, float nextDistance = 0f)
    {
        if (i > sheaths.Slot.Count)
        { return closestIndex; }
        nextDistance = Vector3.SqrMagnitude(me.transform.position - sheaths.Slot[i].transform.position);

        // Update new closest
        if (closestDistance < nextDistance)
        {
            closestDistance = nextDistance;
            closestIndex = i;
        }

        return closestSheath(i + 1, closestIndex, ref closestDistance, nextDistance);
    }
}
