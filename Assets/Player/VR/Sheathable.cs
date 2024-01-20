using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheathable : MonoBehaviour
{
    [Tooltip("Usable Sheaths and relevant data")]
    [SerializeField] private SheathsList sheaths;
    [Tooltip("How far I travel /time to get to sheath")]
    [SerializeField] private float sheathForce = 0.2f;

    private int targetSheath = -1;

    [Header("Scene References")]
    [SerializeField] private GameObject me;
    [Tooltip("This may be Null, ensure that's handled")]
    [SerializeField] private ThrustWeapon weapon;
    [SerializeField] private Rigidbody rb;

    // Perform unsheathing when grabbed and sheathed
    public System.Action unSheath;

    // Methods to enact during Update()
    public System.Action onUpdate;

    private void Start()
    {
        me = this.gameObject;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.Log("No Rigidbody.");
        sheaths = FindObjectOfType<SheathsList>();
        if (sheaths == null)
            Debug.Log("No SheathsList in scene");

    }

    private void Update()
    {
        onUpdate?.Invoke();
    }

    public void sheathSucction()
    {
        Debug.Log(succTime);
        // Error Handling
        if (targetSheath < 0 || targetSheath > 3)
        {
            Debug.Log("Target sheath: " + targetSheath + " out of bounds.");
            if (unSheath != null)
                unSheath();
            else
            {
                Debug.Log("Attempted to sheath without sheath??");
                onUpdate -= sheathSucction;
            }
            succTime = 0f;
            return;
        }
        ///Error Handling

        // Grab transform of targetSheath to make accessing faster
        Transform sheathToBe = sheaths.Slot[targetSheath].transform;

        if (succTime > maxSuccTime)
        { // Snap & stick to sheath
            rb.isKinematic = true;
            transform.SetParent(sheathToBe, true);
            transform.position = sheathToBe.position;

            // Forget this function until sheathed again
            onUpdate -= sheathSucction;
            // Reset succtime
            succTime = 0f;
        }
        else // Move towards sheath and count succtime
        {
            succTime += Time.deltaTime;

            // calc distance from me to sheath
            Vector3 posDifference = sheathToBe.position - transform.position;
            // calc how far i want to move this frame
            Vector3 plannedMove = posDifference.normalized;
            plannedMove *= sheathForce * Time.deltaTime;
            // Accelerate
            plannedMove *= (1f + succTime);

            // If I'm gonna overshoot then just use difference
            if (plannedMove.sqrMagnitude > posDifference.sqrMagnitude)
            {
                plannedMove = posDifference;
            }
            // Make the movement
            transform.position += plannedMove;
        }
    }
    [Tooltip("How long since player released near sheath")]
    private float succTime;
    [Tooltip("How long until snapping to sheath")]
    [SerializeField] private float maxSuccTime = 0.13f;

    /// <summary>
    /// Calcs and checks done when this item is let go by an XR controller
    /// </summary>
    public void OnRelease()
    {
        float closestSheathDist = 0f;
        targetSheath = closestSheath(ref closestSheathDist);
        sheathAt(targetSheath, closestSheathDist);
        onRelease?.Invoke();
    }
    public System.Action onRelease;
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
        if (distance > sheaths.sucction)
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
        if (i >= sheaths.Slot.Count)
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

    public void wipeSheathIndex()
    {
        targetSheath = -1;
    }
}
