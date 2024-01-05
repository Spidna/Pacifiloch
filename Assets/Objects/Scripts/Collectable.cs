using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider myCollider;
    [Tooltip("Information that'll be stored in bag")]
    public ItemDetails myDetails;
    [Tooltip("How fast pullTo() will move me")]
    [SerializeField][Range(0f, 0.7f)] private float speed;
    [Tooltip("How tiny we'll shrink each fixedupdate()")]
    [SerializeField][Range(0.6f, 1f)] private float shrinkRate;
    [Tooltip("Where we're going in space once collected")]
    public GameObject target;

    public event System.Action OnCollected;

    private void Start()
    {
        target = null;
    }

    void FixedUpdate()
    {
        // Run pullTo() if I'm in the process of being stored
        OnCollected?.Invoke();
    }

    private void OnDestroy()
    {
        // Redundancy to make good practice for wiping OnCollected
        OnCollected = null;
    }

    /// <summary>
    /// Initiate animation to be collected
    /// </summary>
    /// <param name="_target">Where I'm going</param>
    public void startCollection(GameObject _target)
    {
        // Establish target for moving towards
        target = _target;
        // Disable colldier to avoid repeat calls of this
        myCollider.enabled = false;
        // Disable some physics to make animation prettier
        rb.drag = 0f;
        rb.useGravity = false;
        rb.Sleep();

        OnCollected += pullTo;
    }
    /// <summary>
    /// Animation of me being collected by a bag
    /// </summary>
    private void pullTo()
    {
        // Shrink a bit
        rb.transform.localScale *= shrinkRate;


        // Calculate the way we're moving
        Vector3 moveVector = target.transform.position - rb.position;

        // Calculate the distance to the target
        float distanceSqr = moveVector.sqrMagnitude;

        // Only use speed if it'll move less than the remaining distance
        if (distanceSqr > speed * speed)
        {
            // Normalize the moveVector and scale it by the calculated speed
            moveVector = moveVector.normalized * speed;
        }

        // Apply the force
        rb.MovePosition(rb.position + moveVector);

        // I considered wiping OnCollected here but the bag could move requiring the object to contiue following
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // Handled in Bag
    //}
}
