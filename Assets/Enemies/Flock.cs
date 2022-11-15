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
    [Tooltip("Range agents can detect their target from")]
    [Range(0f, 10f)]
    [SerializeField] private float _targetingRange;
    public float targetingRange { get { return _targetingRange; } }

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
    [Tooltip("How much agents move to target (negative makes flee)")]
    [Range(-10f, 10f)]
    public float targetWeight;

    [SerializeField] private int spawnSize; // TODO: update to use a list of spawn locations
    public List<FlockAgent> allAgents { get; set; }


    [Tooltip("The thing that the flock attacks/avoids")]
    // TODO: update to have a list of targets in the event of multiplayer or 'substitutes'
    [SerializeField] private GameObject _target;
    [Tooltip("Tag used to find target(s)")]
    [SerializeField] private string _targetTag;
    public GameObject target { get { return _target; } }


    private void Start()
    {
        // Find target
        FindTarget();

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
        allAgents = new List<FlockAgent>();

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

    private void FindTarget()
    {
        // Find possible targets
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag(_targetTag);

        // Since we should only have 1 possible target so far, throw an error if there's multiple
        //TODO: ammend this to a list so we can choose a target instead
        if (targetArray.Length != 1)
        {
            Debug.LogError("Incorrect number of targets with tag: " + _targetTag + " - " + targetArray.Length
                + ". Where targets should only be 1.");
            Debug.Break();
        }
        _target = targetArray[0];
    }

}
