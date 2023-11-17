using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheathable : MonoBehaviour
{
    [Tooltip("Usable Sheaths and relevant data")]
    [SerializeField] private SheathsList sheaths;

    [SerializeField] private GameObject me;

    /// <summary>
    /// Calcs and checks done when this item is let go by an XR controller
    /// </summary>
    public void OnRelease()
    {
        float closestSheathDist = 0f;
        int targetSheath = closestSheath(ref closestSheathDist);
    }

    /// <summary>
    /// Store this item at sheaths[i]
    /// </summary>
    /// <param name="index">Which slot to store in</param>
    /// <returns>true if successful</returns>
    public bool sheathAt(int index)
    {

        return false;
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

        return closestSheath(i+1, closestIndex, ref closestDistance, nextDistance);
    }
}
