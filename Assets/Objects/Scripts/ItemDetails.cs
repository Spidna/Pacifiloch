using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails",
    menuName = "ScriptableObjects/Item Details", order = 6)]

/// This is the information that will be used to store
/// items in bags.
public class ItemDetails : ScriptableObject
{
    public string Name;
    public string Description;
    [Tooltip("Market value in pearls")]
    public float Value;
    [Tooltip("What'd be created in the event this is dropped")]
    public GameObject Object;
}
