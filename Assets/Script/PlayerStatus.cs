using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable
{

    [SerializeField] private int maxHealth = 100; 


    [SerializeField]  private int currentHealth;

    void Awake()
    {
        
        currentHealth = maxHealth;

    }



    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (IsDead())
        {
            Die();
        }
    }

    private bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die()
    {
        Debug.Log("Player is dead");
    }



}
