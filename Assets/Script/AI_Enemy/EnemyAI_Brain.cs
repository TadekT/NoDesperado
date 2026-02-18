using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI_Brain : MonoBehaviour
{
[SerializeField] private enum State
{
    Idle, Patrol, Search, Chase, Attack
}

[SerializeField]private EnemyAI_Movment _Movment;





private State currentState;



    private void Start()
    {
        
    }

}
