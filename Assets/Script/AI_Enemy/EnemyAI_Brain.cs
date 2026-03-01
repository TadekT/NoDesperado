using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
[SerializeField] private enum State
{
    Idle, Patrol, Search, Chase, Attack
}

[SerializeField] private EnemyAI_Movment _movment;





private State currentState;

    private void Awake()
    {
        if(_movment == null)
        {
            _movment = GetComponent<EnemyAI_Movment>();
        }
    }

    private void Start()
    {
        if(_movment == null)
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
                _movment.MoveToNextWaypoint();
                break;
        }
    }

}
