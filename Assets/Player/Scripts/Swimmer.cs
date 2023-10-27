using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Swimmer : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Base Swim speed")]
    [Range(0.5f, 2.5f)]
    public float swimForce;
    [Tooltip("How much water slows player")]
    public float dragForce;
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
    [SerializeField] InputActionReference leftStrokeButton;
    [SerializeField] InputActionReference rightStrokeButton;
    [SerializeField] InputActionReference leftControllerPos;
    [SerializeField] InputActionReference rightControllerPos;
    [SerializeField] InputActionReference leftControllerVel;
    [SerializeField] InputActionReference rightControllerVel;
    [SerializeField] Transform trackingRef;

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
        movementVR();

    }
    private void Update()
    {
    }
    /*
    * Input controls for keyboard
    */
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


    /*
    * Input controls for VR controllers
    */

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

        // Apply drag if player is moving
        /*playerMagnitude = pVelocity.sqrMagnitude;
        if (playerMagnitude > 0.01f)
        {
            pVelocity -= pVelocity * dragForce;
        }
        else if (playerMagnitude != 0f) // or just halt movement (Not in tutorial)
        {
            pVelocity *= 0f;
        }*/
    }

    bool checkMotion()
    {
        // Only make a stroke if the player is holding down the buttons to do so
        if (leftStrokeButton.action.IsPressed()
            && rightStrokeButton.action.IsPressed()) // Switch to &&
        {
            // Collect velocity data from controllers
            var leftHandVel = leftControllerVel.action.ReadValue<Vector3>();
            var rightHandVel = rightControllerVel.action.ReadValue<Vector3>();
            Vector3 localVel = leftHandVel + rightHandVel;
            Debug.Log(localVel);
            localVel.x *= Mathf.Abs(localVel.x);
            localVel.z *= Mathf.Abs(localVel.z);
            localVel.y *= Mathf.Abs(localVel.y);
            localVel *= -1f; // Invert cuz we push against water to move the other way

            // Make stroke if strong enough
            if (localVel.sqrMagnitude > minForce * minForce)
            {
                // Convert local velocity into world velocity
                worldVel = trackingRef.TransformDirection(localVel) * swimForce;
                rb.AddForce(worldVel * swimForce, ForceMode.Acceleration);
                cooldown = 0f;
            }
            return true;
        }
        return false;
    }

    // !!! TODO
    // !!! TODO
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
            }
            else if (rightStrokeButton.action.IsPressed())
            {
                turnVel = rightControllerVel.action.ReadValue<Vector3>();
                controllerPos = rightControllerPos.action.ReadValue<Vector3>();
                turn(turnVel, controllerPos);
            }
        }

    }
    void turn(Vector3 turnVel, Vector3 controllerPos)
    {
        turnVel.y = 0f; // Zero out the vertical component to prevent pitch (tilting).

        // Calculate the torque to apply for yaw rotation (around the y-axis).
        float xTorque = -turnVel.x * controllerPos.z;
        float zTorque = turnVel.z * controllerPos.x;
        float yawTorque = xTorque + zTorque;
        yawTorque *= handTurnSpeed;

        // Apply the torque to the Rigidbody for yaw rotation.
        rb.AddTorque(Vector3.up * yawTorque, ForceMode.VelocityChange);
    }

}
