using UnityEngine;

public class placement : MonoBehaviour
{
    public GameObject wall;
    public float placedistance = 5f;
    public GameObject player;
    public Camera cam;

    // Kosten van de muur
    public string requiredResource = "Wood";
    public int requiredAmount = 1;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Place();
        }
    }

    void Place()
    {
        // Check of inventory bestaat
        if (InventoryManager.instance == null)
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }

        // Check of er genoeg resources zijn
        if (!HasEnoughResources())
        {
            Debug.Log("Not enough resources to place wall!");
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placedistance))
        {
            float playerRot = player.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, playerRot, 0);

            Instantiate(wall, hit.point, rotation);

            // Resources verwijderen NA plaatsen
            InventoryManager.instance.RemoveItems(requiredResource, requiredAmount);
        }
    }

    bool HasEnoughResources()
    {
        // Loop door inventory
        foreach (var item in InventoryManager.instance
                     .GetType()
                     .GetField("inventoryArray", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                     .GetValue(InventoryManager.instance) as System.Collections.Generic.List<System.Collections.Generic.List<string>>)
        {
            if (item[1] == requiredResource)
            {
                int amount = int.Parse(item[2]);
                return amount >= requiredAmount;
            }
        }

        return false;
    }
}
