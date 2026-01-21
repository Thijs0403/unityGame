using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum WeaponType { Hands, Spear, Gun }
    [Header("Current State")]
    public WeaponType currentWeapon;

    [Header("Weapon Stats")]
    public float damage = 10f;
    public float range = 2.0f;
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Models")]
    public GameObject spearModel; // Sleep je speer object hierheen

    private Health playerHealth; 

    void Start()
    {
        playerHealth = GetComponent<Health>();
        UpdateWeaponStats();
    }

    void Update()
    {
        // Check elk frame welk wapen geselecteerd is in de InventoryManager
        CheckInventoryForWeapon();
        
        UpdateWeaponStats();

        // Aanvallen met linker muisknop
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void CheckInventoryForWeapon()
    {
        // We gebruiken de 'instance' van je InventoryManager om te kijken wat je hebt
        // Let op: Dit werkt als je item in de inventory precies "Spear" heet
        
        // Voor nu kijken we handmatig of we een item hebben (je kunt dit later koppelen aan een 'SelectedSlot')
        currentWeapon = WeaponType.Hands; // Standaard handen

        // Voorbeeld: Als je een systeem hebt dat bijhoudt welk item actief is, pas je dit hier aan
        // Omdat je InventoryManager nog geen "SelectedItem" heeft, kun je dit handmatig testen of 
        // een variabele toevoegen aan InventoryManager.
    }

    void UpdateWeaponStats()
    {
        switch (currentWeapon)
        {
            case WeaponType.Hands:
                damage = 10f; range = 1.8f; attackRate = 2f;
                if(spearModel != null) spearModel.SetActive(false);
                break;
            case WeaponType.Spear:
                damage = 30f; range = 4.0f; attackRate = 1f;
                if(spearModel != null) spearModel.SetActive(true);
                break;
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

                if (playerHealth != null && playerHealth.zombieHealthSlider != null)
                {
                    playerHealth.zombieHealthSlider.gameObject.SetActive(true);
                    playerHealth.zombieHealthSlider.maxValue = zombie.maxHealth;
                    playerHealth.zombieHealthSlider.value = zombie.currentHealth;
                }

                if (zombie.currentHealth <= 0)
                {
                    if (playerHealth != null && playerHealth.zombieHealthSlider != null)
                        playerHealth.zombieHealthSlider.gameObject.SetActive(false);
                        
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}