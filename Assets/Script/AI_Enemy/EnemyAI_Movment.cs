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

    
    [SerializeField] private int currentWaypointIndex = 0;
    

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
        

    }
    public void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0)
            return;

        _agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }


// czy mob dotarł do waypontia / celu
// jeśli tak to wyznaczyć następny, jeśli nie to return

    


}
