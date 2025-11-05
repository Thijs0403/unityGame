using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private InventoryManager inventoryManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    //Hallo Sjaak

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {   
            inventoryManager.DebugPrint();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventoryManager.AddItem("appel", "lekker fruit");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventoryManager.AddItem("swaard", "wapen om mee te doden");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventoryManager.AddItem("schild", "een stuk hout");
        }
    }
}
