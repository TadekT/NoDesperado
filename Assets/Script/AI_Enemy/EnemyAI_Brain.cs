using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
private enum State
{
    None, Idle, Patrol, Suspicious, Chase, Attack, Search
}
[SerializeField] private State currentState = State.None;
[SerializeField] private EnemyAI_Movement _enemyMovement;
[SerializeField] private EnemyAI_Vision _enemyVision;




    private void Awake()
    {

        if(_enemyMovement == null)
        {
            _enemyMovement = GetComponent<EnemyAI_Movement>();
        }
        

        if(_enemyVision == null)
        {
            _enemyVision = GetComponent<EnemyAI_Vision>();
        }

    }

    private void OnEnable()
    {
       if(_enemyMovement != null)
        {
            _enemyMovement.OnIdleFinished += HandleIdleFinished;
        }
        if(_enemyVision != null)
        {
            _enemyVision.OnPlayerEnteredFOV += HandlePlayerInFOV;
            _enemyVision.OnPlayerExitedFOV +=  HandlePlayerOutFOV;
        }
    }

    private void OnDisable()
    {
        if(_enemyMovement != null)
        {
            _enemyMovement.OnIdleFinished -= HandleIdleFinished;
            
        }

        if(_enemyVision != null)
        {
            _enemyVision.OnPlayerEnteredFOV -= HandlePlayerInFOV;
            _enemyVision.OnPlayerExitedFOV -=  HandlePlayerOutFOV;
        }       
    }



    private void Start()
    {
        if(_enemyMovement == null)
        {
            this.enabled = false;
            return;
        }
        ChangeState(State.Patrol);
        
    }


    private void Update()
    {
        if (_enemyMovement == null)
            return;

        if (currentState == State.Patrol && _enemyMovement.HasReachedWaypoint())
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
                _enemyMovement.IdleStart();
                break;
            case State.Patrol:
                _enemyMovement.PatrolState();
                break;
            case State.Suspicious:
                _enemyMovement.SuspiciousState();
                break;
        }
    }


    private void ExitState(State state)
    {
        switch(state)
        {
            case State.Idle:
                _enemyMovement.IdleStop();
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
        if(currentState != State.Suspicious)
        {
            ChangeState(State.Suspicious);
        }
    }

    private void HandlePlayerOutFOV()
    {
        if(currentState == State.Suspicious)
        {
            ChangeState(State.Patrol);
        }
    }

}
