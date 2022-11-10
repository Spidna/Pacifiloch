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
    [Tooltip("Distance between Agents who count as Cohesion Neighbours")]
    [Range(0f, 10f)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }
    [Tooltip("Distance between Agents who count as Avoidance Neighbours")]
    [Range(0f, 10f)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }
    [Tooltip("Distance between Agents who count as Alignment Neighbours")]
    [Range(0f, 10f)]
    [SerializeField] private float _alignmentDistance;
    public float alignmentDistance { get { return _alignmentDistance; } }

    [Header("Behaviour Weights")]
    [Tooltip("How strongly attracted agents are")]
    [Range(0f, 10f)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }
    [Tooltip("How respected personal space is")]
    [Range(0f, 10f)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }
    [Tooltip("How much agents want to go the same direction")]
    [Range(0f, 10f)]
    [SerializeField] private float _alignmentWeight;
    public float alignmentWeight { get { return _alignmentWeight; } }


    [SerializeField] private int spawnSize; // TODO update to use a list
    public List<FlockAgent> allAgents { get; set; } // TODO update to use a list

    private void Start()
    {
        allAgents = new List<FlockAgent>();
        GenerateUnits(spawnSize, spawnBounds);
    }

    private void FixedUpdate() // TODO: Call checks for unit death
    {
        for (int i = 0; i < allAgents.Count; i++)
        {
            allAgents[i].MoveUnit();
        }
    }

    private void GenerateUnits(int spawnCount, Vector3 _spawnBounds)
    {
        for (int i = 0; i < spawnCount; i++) 
        {
            Vector3 randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * _spawnBounds.x, randomVector.y * _spawnBounds.x, randomVector.z * _spawnBounds.x);
            Vector3 spawnPosition = transform.position + randomVector;
            Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allAgents.Add(Instantiate(agentPrefab, spawnPosition, rotation));
            allAgents[i].AssignFlock(this);
        }
    }

}
