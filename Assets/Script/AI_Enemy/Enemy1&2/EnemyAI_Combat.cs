using UnityEngine;

public class EnemyAI_Combat : MonoBehaviour
{

    [SerializeField] private OnTriggerEnterCombatDetection combatTrigger;
    [SerializeField] private int damage = 10;
    private IDamageable currentTarget;

    public bool IsPlayerInAttackRange => currentTarget!= null;

    private void Awake()
    {
        if(combatTrigger == null)
        {
            combatTrigger = GetComponentInChildren<OnTriggerEnterCombatDetection>();
        }
    }


    private void OnEnable()
    {
        if(combatTrigger != null)
        {
            combatTrigger.InAttackRange += HandleCombatSignal;
        }

    }

    private void OnDisable()
    {
        if(combatTrigger != null)
        {
            combatTrigger.InAttackRange -= HandleCombatSignal;
        }
        currentTarget= null;
    }

    private void HandleCombatSignal(IDamageable target)
    {
        currentTarget= target;

    }

    public void Attack()
    {
        if(currentTarget== null ) return;
        currentTarget.TakeDamage(damage);
    }

}