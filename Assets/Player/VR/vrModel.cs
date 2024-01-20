using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrModel : MonoBehaviour
{
    public System.Action onUpdate;
    public float xRot;
    public float zRot;

    /// <summary>
    /// Doesn't make sense for the PC entire body to rotate with them
    /// </summary>
    void ResetXZRotation()
    {
        // Set x and z rotations to zero.
        transform.rotation = Quaternion.Euler(xRot, transform.eulerAngles.y, zRot);
    }

    private void Start()
    {
        onUpdate += ResetXZRotation;
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate.Invoke();
    }
}
