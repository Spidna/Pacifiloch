using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrModel : MonoBehaviour
{
    public System.Action onUpdate;


    public Transform parent;

    /// <summary>
    /// Doesn't make sense for the PC entire body to rotate with them
    /// </summary>
    void ResetXZRotation()
    {
        float yRot = -parent.transform.localEulerAngles.x;
        // Set x and z rotations to zero.
        transform.localRotation = Quaternion.Euler(yRot, 0f, 0f);
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
