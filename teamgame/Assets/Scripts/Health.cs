using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Player Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider playerHealthSlider; 

    [Header("Zombie Health UI")]
    public Slider zombieHealthSlider; // De balk die alleen verschijnt bij een gevecht

    [Header("Death & Respawn")]
    public GameObject deathText;      // De "YOU ARE DEAD" tekst
    public Transform spawnPoint;      // Je lege object 'Spawnpoint' in de scene

    void Start()
    {
        currentHealth = maxHealth;
        
        // Zet alles goed aan het begin
        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = maxHealth;
            playerHealthSlider.value = maxHealth;
        }

        // Zorg dat de dood-tekst en zombie-balk standaard uit staan
        if (deathText != null) deathText.SetActive(false);
        if (zombieHealthSlider != null) zombieHealthSlider.gameObject.SetActive(false);
    }

    // Aangepaste TakeDamage die weet welke zombie aanvalt
    public void TakeDamage(float amount, Zombie attackingZombie = null)
    {
        currentHealth -= amount;
        if (playerHealthSlider != null) playerHealthSlider.value = currentHealth;

        // Laat de healthbar van de zombie zien
        if (zombieHealthSlider != null && attackingZombie != null)
        {
            zombieHealthSlider.gameObject.SetActive(true);
            zombieHealthSlider.maxValue = attackingZombie.maxHealth;
            zombieHealthSlider.value = attackingZombie.currentHealth;
        }

        Debug.Log("Speler HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        // Toon de tekst pas als je echt dood bent
        if (deathText != null) deathText.SetActive(true);

        yield return new WaitForSeconds(3f); // Wacht even voor de speler

        // Reset positie en health
        transform.position = spawnPoint.position;
        currentHealth = maxHealth;
        
        if (playerHealthSlider != null) playerHealthSlider.value = maxHealth;
        if (deathText != null) deathText.SetActive(false);
        if (zombieHealthSlider != null) zombieHealthSlider.gameObject.SetActive(false);
    }
}