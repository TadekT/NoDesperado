using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movment : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints;
    

    [Header("Reference")]
    private NavMeshAgent _agent;

    [Header("Transitions")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool HasReachedDestination;
    
    
    private int currentWaypointIndex = 0;

    private void Awake()
    {
        if(waypoints.Count == 0)
        {
            Debug.Log("Waypoints list is empty. Please assign waypoints in the inspector.");
        }    
        
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.Log("NavMeshAgent component not found on " + gameObject.name);
        }
    }

    private void Start()
    {
        
        MoveToNextWaypoint();
    }

    private void Update()
    {
        if (!isMoving)
        {
            MoveToNextWaypoint();
        }
        else
        {
            CheckIfReachedDestination();
        }

    }

    private void RotateTorwardsTarget()
    {
        Vector3 turnTowardNavSteeringTarget = _agent.steeringTarget;
        Vector3 direction = (turnTowardNavSteeringTarget  - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
    }
    public void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0)
        {
            Debug.Log("No waypoints assigned. Cannot move.");
            return;
        }
        RotateTorwardsTarget();

        _agent.SetDestination(waypoints[currentWaypointIndex].position);
        isMoving = true;
        if(currentWaypointIndex >= waypoints.Count - 1)
        {
            currentWaypointIndex = 0;
        }
        else
        {
            currentWaypointIndex ++;
        }

    }

    
    private void CheckIfReachedDestination()
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            HasReachedDestination = true;
            isMoving = false;
        }
        else
        {
            HasReachedDestination = false;
        }

    }   



}
