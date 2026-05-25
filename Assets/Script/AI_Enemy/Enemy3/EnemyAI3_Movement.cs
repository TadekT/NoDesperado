using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;



public class EnemyAI3_Movement : MonoBehaviour
{   
    // lista waypointów
    [Header("Waypoints list")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    
    
    private NavMeshAgent _agent;


    [Header("Transitions")]
    [SerializeField] private float walkSpeed = 2;
    [SerializeField] private float runSpeed = 4;
    [SerializeField] private float waypointReachedThreshold = 0.15f;
    [SerializeField] private float rotationSpeed = 6f;
    
    [Header("Random NavMeshSamples Setting")]
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
        if(_agent == null) return;
        _agent.speed = walkSpeed;
    }

    public void Run()
    {
        if(_agent == null) return;
        _agent.speed = runSpeed;
    }

    public void Stop()
    {
        if(_agent == null) return;

        _agent.isStopped = true;
        _agent.ResetPath();
    
    }

    public void ChaseDistance()
    {
        if(_agent == null) return;
        _agent.stoppingDistance = 2f;    
    }

    public void PatrolDistance()
    {
        if(_agent == null) return;
        _agent.stoppingDistance = 0.3f;    
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
        
        if(randomPosition == Vector3.zero) return;

        MoveTo(randomPosition);
    
    }

    public Vector3 RandomNavMeshPosition(Vector3 lastKnown)
    {
        
        Vector3 randomPosition =  Random.insideUnitSphere * randomSearchRadius;

        randomPosition += lastKnown;
        
        Vector3 finalPosition = Vector3.zero;
        
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition,out hit, randomSearchRadius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition ;

    }

    public void FaceToTarget(Vector3 targetPosition)
    {
        
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;
        if(direction.sqrMagnitude < 0.001f) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime * rotationSpeed);

    }

}
