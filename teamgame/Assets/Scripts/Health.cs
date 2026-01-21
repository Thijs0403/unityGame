using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        OnDeath.Invoke();
        // For zombies, you might play an animation; for now, we destroy.
        Destroy(gameObject);
    }
}