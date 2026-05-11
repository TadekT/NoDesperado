using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
[SerializeField] private enum State
{
    Idle, Patrol, Suspice, Chase, Attack, Search
}

[SerializeField] private EnemyAI_Movment _enemyMovment;
[SerializeField] private EnemyAI_Vision _enemyVision;




private State currentState;

    private void Awake()
    {

        if(_enemyMovment == null)
        {
            _enemyMovment = GetComponent<EnemyAI_Movment>();
        }
        

        if(_enemyVision == null)
        {
            _enemyVision = GetComponent<EnemyAI_Vision>();
        }

    }

    private void OnEnable()
    {
       if(_enemyMovment != null)
        {
            _enemyMovment.OnIdleFinished += HandleIdleFinished;
        }
        if(_enemyVision != null)
        {
            _enemyVision.OnPlayerEnteredFOV += HandlePlayerInFOV;
            _enemyVision.OnPlayerExitedFOV +=  HandlePlayerOutFOV;
        }
    }

    private void OnDisable()
    {
        if(_enemyMovment != null)
        {
            _enemyMovment.OnIdleFinished -= HandleIdleFinished;
            
        }

        if(_enemyVision != null)
        {
            _enemyVision.OnPlayerEnteredFOV -= HandlePlayerInFOV;
            _enemyVision.OnPlayerExitedFOV -=  HandlePlayerOutFOV;
        }       
    }



    private void Start()
    {
        if(_enemyMovment == null)
        {
            this.enabled = false;
            return;
        }
        ChangeState(State.Patrol);
        
    }


    private void Update()
    {
        if (_enemyMovment == null)
            return;

        if (currentState == State.Patrol && _enemyMovment.HasReachedWaypoint())
        {
            ChangeState(State.Idle);
        }
    }


    private void ChangeState(State next)
    {
        if(next == currentState) 
            return;

        ExitState(currentState);

        currentState = next;
        
        EntereState(currentState);
    }


    private void EntereState(State state)
    {   
        switch(state)
        {
            
            case State.Idle:
                _enemyMovment.IdleStart();
                break;
            case State.Patrol:
                _enemyMovment.PatrolState();
                break;
            case State.Suspice:
                _enemyMovment.SuspiceState();
                break;
        }
    }


    private void ExitState(State state)
    {
        switch(state)
        {
            case State.Idle:
                _enemyMovment.IdleStop();
                break;
        }
    }


    private void HandleIdleFinished()
    {
        if(currentState == State.Idle)
        {
            ChangeState(State.Patrol);
        }
    }


    private void HandlePlayerInFOV()
    {
        if(currentState != State.Suspice)
        {
            ChangeState(State.Suspice);
        }
    }

    private void HandlePlayerOutFOV()
    {
        if(currentState == State.Suspice)
        {
            ChangeState(State.Patrol);
        }
    }

}
