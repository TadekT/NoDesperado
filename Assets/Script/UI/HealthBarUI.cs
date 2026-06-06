using UnityEngine;

public class HealthBarUI : MonoBehaviour
{

    public float Health =100;
    public float MaxHealth = 100;
    public float Width = 250;
    public float Height = 50;

    [SerializeField] private RectTransform healthBar;

    public void SetMaxHealth(float maxHealth)
    {
         MaxHealth = Mathf.Max(1f, maxHealth);
    }

    public void SetHealth(float health)
    {   
        if (MaxHealth <= 0f || healthBar == null) return;
        Health = health;
        float x = Health / MaxHealth;
        float newWidth = x * Width;
        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}   
