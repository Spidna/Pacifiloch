using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour //10:00
{
    [Header("Spawn Setup")]
    [Tooltip("The prefab that will be used to populate flock")]
    [SerializeField] private FlockAgent agentPrefab;
    [Tooltip("The box dimensions the population can spawn within")]
    [SerializeField] private Vector3 spawnBounds;

    [Header("Detection Distances")]
    [Range(0f, 10f)]
    [Tooltip("The distance between agents that counts as neighbours")]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [SerializeField] private int spawnSize; // TODO update to use a list
    public List<FlockAgent> allAgents { get; set; } // TODO update to use a list

    private void Start()
    {
        GenerateUnits();
    }

    private void Update()
    {
        for (int i = 0; i < allAgents.Count; i++)
        {
            allAgents[i].MoveUnit();
        }
    }

    private void GenerateUnits() //TODO pretty sure this is redundant
    {
        allAgents = new List<FlockAgent>(spawnSize);
        for (int i = 0; i < allAgents.Count; i++) 
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.x, randomVector.z * spawnBounds.x);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allAgents[i] = Instantiate(agentPrefab, spawnPosition, rotation);
            allAgents[i].AssignFlock(this);
        }
    }

}
