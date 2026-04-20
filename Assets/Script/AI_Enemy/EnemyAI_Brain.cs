using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
[SerializeField] private enum State
{
    Idle, Patrol, Search, Chase, Attack
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

    private void Start()
    {
        if(_enemyMovment == null)
        {
            this.enabled = false;
            return;
        }
        ChangeState(State.Patrol);
        
    }


    private void ChangeState(State next)
    {
        if(next == currentState) return;


        currentState = next;
        EntereState(currentState);
    }
    private void EntereState(State state)
    {   
        switch(state)
        {
            case State.Patrol:
                //_enemyMovment.MoveToNextWaypoint();
                break;
        }
    }

}
