using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Base Swim speed")]
    [Range(0f, 10f)] public float swimForce;
    [Tooltip("Minimum swing power needed to move")]
    public float minForce;
    [Tooltip("Minimum time between swings to count")]
    public float minStroke;

    [Tooltip("How fast player turns by using hand turning")]
    [Range(0f, 1f)] public float handTurnSpeed;
    [Range(0f, 11f)] public float maxTurnSpeed;
    [Tooltip("Players turn with single hand swing")]
    public bool handTurnEnabled;


    [Tooltip("How loud swimming is")]
    [SerializeField][Range(0f, 0.4f)] float volumeFactor;
    [Tooltip("How strong haptics for swimming is")]
    [SerializeField][Range(0f, 0.01f)] float hapticFactor;

    // Orientation state.
    Quaternion initOrientation;
    Vector2 currAngle;
    // Cached cursor state.
    CursorLockMode prevLockState;
    bool cursourVisible;



    [Header("References")]
    [SerializeField] Camera myCamera;
    [SerializeField] XRBaseController leftController;
    [SerializeField] XRBaseController rightController;
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




    }

    private void FixedUpdate()
    {
        // Make a noise based on the velocity of swimmer
        swimSound();
        //toggleAbleFixedUpdate();
    }

    private void Update()
    {
        // Check for VR movement inputs
        movementVR();
    }

    private float preSpeed = 0f;
    /// <summary>
    /// Calculate doppler for swimming
    /// </summary>
    private void swimSound()
    {
        Vector3 swimVelocity = rb.velocity + rb.angularVelocity;
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
        // Fade swimSound in over 3 frames max
        swimSFX.volume += 0.08f;
        swimSFX.volume *= 2f;
        // Cap that fade in at the target volume
        float maxVolume = curSpeed * volumeFactor;
        if (maxVolume > 1f)
            maxVolume = 1f;

        swimSFX.volume = Mathf.Clamp(swimSFX.volume, 0f, maxVolume);

        // TODO potentially drop pitch changes if interferes with doppler?
        //swimSFX.pitch = 1f;

        preSpeed = curSpeed;
    }
    /// <summary>
    /// Stretch FOV based on speed
    /// </summary>

    private event System.Action toggleAbleFixedUpdate;

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

    // If the player hasn't released both movement buttons, don't turn yet
    // preventing nauseating turning at the end of a stroke
    bool motionDisableTurn = false;
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
            motionDisableTurn = true;
            // Collect velocity data from controllers
            var leftHandVel = leftControllerVel.action.ReadValue<Vector3>();
            var rightHandVel = rightControllerVel.action.ReadValue<Vector3>();
            Vector3 localVel = leftHandVel + rightHandVel;
            // Squared to improve precision
            localVel.x *= Mathf.Abs(localVel.x); //* localVel.x;
            localVel.z *= Mathf.Abs(localVel.z); //* localVel.z;
            localVel.y *= Mathf.Abs(localVel.y); //* localVel.y;
            // Invert cuz we push against water to move the other way
            localVel *= -1f;

            // Make stroke if strong enough
            if (localVel.sqrMagnitude > minForce * minForce)
            {
                // Convert local velocity into world velocity
                worldVel = trackingRef.TransformDirection(localVel) * swimForce * Time.deltaTime;
                rb.AddForce(worldVel, ForceMode.Impulse);
                cooldown = 0f;

                // Haptic feedback for input recognition
                turnHapticsOculus(rightHandVel, rightController);
                turnHapticsOculus(leftHandVel, leftController);

            }
            return true;
        }

        /// If the player hasn't released both movement buttons, don't turn yet
        if (motionDisableTurn)
            motionDisableTurn = leftStrokeButton.action.IsPressed() || rightStrokeButton.action.IsPressed();
        return motionDisableTurn;
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
                turnHapticsOculus(turnVel, leftController);
            }
            else if (rightStrokeButton.action.IsPressed())
            {
                turnVel = rightControllerVel.action.ReadValue<Vector3>();
                controllerPos = rightControllerPos.action.ReadValue<Vector3>();
                turn(turnVel, controllerPos);
                turnHapticsOculus(turnVel, rightController);
            }
        }

    }

    /// <summary>
    /// Gives haptic feedback based on how hard player is swimming
    /// </summary>
    /// <param name="turnVel">Velocity of the controller in question</param>
    /// <param name="controller">Controller to output haptics to</param>
    void turnHapticsOculus(Vector3 turnVel, XRBaseController controller)
    {
        float turnMag;
        //Haptics
        turnMag = turnVel.magnitude;
        turnMag *= turnMag * hapticFactor;
        Mathf.Clamp(turnMag, 0f, 1f);
        controller.SendHapticImpulse(turnMag, 0.022f);
    }
    /// <summary>
    /// Do the math to turn the player based on the movement of 1 hand
    /// </summary>
    /// <param name="turnVel">Velocity of the controller in question</param>
    /// <param name="controllerPos">Where the controller is</param>
    void turn(Vector3 turnVel, Vector3 controllerPos)
    {
        turnVel.y = 0f; // Zero out the vertical component to prevent pitch (tilting).
        // Square velocity to increase exagerate control
        turnVel.x *= Mathf.Abs(turnVel.x);
        turnVel.z *= Mathf.Abs(turnVel.z);

        // Calculate the torque to apply for yaw rotation (around the y-axis).
        float xTorque = -turnVel.x * controllerPos.z;
        float zTorque = turnVel.z * controllerPos.x;
        float yawTorque = xTorque + zTorque;
        yawTorque *= handTurnSpeed * Time.deltaTime;
        // Limit rotation speed to prevent sickness
        yawTorque = Mathf.Clamp(yawTorque, -maxTurnSpeed, maxTurnSpeed);

        // Apply the torque to the Rigidbody for yaw rotation.
        rb.AddTorque(Vector3.up * yawTorque, ForceMode.VelocityChange);
    }
}