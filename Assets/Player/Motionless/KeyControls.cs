using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KeyControls : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [Tooltip("How fast the player character swims normally")]
    [SerializeField][Range(0f, 20f)] float swimSpeed;
    [Tooltip("How much speed is gained when sprinting")]
    [SerializeField][Range(0f, 20f)] float sprintFactor;
    [Tooltip("How many seconds pushing a stroke yields gains")]
    [SerializeField][Range(0f, 20f)] float strokeCap;
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


    private MotionlessA inputA;
    private Vector3 moveVector = Vector3.zero;


    private void FixedUpdate()
    {
        swimMovement(moveVector);
    }


    void Awake()
    { 
        inputA = new MotionlessA();
        strokeOutput = 1f;
        strokeDuration = strokePre;
    }
    private void OnEnable()
    {
        inputA.Enable();
        inputA.Player.Movement.performed += OnMovePressed;
        inputA.Player.Movement.canceled += OnMoveRelease;

        inputA.Player.HardStride.performed += PushingStride;
        inputA.Player.HardStride.canceled += ReleaseStride;

    }
    private void OnDisable()
    {
        inputA.Disable();
        inputA.Player.Movement.performed -= OnMovePressed;
        inputA.Player.Movement.canceled -= OnMoveRelease;

        inputA.Player.HardStride.performed -= PushingStride;
        inputA.Player.HardStride.canceled -= ReleaseStride;
    }

    /// <summary>
    /// Make movement calculations.
    /// </summary>
    private void swimMovement(Vector3 direction)
    {
        rb.AddForce(moveVector * swimSpeed);
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

        rb.drag = dragSwimming;
    }
    /// <summary>
    /// Reset input data for movement
    /// </summary>
    /// <param name="value">The Vector3 of movement input</param>
    private void OnMoveRelease(InputAction.CallbackContext value)
    {
        moveVector = Vector3.zero;
        rb.drag = dragNeutral;
    }
    /// <summary>
    /// When the player is holding the sprint button
    /// </summary>
    /// <param name="value">Make sure this thing is called</param>
    private void PushingStride(InputAction.CallbackContext value)
    {

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
}
