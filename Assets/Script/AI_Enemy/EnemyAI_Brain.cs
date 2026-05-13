using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI_Movement))]
[RequireComponent(typeof(EnemyAI_Movement))]

public class EnemyAI_Brain : MonoBehaviour
{
private enum State
{
    None, Idle, Patrol, Suspicious, Chase, Attack, Search
}
[SerializeField] private State currentState = State.None;
[SerializeField] private State previousState;


[SerializeField] private EnemyAI_Movement movement;
[SerializeField] private EnemyAI_Vision vision;





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
        if(movement == null)
        {
            this.enabled = false;
            return;
        }
        ChangeState(State.Patrol);
        
    }


    private void Update()
    {

    }
    

    private void ChangeState(State nextState)
    {
        if(nextState == currentState) 
            return;

        previousState = currentState;
        ExitState(previousState);
        
        currentState = nextState;
        
        EntereState(nextState);

        
    }
    
    
    private void TickState(State state)
    {
        switch (state)
        {
            
        }        
    }


    private void EntereState(State state)
    {   
        switch(state)
        {
            
            case State.Idle:
                break;
            case State.Patrol:
                break;
            case State.Suspicious:
                break;
        }
    }


    private void ExitState(State state)
    {
        switch(state)
        {
            case State.Idle:
                break;
        }
    }



}
