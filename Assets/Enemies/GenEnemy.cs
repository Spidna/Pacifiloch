using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenEnemy : MonoBehaviour
{
    [Header("Personal stats")]
    [Tooltip("Current hitpoints")]
    [SerializeField] private float _hp;
    [Tooltip("Maximum hitpoints")]
    [SerializeField] private float _maxHp;
    [Tooltip("Range to trigger attack from")]
    [SerializeField] private float _atkRange;
    [Tooltip("My acceleration speed when swimming")]
    [SerializeField] private float _moveSpeed;
    [Tooltip("My turn speed")]
    [SerializeField] private float _turnSpeed;

    [Header("References")]
    [Tooltip("My rigidbody")]
    [SerializeField] private Rigidbody myRigidbody;
    public Rigidbody rigidbody { get { return myRigidbody; } }
    [Tooltip("The player or dummy I'm targetting")]
    [SerializeField] private GameObject _targetP;

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
        _targetP = _targetP;
    }

    // Decide what to do
    void decisions()
    {
        chooseTarget();
        // If the target is close enough to attack, then do so.
        targetDist = (transform.position
            - _targetP.transform.position).magnitude;
        if (_atkRange >= targetDist)
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
        //decisions();
    }


}

/*
public class TestVerticalMoveForward : MonoBehaviourExt
{
    private GameObject from;

    private GameObject to;

    private GameObject target;
    private GameObject center;

    // Use this for initialization
    void Start()
    {
        from = GameObject.Find("From");
        to = GameObject.Find("To");
        center = GameObject.Find("Character/Center");
        target = from;
    }

    private float speed = 10;

    // Update is called once per frame
    void Update()
    {
        Watch("center.transform.position", transform.position);
        var offset = target.transform.position - transform.position;

        var forward = offset.normalized;
        var offset2 = speed * Time.deltaTime * forward;
        if (offset.magnitude <= offset2.magnitude)
        {
            transform.position += offset;
            if (target == to)
            {
                target = from;
            }
            else
            {
                target = to;
            }
        }
        else
        {
            transform.position += offset2;
        }

        if (offset != Vector3.zero)
        {
            var upAxis = transform.rotation * Vector3.up;
            transform.rotation =
                Quaternion.LookRotation(target.transform.position - transform.position, upAxis);
        }
    }
}
*/