using UnityEngine;

public class PlayerControlsScript : MonoBehaviour
{
    public InventoryManager inventoryManager;

    void Start()
    {
        if (inventoryManager == null)
        {
            // Veranderd van FindObjectOfType naar FindAnyObjectByType
            inventoryManager = Object.FindAnyObjectByType<InventoryManager>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Zorg dat de InventoryManager een 'instance' heeft in zijn eigen script
            if (InventoryManager.instance != null)
            {
                InventoryManager.instance.ToggleInventory();
            }
        }   
    }
}