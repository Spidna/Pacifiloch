using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KeyControls : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField][Range(0f, 20f)] float moveSpeed;
    [SerializeField] MotionlessA inputA;

    private void OnEnable()
    {
        inputA.Enable();
    }
    private void OnDisable()
    {
        inputA.Disable();
    }
    void Awake()
    {

    }

    private void Update()
    {
    }

    private void Interact()
    {

    }

}
