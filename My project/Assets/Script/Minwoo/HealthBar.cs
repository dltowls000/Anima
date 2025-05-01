using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthBarController healthBarController;  
    public float maxHealth; 
    public float currentHealth;

    public void Initialize(float maxStamina)
    {
        maxHealth = maxStamina;
        currentHealth = maxHealth;
        healthBarController.UpdateHealth(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; 
        }

        healthBarController.UpdateHealth(currentHealth / maxHealth);
    }
}
