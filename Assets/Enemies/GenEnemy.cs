using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEnemy : MonoBehaviour
{
    [Header("Personal stats")]
    [Tooltip("Current hitpoints")]
    [SerializeField] float hp;
    [Tooltip("Maximum hitpoints")]
    [SerializeField] float maxHp;
    [Tooltip("Range to trigger attack from")]
    [SerializeField] float atkRange;
    [Tooltip("My acceleration speed when swimming")]
    [SerializeField] float moveSpeed;
    [Tooltip("My turn speed")]
    [SerializeField] float turnSpeed;

    [Header("References")]
    [Tooltip("My rigidbody")]
    [SerializeField] Rigidbody myRigidbody;
    [Tooltip("The player or dummy I'm targetting")]
    [SerializeField] GameObject targetP;

    [Tooltip("Distance from targetP")]
    private float targetDist = 0f;
    [Tooltip("EMPTY which buffers turns by leading this enemy.")]
    //[SerializeField] Rabbit _rabbit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // !TODO!
    // Redundant function that'll make future
    //* mechanics easier to implement
    void chooseTarget()
    {
        targetP = targetP;
    }

    // Decide what to do
    void decisions()
    {
        chooseTarget();
        // If the target is close enough to attack, then do so.
        targetDist = (transform.position
            - targetP.transform.position).magnitude;
        if (atkRange >= targetDist)
        {
            doAtkSimple();
        }
        // If not in range to attack, move towards target
        else
        {
            advance();
        }
    }

    // Initiate attack sequence
    void doAtkSimple()
    {
        // !TODO!
        // Implement attack animation and trigger
    }

    // Attempt to move closer to the target
    void advance() //!TODO! The turning doesn't even come close to working lol
    {
        

    }
    /*
      // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 1.0f;

    void Update()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
     * */

    // Update is called once per frame
    void Update()
    {
        decisions();

    }


}
