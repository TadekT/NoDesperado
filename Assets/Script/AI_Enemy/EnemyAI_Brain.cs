using System;
using UnityEngine;

// komponenty wymagane do działania skryptu
[RequireComponent(typeof(EnemyAI_Movement))]
[RequireComponent(typeof(EnemyAI_Vision))]

public class EnemyAI_Brain : MonoBehaviour
{

    public event Action<EnemyState,EnemyState> OnChangeState;

    // prosty enum state trzymający wszystkie stany przeciwnika AI
    public enum EnemyState
    {
        None, Idle, Patrol, Suspicious, Chase, Attack, Search
    }
   
    //Zmienna przechowójąca obacnie działający stan
    [SerializeField] private EnemyState currentState = EnemyState.None;
    
    //Zmienna przechowójąca poprzednio działający stan
    [SerializeField] private EnemyState previousState;

    //Referencja do skryptu EnemyAI_Movement, aby sterować poruszaniem sie przeciwnika AI
    [SerializeField] private EnemyAI_Movement movement;

    //Referencja do skryptu EnemyAI_Vision, aby odczytywać dane z wizji przeciwnika AI
    [SerializeField] private EnemyAI_Vision vision;


    [Header("Idle")]
    //Zmienna określająca wartość długości stanu idle, odliczany poprzes porównanie z stateTimer 
    [SerializeField] private float idleDuration = 2f;


    [Header("Suspicious")]
    [SerializeField] private float suspiciousDuration = 2f;

    // Ogólny licznik do odczytania Time.deltatime, do zerowania i sterowania TickStatem
    [SerializeField] private float stateTimer;

    // transform Playera 
    private Transform target;

    // ostatnia znana pozycja gracza, do urzytku w TickState chase
    

    
    //Sprawdzanie poprawności przypisania referencji
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

    // Start, do wyjścia ze stanu EnemyState.None
    private void Start()
    {
        if(movement.HasWaypoints)
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

        UpdateVisionData();
    }
    
    
    private void UpdateVisionData()
    {
        if (vision == null)
            return;

        if (vision.CanSeePlayer && vision.PlayerTransform != null)
        {
            target = vision.PlayerTransform;
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

        OnChangeState?.Invoke(previousState,currentState);

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
            case EnemyState.Chase:
                TickChase();
                break;

        }        
    }

    //Metoda do wchodzenia / rozpoczynania stanów 
    //Ma ona wywołac konkretną funkcje zachodząco podczas sprecyzowanego stanu
    private void EnterState(EnemyState state)
    {   
        switch(state)
        {
            
            case EnemyState.Idle:
                movement.Stop();
                break;

            case EnemyState.Patrol:
                if (movement.HasWaypoints)
                {
                    movement.Walk();
                    movement.MoveTo(movement.CurrentWaypoint());
                }
                break;
            case EnemyState.Suspicious:
                movement.Stop();
                break;
        
            case EnemyState.Chase:
                movement.Run();
                break;

        }
    }

    //Metoda do wychodzenia / zamykania konkretnych statów
    //jeśli będzie potrzeba to musi wyłączyć konkretną akcje 
    // np: mogło by wyłączyć animacje idle / albo animacje walk itp
    private void ExitState(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.Idle:
            
                break;

            case EnemyState.Suspicious:
                
                break;
        }
    }

    // co sie tu dzieje :
    // jeśli jestem w TickIdle to:
    private void TickIdle()
    {
        //oczekauje aż timer(stoper) osiągnie swoją wartość
        //potem wracam do trybu patrol
        if(stateTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    // co sie tu dzieje :
    // jeśli jestem w TickPatrol() to:
    private void TickPatrol()
    {
        //widze gracz , przechodze TickSuspicious 
        if (CanSeePlayer())
        {
            ChangeState(EnemyState.Suspicious);
            return;
        }
        //jeśli nie posiadam waypointa to przechodze w TickIdle
        if (!movement.HasWaypoints)
        {
            ChangeState(EnemyState.Idle);
            return;
        }
        // jeśli dotarłem do waypointa to przechodze w TickIdle
        if (movement.HasReachedDestination())
        {   
            movement.SetNextPatrolDestination();
            ChangeState(EnemyState.Idle);
        }
    }

    // co sie tu dzieje :
    // jeśli jestem w TickSuspicious i przestaje widziec gracza to wracam do patrolu
    private void TickSuspicious()
    {
        
        if (!CanSeePlayer())
        {
            ChangeState(EnemyState.Patrol);
        }
        if(suspiciousDuration <= stateTimer)
        {
            ChangeState(EnemyState.Chase);
        }
        
    }

    private void TickChase()
    {
        if (CanSeePlayer())
        {
            movement.MoveTo(vision.LastKnownPlayerPosition);
        }
        else
        {
            movement.MoveTo(vision.LastKnownPlayerPosition);
            if (movement.HasReachedDestination())
            {
                ChangeState(EnemyState.Patrol);
            }
        }
    }

    private bool CanSeePlayer()
    {
        return vision != null &&
               vision.CanSeePlayer &&
               vision.PlayerTransform != null;
    }

}
