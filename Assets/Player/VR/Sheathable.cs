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
    [Tooltip("Where my anchor is for moving to sheath, same as reverse grip")]
    [SerializeField] private Transform gripTransform;
    [Tooltip("This may be Null, ensure that's handled")]
    [SerializeField] private ThrustWeapon weapon;
    [SerializeField] public Rigidbody rb;

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
            //rb.isKinematic = true;
            //transform.SetParent(sheathToBe, true);
            //transform.position = sheathToBe.position;

            // Forget this function until sheathed again
            onUpdate -= sheathSucction;
            // Reset succtime
            succTime = 0f;
        }
        else // Move towards sheath and count succtime
        {
            // Calculate Lerp progress
            succTime += Time.deltaTime;
            float t = Mathf.Clamp01(succTime / maxSuccTime);

            // Calculate how far is left to move
            Vector3 posSum = gripTransform.localPosition    ;
            Quaternion rotSum = gripTransform.localRotation ;


            // Lerp
            transform.localPosition = Vector3.Lerp(transform.localPosition, posSum, t);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotSum, t);
        }
    }
    [Tooltip("How long since player released near sheath")]
    private float succTime;
    [Tooltip("How long until snapping to sheath")]
    [SerializeField] private float maxSuccTime = 0.2f;

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
    /// Find the closest sheath and how far it is
    /// </summary>
    /// <param name="closestDistance">reference to the SqrMagnitude of distance of closest sheath</param>
    /// <returns>The index of the closest sheath</returns>
    private int closestSheath(ref float closestDistance)
    {
        float nextDistance; // How close the currently checked sheath is
        int closestIndex = 0; // Which sheath is currently the closest
        // Calc i = 0  before the loop to ensure we have a baseline value
        closestDistance = Vector3.SqrMagnitude(me.transform.position - sheaths.Slot[0].transform.position);
        // Loop thru the rest of the Slots to find the closest
        for (int i = 1; i < sheaths.Slot.Count; i++)
        {
            // Find how close this sheath is
            nextDistance = Vector3.SqrMagnitude(me.transform.position - sheaths.Slot[i].transform.position);
            // Update closest sheath if this sheath is closer
            if (closestDistance > nextDistance)
            {
                closestDistance = nextDistance;
                closestIndex = i;
            }
        }

        // closestDistance is a reference so it's already stored
        return closestIndex;
    }

    public void wipeSheathIndex()
    {
        targetSheath = -1;
    }
}
