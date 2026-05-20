using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyAI_Movement : MonoBehaviour
{   
    // lista waypointów
    [Header("Waypoints list")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    

    [Header("Reference")]
    private NavMeshAgent _agent;


    [Header("Transitions")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float waypointReachedThreshold = 0.15f;
    
    [Header("Radndom NavMeshSamples Setting")]
    [SerializeField] private float randomSearchRadius = 4f;



    [Header("Current waypoints debug")]
    [SerializeField] private int currentWaypointIndex = 0;
    [SerializeField] private Vector3 currentWaypointPosition;

    public bool HasWaypoints => waypoints != null && waypoints.Count > 0;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetNextPatrolDestination()
    {
        if (!HasWaypoints) 
            return;
        if(_agent == null)
            return;

        currentWaypointIndex++;
        currentWaypointIndex %= waypoints.Count;
    }

    public Vector3 CurrentWaypoint()
    {
        Transform waypoint = waypoints[currentWaypointIndex];
        currentWaypointPosition = waypoint.position;
        return currentWaypointPosition;
    }
    public void MoveTo(Vector3 position)
    {
        if(_agent == null) return;

        Resume();
        _agent.SetDestination(position);
    
    }

    public void Walk()
    {
        _agent.speed = walkSpeed;
    }

    public void Run()
    {
        _agent.speed = runSpeed;
    }

    public void Stop()
    {
        if(_agent == null) return;

        _agent.isStopped = true;
        _agent.ResetPath();
    
    }

    public void Resume()
    {
        if(_agent == null) return;

        _agent.isStopped = false;

    }


    public bool HasReachedDestination()
    {
        
        if(_agent == null)
            return false;
        
        if (_agent.pathPending)
            return false;

        if (_agent.remainingDistance > _agent.stoppingDistance + waypointReachedThreshold)
            return false;

        return !_agent.hasPath || _agent.velocity.sqrMagnitude <= 0.01f;
    
    }


    public void MoveToRandomPosition(Vector3 center)
    {
        Vector3 randomPosition = RandomNavMeshPosition(center);
        if(randomPosition == Vector3.zero)
        {
            return;
        }

        if(randomPosition != Vector3.zero)
        {
            MoveTo(randomPosition);
        }
    }

    public Vector3 RandomNavMeshPosition(Vector3 lastKnown)
    {
        
        Vector3 randomPosition =  Random.insideUnitSphere * randomSearchRadius;

        randomPosition += lastKnown;
        
        Vector3 finalePosition = Vector3.zero;
        
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition,out hit, randomSearchRadius, 1))
        {
            finalePosition = hit.position;
        }

        return finalePosition ;

    }


}
