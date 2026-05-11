using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyAI_Movement : MonoBehaviour
{   

    public event Action OnIdleFinished;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    

    [Header("Reference")]
    private NavMeshAgent _agent;


    [Header("Transitions")]
    [SerializeField] private float idleTimeInterval = 2f;
    [SerializeField] private float waypointReachedThreshold = 0.15f;
    
    [SerializeField] private int currentWaypointIndex = 0;


    [Header("Courutine reference")]
    private Coroutine IdleCoroutineReference;
    

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if(waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy.");
            return;
        }
    }
    
    public void PatrolState()
    {
        if (waypoints.Count == 0) 
            return;
        if(_agent == null)
            return;

        _agent.isStopped = false;

        _agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }
    
    public bool HasReachedWaypoint()
    {
        if(_agent == null)
            return false;
        
        if (_agent.pathPending)
            return false;

        if (_agent.remainingDistance > _agent.stoppingDistance + waypointReachedThreshold)
            return false;

        return !_agent.hasPath || _agent.velocity.sqrMagnitude <= 0.01f;
    }

#region idle
    private IEnumerator IdleState()
    {
       yield return new WaitForSecondsRealtime(idleTimeInterval);
       OnIdleFinished?.Invoke();
    }

    public void IdleStart()
    {
        if(IdleCoroutineReference != null)
            return;

        IdleCoroutineReference = StartCoroutine(IdleState());
        Debug.Log("Start Idle ");
    }

    public void IdleStop()
    {
        if(IdleCoroutineReference == null)
            return;

        StopCoroutine(IdleCoroutineReference);
        IdleCoroutineReference = null;
        Debug.Log("Stop Idle ");
        
    }
#endregion    

    public void SuspiciousState()
    {
        _agent.isStopped = true;
    

    }

    private void FollowPlayerMovement()
    {

    }

}
