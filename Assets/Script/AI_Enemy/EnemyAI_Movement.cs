using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor.PackageManager.Requests;


public class EnemyAI_Movement : MonoBehaviour
{   

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    

    [Header("Reference")]
    private NavMeshAgent _agent;


    [Header("Transitions")]
    [SerializeField] private float waypointReachedThreshold = 0.15f;
    
    [SerializeField] private int currentWaypointIndex = 0;

    public bool HasWyapoints => waypoints != null && waypoints.Count > 0;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    

    public void SetNextPatrolDestination()
    {
        if (!HasWyapoints) 
            return;
        if(_agent == null)
            return;

        Resume();

        Transform waypoint = waypoints[currentWaypointIndex];
        _agent.SetDestination(waypoint.position);

        currentWaypointIndex++;
        currentWaypointIndex %= waypoints.Count;
    
    }


    public void MoveTo(Vector3 position)
    {
        if(_agent == null) return;

        Resume();
        _agent.SetDestination(position);
    
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



}
