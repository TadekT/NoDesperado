using System;
using UnityEngine;

// komponenty wymagane do działania skryptu
[RequireComponent(typeof(EnemyAI3_Movement))]
[RequireComponent(typeof(EnemyAI3_Vision))]
[RequireComponent(typeof(EnemyAI3_Combat))]

public class EnemyAI3_Brain : MonoBehaviour
{
#region Variables
    public event Action<EnemyState,EnemyState> OnChangeState3;

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
    [SerializeField] private EnemyAI3_Movement movement;

    //Referencja do skryptu EnemyAI_Vision, aby odczytywać dane z wizji przeciwnika AI
    [SerializeField] private EnemyAI3_Vision vision;
    [SerializeField] private EnemyAI3_Combat combat;


    [Header("Idle")]
    //Zmienna określająca wartość długości stanu idle, odliczany poprzes porównanie z stateTimer 
    [SerializeField] private float idleDuration = 2f;


    [Header("Suspicious")]
    [SerializeField] private float suspiciousDuration = 2f;

    [Header("Search")]
    [SerializeField] private int maxRandomSearchPoints = 2;

    [Header("Chase")]
    [SerializeField] private float minChaseDuration = 2f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1f;


    private int searchPointsVisited = 0;

    [Header("State timer for debug")]
    // Ogólny licznik do odczytania Time.deltatime, do zerowania i sterowania TickStatem
    [SerializeField] private float stateTimer;

    // ostatnia znana pozycja gracza, do urzytku w TickState chase
    private Vector3 lastKnownPosition; 
    private Transform _cachedPlayerTransform;
#endregion





#region Awake Method
    //Sprawdzanie poprawności przypisania referencji
    private void Awake()
    {

        if(movement == null)
        {
            movement = GetComponent<EnemyAI3_Movement>();
        }
        
        if(vision == null)
        {
            vision = GetComponent<EnemyAI3_Vision>();
        }

        if(combat == null)
        {
            combat = GetComponent<EnemyAI3_Combat>();
        }

    }
#endregion





#region Start Method
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
#endregion





#region Update Method
    private void Update()
    {
        stateTimer += Time.deltaTime;

        UpdateVisionData();
        TickState(currentState);

    }
#endregion
    




#region UpdateVisionData Method
    private void UpdateVisionData()
    {
        if (vision == null) return;

        if (vision.CanSeePlayer && vision.PlayerTransform != null)
        {
            lastKnownPosition = vision.LastKnownPlayerPosition;
            _cachedPlayerTransform = vision.PlayerTransform;
        }
        else
        {
            _cachedPlayerTransform = null;
        }
    }
#endregion





#region ChangeState Method
    private void ChangeState(EnemyState nextState)
    {
        if(nextState == currentState) 
            return;

        previousState = currentState;
        ExitState(previousState);
        
        stateTimer = 0;
        currentState = nextState;
        
        EnterState(nextState);

        OnChangeState3?.Invoke(previousState,currentState);
        
    }
#endregion
    




#region TickState Method
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
            
            case EnemyState.Search:
                TickSearch();
                break;
            
            case EnemyState.Attack:
                TickAttack();
                break;

        }        
    }
#endregion



#region EnterState Method
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
                    movement.PatrolDistance();
                    movement.Walk();
                    movement.MoveTo(movement.CurrentWaypoint());
                }
                break;

            case EnemyState.Suspicious:
                movement.Stop();
                break;
        
            case EnemyState.Chase:
                movement.ChaseDistance();
                movement.Run();
                vision.IsAlerted = true;
                break;

            case EnemyState.Search:
                if (lastKnownPosition != Vector3.zero)
                {
                    movement.MoveToRandomPosition(lastKnownPosition);
                }
                break;

            case EnemyState.Attack:
                vision.IsAlerted = true;
                movement.Stop();
                break;
        }
    }
#endregion





#region ExitState Method
    //Metoda do wychodzenia / zamykania konkretnych statów
    //jeśli będzie potrzeba to musi wyłączyć konkretną akcje 
    // np: mogło by wyłączyć animacje idle / albo animacje walk itp
    private void ExitState(EnemyState state)
    {
        switch(state)
        {

            case EnemyState.Chase:
                vision.IsAlerted = false;
                break;
           
            case EnemyState.Search:
                searchPointsVisited = 0;
                break;
        }
    }
#endregion





#region TickIdle Method
    // co sie tu dzieje :
    // jeśli jestem w TickIdle to: odliczam timer a potem patrol
    private void TickIdle()
    {
        //oczekauje aż timer(stoper) osiągnie swoją wartość
        //potem wracam do trybu patrol
        if(stateTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }
#endregion





#region TickPatrol Method
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
#endregion

    // co sie tu dzieje :
    // jeśli jestem w TickSuspicious i przestaje widziec gracza to wracam do patrolu




#region TickSuspicious Method
    private void TickSuspicious()
    {
        if(!CanSeePlayer())
        {
            ChangeState(EnemyState.Search);
            return;
        }
        
        movement.FaceToTarget(_cachedPlayerTransform.position); 
        
        if(suspiciousDuration <= stateTimer)
        {
            ChangeState(EnemyState.Chase);
        }
        
    }
#endregion




#region TickChase Method
    private void TickChase()
    {
        if (combat.IsPlayerInAttackRange && CanSeePlayer())
        {
            ChangeState(EnemyState.Attack);
            return;
        }
        if (CanSeePlayer())
        {
            movement.MoveTo(_cachedPlayerTransform.position);
            movement.FaceToTarget(_cachedPlayerTransform.position);
        }
        else
        {
            movement.MoveTo(vision.LastKnownPlayerPosition);
            if (movement.HasReachedDestination() && stateTimer >= minChaseDuration)
            {
                ChangeState(EnemyState.Search);
            }
        }
    }

    #endregion




#region TickSearch Method

    private void TickSearch()
    {       
        if (CanSeePlayer())
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        if (movement.HasReachedDestination())
        {
            searchPointsVisited++;
            if(searchPointsVisited >= maxRandomSearchPoints)
            {
                ChangeState(EnemyState.Patrol);
            }
            else
            {
                if (lastKnownPosition != Vector3.zero)
                {
                    movement.MoveToRandomPosition(lastKnownPosition);
                }
            }
        }
    }
#endregion




#region TickAttack Method
    private void TickAttack()
    {
        // gracz wyszedł z zasięgu — wróć do Chase
        if (!combat.IsPlayerInAttackRange || !CanSeePlayer())
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        movement.FaceToTarget(_cachedPlayerTransform.position);
        // bij co attackCooldown sekund
        if (stateTimer >= attackCooldown)
        {
            combat.Attack();
            stateTimer = 0f;
        }
    }
#endregion





#region CanSeePlayer Method
    private bool CanSeePlayer()
    {
        return vision != null &&
               vision.CanSeePlayer &&
               _cachedPlayerTransform != null;
    }
#endregion

}