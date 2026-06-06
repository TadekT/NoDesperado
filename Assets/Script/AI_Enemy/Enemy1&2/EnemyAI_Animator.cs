using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(EnemyAI_Brain))]
public class EnemyAI_Animator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAI_Brain brain;
    [SerializeField] private NavMeshAgent agent;

    private static readonly int Speed = Animator.StringToHash("speed");

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();    
        } 
        if (brain == null)
        {
            brain = GetComponent<EnemyAI_Brain>();
        }
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }   
    }

    private void Update()
    {
        // prędkość agenta → parametr animatora
        if(animator != null && agent != null)
        {
            animator.SetFloat(Speed, agent.velocity.magnitude);
        }
    }
}