using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{

    public float Health;
    public float MaxHealth;
    public float Width;
    public float Height;

    [SerializeField] private RectTransform healthBar;

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        Health = health;
        float x = Health / MaxHealth;
        float newWidth = x * Width;
        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}   
