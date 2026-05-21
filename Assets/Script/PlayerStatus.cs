using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    [SerializeField] private int maxHealth = 100; 


    private int currentHealth;

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
