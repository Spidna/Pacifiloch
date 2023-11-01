using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Base Swim speed")]
    [Range(0f, 2f)]
    public float swimForce;
    [Tooltip("Minimum swing power needed to move")]
    public float minForce;
    [Tooltip("Minimum time between swings to count")]
    public float minStroke;
    [Tooltip("How fast player turns by using hand turning")]
    [Range(0f, 1f)]
    public float handTurnSpeed;
    [Tooltip("Players turn with single hand swing")]
    public bool handTurnEnabled;

    // TODO Probably remove this
    [Tooltip("Keyboard or VR controls")]
    public bool keyboardControls;
    [Header("Mouse Input controls")]
    [Tooltip("Mouse look sensitivity")]
    public Vector2 mouseTurnSpeed = new Vector2(1, 1);
    [Tooltip("Maximum rotation from the initial orientation")]
    public Vector2 degreeClamp = new Vector2(90, 80);
    [Tooltip("Invert vertical turning")]
    public bool invertY;


    // Orientation state.
    Quaternion initOrientation;
    Vector2 currAngle;
    // Cached cursor state.
    CursorLockMode prevLockState;
    bool cursourVisible;

    [Header("References")]
    [SerializeField] UnityEngine.XR.InputDevice leftController;
    [SerializeField] UnityEngine.XR.InputDevice rightController;
    [SerializeField] InputActionReference leftStrokeButton;
    [SerializeField] InputActionReference rightStrokeButton;
    [SerializeField] InputActionReference leftControllerPos;
    [SerializeField] InputActionReference rightControllerPos;
    [SerializeField] InputActionReference leftControllerVel;
    [SerializeField] InputActionReference rightControllerVel;
    [SerializeField] Transform trackingRef;

    [SerializeField] private GameObject swimDoppler;
    [SerializeField] private AudioSource swimSFX;
    [SerializeField] private Rigidbody rb;

    private Vector3 worldVel;
    private float cooldown = 0f; // time since last stroke
    // Player's current speed
    private float playerMagnitude;

    private void Awake()
    { // Force rigidbody settings to prevent problems
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Establish haptic nodes
        try
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        catch
        {
            Debug.Log("ERR missing controller XRNode.");
        }
    }

    private void FixedUpdate()
    {
        // Check for VR movement inputs
        movementVR();

        // Make a noise based on the velocity of swimmer
        swimSound();
    }

    private void Update()
    {
    }
    /// <summary>
    ///Input controls for keyboard
    /// </summary>
    /*void movementKeyboard()
    {
        // Get WASD input (or customized thereof)
        Vector3 pForce;
        pForce.x = Input.GetAxis("Horizontal");
        pForce.y = Input.GetAxis("Vertical");
        pForce.z = 0f;

        // Convert local velocity into world velocity
        Vector3 worldVel = trackingRef.TransformDirection(pForce);
        rb.AddForce(worldVel * swimForce, ForceMode.Acceleration);
    }*/



    private float preSpeed = 0f;
    /// <summary>
    /// Calculate doppler for swimming
    /// </summary>
    private void swimSound()
    {
        Vector3 swimVelocity = rb.velocity;
        float curSpeed = swimVelocity.magnitude;

        // When accelerating, put the swimDoppler in front of player's movement
        if (preSpeed < curSpeed)
        {
            swimDoppler.transform.position = swimVelocity * 0.3f;
        }
        // When decelerating, drag it behind the player's movement
        else
        {
            swimDoppler.transform.Translate(swimVelocity * -0.1f);
        }
        // Faster = higher pitch & volume
        Debug.Log("CurSpeed: " + curSpeed);
        swimSFX.volume += 0.1f;
        swimSFX.volume *= 2f;
        // TODO if volume greater than target set to target
        // TODO potentially drop pitch changes if interferes with doppler?
        swimSFX.pitch = 1f;

        preSpeed = curSpeed;
    }

    /// <summary>
    /// Input controls for VR
    /// </summary>
    void movementVR()
    {
        cooldown += Time.fixedDeltaTime;

        if (cooldown > minStroke)
        {
            // check if player is going to move
            if (!checkMotion())
            {
                // check if player is going to turn only if they aren't moving
                checkTurn();
            }
        }
    }

    /// <summary>
    /// Check if player is making input to translate
    /// </summary>
    /// <returns>true if player is trying to move</returns>
    bool checkMotion()
    {
        // Only make a stroke if the player is holding down the buttons to do so
        if (leftStrokeButton.action.IsPressed()
            && rightStrokeButton.action.IsPressed())
        {
            // Collect velocity data from controllers
            var leftHandVel = leftControllerVel.action.ReadValue<Vector3>();
            var rightHandVel = rightControllerVel.action.ReadValue<Vector3>();
            Vector3 localVel = leftHandVel + rightHandVel;
            Debug.Log(localVel);
            localVel.x *= Mathf.Abs(localVel.x); //* localVel.x;
            localVel.z *= Mathf.Abs(localVel.z); //* localVel.z;
            localVel.y *= Mathf.Abs(localVel.y); //* localVel.y;
            localVel *= -1f; // Invert cuz we push against water to move the other way

            // Make stroke if strong enough
            if (localVel.sqrMagnitude > minForce * minForce)
            {
                // Convert local velocity into world velocity
                worldVel = trackingRef.TransformDirection(localVel) * swimForce;
                rb.AddForce(worldVel, ForceMode.Impulse);
                cooldown = 0f;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the player is making input to turn.
    /// </summary>
    void checkTurn()
    {
        if (handTurnEnabled)
        {
            // Get rotational velocity from the active hand
            Vector3 turnVel;
            Vector3 controllerPos;

            if (leftStrokeButton.action.IsPressed())
            {
                turnVel = leftControllerVel.action.ReadValue<Vector3>();
                controllerPos = leftControllerPos.action.ReadValue<Vector3>();
                turn(turnVel, controllerPos);
                turnHaptics(turnVel, leftController);
            }
            else if (rightStrokeButton.action.IsPressed())
            {
                turnVel = rightControllerVel.action.ReadValue<Vector3>();
                controllerPos = rightControllerPos.action.ReadValue<Vector3>();
                turn(turnVel, controllerPos);
                turnHaptics(turnVel, rightController);
            }
        }

    }

    /// <summary>
    /// Gives haptic feedback based on how hard player is swimming
    /// </summary>
    /// <param name="turnVel">Velocity of the controller in question</param>
    /// <param name="controller">Controller to output haptics to</param>
    void turnHaptics(Vector3 turnVel, UnityEngine.XR.InputDevice controller)
    {
        float turnMag;
        //Haptics
        turnMag = turnVel.magnitude * 0.03f;
        controller.SendHapticImpulse(0, turnMag, 0.7f);
    }
    /// <summary>
    /// Do the math to turn the player based on the movement of 1 hand
    /// </summary>
    /// <param name="turnVel">Velocity of the controller in question</param>
    /// <param name="controllerPos">Where the controller is</param>
    void turn(Vector3 turnVel, Vector3 controllerPos)
    {
        turnVel.y = 0f; // Zero out the vertical component to prevent pitch (tilting).
        turnVel.x *= Mathf.Abs(turnVel.x);
        turnVel.z *= Mathf.Abs(turnVel.z);

        // Calculate the torque to apply for yaw rotation (around the y-axis).
        float xTorque = -turnVel.x * controllerPos.z;
        float zTorque = turnVel.z * controllerPos.x;
        float yawTorque = xTorque + zTorque;
        yawTorque *= handTurnSpeed;

        // Apply the torque to the Rigidbody for yaw rotation.
        rb.AddTorque(Vector3.up * yawTorque, ForceMode.VelocityChange);
    }

}
