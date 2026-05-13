using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyAI_Movement))]
[RequireComponent(typeof(EnemyAI_Movement))]

public class EnemyAI_Brain : MonoBehaviour
{
private enum EnemyState
{
    None, Idle, Patrol, Suspicious, Chase, Attack, Search
}
[SerializeField] private EnemyState currentState = EnemyState.None;
[SerializeField] private EnemyState previousState;


[SerializeField] private EnemyAI_Movement movement;
[SerializeField] private EnemyAI_Vision vision;

[Header("Idle")]
[SerializeField] private float idleDuration = 2f;


[SerializeField] private float stateTimer;


    private Transform target;
    private Vector3 lastKnownPlayerPosition;

    

    private void Awake()
    {

        if(movement == null)
        {
            movement = GetComponent<EnemyAI_Movement>();
        }
        

        if(vision == null)
        {
            vision = GetComponent<EnemyAI_Vision>();
        }

    }


    private void Start()
    {
        if(movement.HasWyapoints)
        {
            ChangeState(EnemyState.Patrol);
        }
        else
        {
            ChangeState(EnemyState.Idle);
            
        }
        
    }


    private void Update()
    {
        stateTimer += Time.deltaTime;

        TickState(currentState);
    }
    
        private void UpdateVisionData()
    {
        if (vision == null)
            return;

        if (vision.CanSeePlayer && vision.PlayerTransform != null)
        {
            target = vision.PlayerTransform;
            lastKnownPlayerPosition = target.position;
        }
    }

    private void ChangeState(EnemyState nextState)
    {
        if(nextState == currentState) 
            return;

        previousState = currentState;
        ExitState(previousState);
        
        stateTimer = 0;
        currentState = nextState;
        
        EnterState(nextState);

        
    }
    
    
    private void TickState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                TickIdle();
                break;

            case EnemyState.Patrol:
                TickPatrol();
                break;

            case EnemyState.Suspicious:
                TickSuspicious();
                break;

        }        
    }


    private void EnterState(EnemyState state)
    {   
        switch(state)
        {
            
            case EnemyState.Idle:
                movement.Stop();
                break;

            case EnemyState.Patrol:
                if (movement.HasWyapoints)
                {
                    movement.SetNextPatrolDestination();
                }
                break;
                

            case EnemyState.Suspicious:
                movement.Stop();
                break;
        }
    }


    private void ExitState(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.Idle:
                break;
        }
    }


    private void TickIdle()
    {
        if(stateTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }


    
    private void TickPatrol()
    {
        if (CanSeePlayer())
        {
            ChangeState(EnemyState.Suspicious);
            return;
        }
        if (!movement.HasWyapoints)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        if (movement.HasReachedDestination())
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void TickSuspicious()
    {
        if (!CanSeePlayer())
        {
            ChangeState(EnemyState.Patrol);
        }

    }

    private bool CanSeePlayer()
    {
        return vision != null &&
               vision.CanSeePlayer &&
               vision.PlayerTransform != null;
    }
}
