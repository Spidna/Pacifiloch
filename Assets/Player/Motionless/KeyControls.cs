using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KeyControls : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Rigidbody of the player gameobject")]
    [SerializeField] Rigidbody playerBody;
    [Tooltip("Transform of the player's Head object")]
    [SerializeField] Transform head;

    [Header("Movement")]
    [Tooltip("How fast the player character swims normally")]
    [SerializeField][Range(0f, 20f)] float swimSpeed;
    [Tooltip("Max speed the player can swim at.")]
    [SerializeField][Range(0f, 100f)] float swimCap;
    [Tooltip("How much speed is gained when sprinting")]
    [SerializeField][Range(0f, 20f)] float sprintFactor;
    [Tooltip("Half of how many seconds pushing a stroke yields gains")]
    private float strokeCap;
    [Tooltip("How many seconds for a stroke to peak")]
    [SerializeField][Range(0f, -20f)] float strokePre;
    [Tooltip("Seconds spent in this stroke")]
    private float strokeDuration;
    [Tooltip("How much sprint is currently affecting move speed, 1f+")]
    private float strokeOutput;
    [Tooltip("Drag when player is making no movement inputs")]
    [SerializeField][Range(0f, 20f)] float dragNeutral;
    [Tooltip("Drag as is reduced while moving")]
    [SerializeField][Range(0f, 20f)] float dragSwimming;
    private Vector3 moveVector = Vector3.zero;

    [Header("Camera Control")]
    [SerializeField][Range(0f, 100f)] float mouseSensitivity;
    [Tooltip("Whether we square the mouse delta output to exagerate precision")]
    [SerializeField] bool mouseSquarePrecision;
    [Tooltip("Stores x rotation")]
    private float xRotation = 0f;
    //[SerializeField][Range(0f, 100f)] float stickSensitivity;
    //[Tooltip("Whether we square the joystick output to exagerate precision")]
    //[SerializeField] bool stickSquarePrecision;

    [Tooltip("Static Input settings file")]
    private MotionlessA inputA;

    void Awake()
    {
        inputA = new MotionlessA();

        // Setup stroke calculations
        strokeOutput = 1f;
        strokeDuration = strokePre;
        strokeCap = Mathf.Abs(strokePre * -strokePre);

    }
    private void OnEnable()
    {
        // Start listening to inputs or smthn idk
        inputA.Enable();
        // Activate Movement inputs
        inputA.Player.Movement.performed += OnMovePressed;
        inputA.Player.Movement.canceled += OnMoveRelease;
        // Activate Sprint inputs
        inputA.Player.HardStride.performed += PushingStride;
        inputA.Player.HardStride.canceled += ReleaseStride;
        // Activate Mouse Look
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void OnDisable()
    {
        inputA.Disable();
        inputA.Player.Movement.performed -= OnMovePressed;
        inputA.Player.Movement.canceled -= OnMoveRelease;

        inputA.Player.HardStride.performed -= PushingStride;
        inputA.Player.HardStride.canceled -= ReleaseStride;

        /// TODO SCRAP THIS
        Cursor.lockState = CursorLockMode.None;


    }

    private void FixedUpdate()
    {
        // If strokeDuration is large enough we can assume sprint is being held
        if (strokeDuration > strokePre)
        { // So we continue sprint action
            PushingStride(new InputAction.CallbackContext());
        }

        swimMovement(moveVector);

    }

    private void Update()
    {
        CalculateLook();

    }

    /// <summary>
    /// Make movement calculations.
    /// </summary>
    private void swimMovement(Vector3 direction)
    {
        Vector3 finalSwim = direction;
        // Produce swim speed
        finalSwim *= swimSpeed * strokeOutput;
        // Ensure swim speed doesn't exceed cap
        if (finalSwim.magnitude > swimCap)
        {
            finalSwim = direction * swimCap;
        }
        // Move
        playerBody.AddForce(finalSwim);
    }
    /// <summary>
    /// Gather input data for movement
    /// </summary>
    /// <param name="value">The Vector3 of movement input</param>
    private void OnMovePressed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector3>();
        // TODO probably remove the squaring
        moveVector.x *= Mathf.Abs(moveVector.x);
        moveVector.y *= Mathf.Abs(moveVector.y);
        moveVector.z *= Mathf.Abs(moveVector.z);

        playerBody.drag = dragSwimming;
    }
    /// <summary>
    /// Reset input data for movement
    /// </summary>
    /// <param name="value">The Vector3 of movement input</param>
    private void OnMoveRelease(InputAction.CallbackContext value)
    {
        moveVector = Vector3.zero;
        playerBody.drag = dragNeutral;
    }

    /// <summary>
    /// When the player is holding the sprint button
    /// </summary>
    /// <param name="value">Make sure this thing is called</param>
    private void PushingStride(InputAction.CallbackContext value)
    {
        // Add time to stroke duration
        strokeDuration += Time.deltaTime;

        // Squared to make gain speed faster, peak, then continue gaining speed with
        // diminishing returns
        strokeOutput = strokeDuration * -strokeDuration;
        //Debug.Log("Squared: " + strokeOutput);
        // Add strokeCap to set a speed gain floor
        strokeOutput += strokeCap;
        //Debug.Log("Cap boost: " + strokeOutput);
        // Multiply by sprintFactor to control gains
        strokeOutput *= sprintFactor;
        //Debug.Log("Sprint: " + strokeOutput);
        // Add 1 to make sure 1 is the absolute minimum
        strokeOutput += 1f;
        // Mitigate losses from holding sprint too long
        strokeOutput = Mathf.Clamp(strokeOutput, 0.95f, 10f);
    }
    /// <summary>
    /// When the player releases the sprint button
    /// </summary>
    /// <param name="value">Make calling work somehow idk</param>
    private void ReleaseStride(InputAction.CallbackContext value)
    {
        strokeDuration = strokePre;
        strokeOutput = 1f;
    }


    private void CalculateLook()
    {
        Vector2 value = inputA.Player.Turning.ReadValue<Vector2>();
        value *= mouseSensitivity * Time.deltaTime;

        xRotation -= value.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.transform.Rotate(Vector3.up * value.x);
    }
}
