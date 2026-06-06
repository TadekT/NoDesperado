using UnityEngine;
[RequireComponent(typeof(HealthBarUI))]
public class PlayerStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] private int maxHealth = 100; 


    [SerializeField]  private int currentHealth;

    public bool IsHidden {get ; private set;}



    void Awake()
    {
        
        currentHealth = maxHealth;        
        if(healthBar != null)
        {
        healthBar.SetMaxHealth(maxHealth);    
        }

    }



    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0,currentHealth - amount);
        if(healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

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
        SetHidden(false);
        gameObject.SetActive(false);

    }

    public void SetHidden(bool hidden)
    {
        IsHidden = hidden;
    }

}
