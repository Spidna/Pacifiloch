using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePrefabs : MonoBehaviour
{
    [Tooltip("Prefab to create")]
    [SerializeField] protected GameObject toMake;
    [Tooltip("How many to make")]
    [SerializeField] public int quantity;
    [Tooltip("Sphere to spawn within")]
    public float radius;




    // Start is called before the first frame update
    void Start()
    {
        randStart();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Execute at the start in a random arrangement
    /// </summary>
    void randStart()
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector3 randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector *= radius;
            Vector3 spawnPosition = transform.position + randomVector;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Instantiate(toMake, spawnPosition, rotation);
        }
    }
}
