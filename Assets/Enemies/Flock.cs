using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOTE: It's unlikely that the Obstacle avoidance function will work well
 * against large, flat surfaces or intersecting obstacle hitboxes.
 */

public class Flock : MonoBehaviour ///10:00
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
    [Tooltip("Range agents will start avoiding obstacles")]
    [Range(0f, 10f)]
    [SerializeField] private float _obstacleRange;
    public float obstacleRange { get { return _obstacleRange; } }

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
    [Tooltip("How strongly agents avoid obstacles")]
    [Range(0f, 10f)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }

    [SerializeField] private int spawnSize; // TODO: update to use a list of spawn locations
    public List<FlockAgent> allAgents { get; set; }

    [Header("Synced values")]
    [Tooltip("Values that all members of the flock share")]
    public AtkSync atkSync;

    public GameObject target { get { return atkSync._target; } }
    /* Quick example of how LayerMask works:
      // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
     */
    [Tooltip("Which layer(s) are obstacles. UPDATES AT START()")]
    [SerializeField] private int _obstacleLayerMask;
    public int obstacleLayerMask { get { return _obstacleLayerMask; } }


    private void Start()
    {
        // Find target
        FindTarget();

        // Convert to bitwise value.
        _obstacleLayerMask = 1 << _obstacleLayerMask;

        GenerateUnits(spawnSize, spawnBounds);
    }

    private void FixedUpdate() // TODO: Call checks for unit death
    {
        //for (int i = 0; i < allAgents.Count; i++)
        //{
        //    allAgents[i].Move();
        //}
        atkSync.t += Time.deltaTime;
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
        GameObject[] targetArray = GameObject.FindGameObjectsWithTag(atkSync._targetTag);

        // Since we should only have 1 possible target so far, throw an error if there's multiple
        //TODO: ammend this to a list so we can choose a target instead
        if (targetArray.Length != 1)
        {
            Debug.LogError("Incorrect number of targets with tag: " + atkSync._targetTag + " - " + targetArray.Length
                + ". Where targets should only be 1.");
            Debug.Break();
        }
        atkSync._target = targetArray[0];
    }

}
