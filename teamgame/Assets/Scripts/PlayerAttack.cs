using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum WeaponType { Hands, Spear, Gun } // De verschillende wapens
    public WeaponType currentWeapon;

    [Header("Weapon Stats")]
    public float damage;
    public float range;
    public float attackRate;
    private float nextAttackTime = 0f;

    private Health playerHealth;

    void Start()
    {
        playerHealth = GetComponent<Health>();
        SetWeaponStats(); // Zet de stats goed bij het opstarten
    }

    // Deze functie past de stats aan op basis van je wapen
    void SetWeaponStats()
    {
        if (currentWeapon == WeaponType.Hands) {
            damage = 10f; range = 1.5f; attackRate = 2f;
        }
        else if (currentWeapon == WeaponType.Spear) {
            damage = 25f; range = 3.5f; attackRate = 1f;
        }
        else if (currentWeapon == WeaponType.Gun) {
            damage = 50f; range = 20f; attackRate = 0.5f;
        }
    }

    void Update()
    {
        // Wisselen van wapen met de nummertoetsen (1, 2, 3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeapon = WeaponType.Hands; SetWeaponStats(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentWeapon = WeaponType.Spear; SetWeaponStats(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentWeapon = WeaponType.Gun; SetWeaponStats(); }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Zombie zombie = hit.transform.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.currentHealth -= damage;
                
                // Update de balk van de zombie die je op dat moment slaat
                if (playerHealth != null && playerHealth.zombieHealthSlider != null)
                {
                    playerHealth.zombieHealthSlider.gameObject.SetActive(true);
                    playerHealth.zombieHealthSlider.value = zombie.currentHealth;
                }

                if (zombie.currentHealth <= 0) 
                {
                    playerHealth.zombieHealthSlider.gameObject.SetActive(false);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}