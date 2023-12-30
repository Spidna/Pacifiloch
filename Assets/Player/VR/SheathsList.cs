using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheathsList : MonoBehaviour
{
    [Tooltip("Usable Sheaths, 0LeftHip, 1RightHip, 2LeftShoulder, 3RightShoulder")]
    public List<Sheath> Slot;

    [Tooltip("Range Sheaths can grab from")]
    [Range(0f, 0.5f)] public float sucction;
}
