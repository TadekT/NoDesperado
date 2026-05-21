using UnityEngine;

public class EnemyAI_Combat : MonoBehaviour
{

    [SerializeField] private OnTriggerEnterCombatDetection combatTrigger;
    [SerializeField] private int damage = 10;
    private IDamageable curretTargget;

    public bool IsPlayerInAttackRange => curretTargget != null;

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
    }

    private void HandleCombatSignal(IDamageable target)
    {
        curretTargget = target;

    }

    public void Attack()
    {
        if(curretTargget == null ) return;
        curretTargget.TakeDamage(damage);
    }

}
