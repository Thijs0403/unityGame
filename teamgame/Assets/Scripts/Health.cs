using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Nodig voor de vertraging

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    [Header("Death UI")]
    public GameObject deathText; // Sleep je DeathText hiernaartoe
    public Transform spawnPoint; // Sleep je Spawnpoint hiernaartoe

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthSlider != null) healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        Debug.Log("Speler is dood!");
        
        // 1. Laat de tekst zien
        if (deathText != null) deathText.SetActive(true);

        // 2. Wacht 3 seconden zodat de speler de tekst kan lezen
        yield return new WaitForSeconds(3f);

        // 3. Reset de speler
        transform.position = spawnPoint.position;
        currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = maxHealth;
        
        // 4. Maak de tekst weer onzichtbaar
        if (deathText != null) deathText.SetActive(false);
    }
}