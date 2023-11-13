using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [Tooltip("My Gameobject")]
    [SerializeField] private GameObject me;
    [Tooltip("The collider that collects items into bag")]
    [SerializeField] private Collider collectRange;

    [Header("Inventory")]
    [Tooltip("List of items in Inventory, index shared with itemsCount")]
    [SerializeField] private List<ItemDetails> items;
    [Tooltip("Quantity of each item in Inventory, index shared with items")]
    [SerializeField] private List<int> itemsCount;
    [Tooltip("Maximum capacity of bag")]
    [SerializeField][Range(0, 50000)] private int capacity;
    [Tooltip("Current total quantity in Inventory DONT EDIT MANUALLY")]
    [SerializeField][Range(0, 50000)] private int totalCount;

    private void OnCollisionEnter(Collision collision)
    {
        // Only execute if it's the right object type
        if (collision.gameObject.CompareTag("Collectable"))
        {
            AddToBag(collision.gameObject, 1);
        }
    }

    /// <summary>
    /// Store the input item(s) in this bag
    /// </summary>
    /// <param name="input">What's going in the bag</param>
    /// <param name="quantity">How much of it's going in the bag</param>
    /// <returns>True if succesffully stored</returns>
    public bool AddToBag(GameObject input, int quantity)
    {
        // Get the Collectable component
        Collectable toStore = input.GetComponent<Collectable>();
        if (toStore != null)
        {
            Debug.Log(input + "Is missing Collectable component.", input);
            Debug.DebugBreak();
            return false;
        }



        return false;
    }

    /// <summary>
    /// Find the target in this bag
    /// </summary>
    /// <param name="target">What we're looking for</param>
    /// <returns>Index of target in bag, (-index -1) of closest in order</returns>
    public int FindInBag(ItemDetails target)
    {


        return FindInBag(target, 0, items.Count - 1, 0, 0);
    }
    private int FindInBag(ItemDetails target, int low, int high, int mid, int comparisonResult)
    {
        // Exhausted the search, return the negative closest option -1
        // -1 so we distinguish between index 0 and -0
        if (low > high)
            return -low -1;

        // halway point
        mid = (low + high) / 2;
        comparisonResult = string.Compare(items[mid].Name, target.Name);

        // Item found at index 'mid'
        if (comparisonResult == 0)
            return mid;
        // Target is greater, search the right half
        else if (comparisonResult < 0)
            low = mid + 1;
        // Target is smaller, search the left half
        else
            high = mid - 1;

        return FindInBag(target, low, high, mid, comparisonResult);
    }
}