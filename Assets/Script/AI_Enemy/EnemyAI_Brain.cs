using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
[SerializeField] private enum State
{
    Idle, Patrol, Chase, Attack, Search
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

        if(_enemyMovment != null)
        {
            _enemyMovment.OnIdleFinished += HandleIdleFinished;
        }
    }

    private void OnDestroy()
    {
        if(_enemyMovment != null)
        {
            _enemyMovment.OnIdleFinished -= HandleIdleFinished;
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
            
            case State.Patrol:
                _enemyMovment.MoveToNextWaypoint();
                break;
            case State.Idle:
                _enemyMovment.IdleStart();
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

}
