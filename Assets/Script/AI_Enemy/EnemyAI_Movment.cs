using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Movment : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    

    [Header("Reference")]
    private NavMeshAgent _agent;




    [Header("Transitions")]
    [SerializeField] private bool HasReachedDestination;
    [SerializeField] private bool agentHasPath;
    [SerializeField] private int currentWaypointIndex = 0;
    [SerializeField] float distanceToWaypoint;
    [SerializeField] float rm;
    [SerializeField] NavMeshPathStatus ps;
    [SerializeField] bool pathStale;
    [SerializeField] bool hasAPath;
[SerializeField] bool pendingPath;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy.");
            return;
        }
    }
    private void Update()
    {
        agentHasPath = _agent.hasPath;
        MobReacheDestination();
        rm = _agent.remainingDistance;
        ps = _agent.pathStatus;
        pathStale = _agent.isPathStale;
        hasAPath = _agent.hasPath;
        pendingPath = _agent.pathPending;
    }
    public void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0)
            return;

        _agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    private bool MobReacheDestination()
    {
        distanceToWaypoint = Vector3.Distance(waypoints[currentWaypointIndex - 1].position, _agent.transform.position);

        if(distanceToWaypoint <= 1f)
        {
            HasReachedDestination = true;
            return true;
        }
        else
        {
            HasReachedDestination = false;
            return false;
        }


    }
// czy mob dotarł do waypontia / celu
// jeśli tak to wyznaczyć następny, jeśli nie to return

    


}
