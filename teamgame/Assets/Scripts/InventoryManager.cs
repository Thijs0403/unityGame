using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<List<string>> inventoryArray = new List<List<string>>();

    public void AddItem(string itemName, string description)
    {
        List<string> newItem = new List<string>();
        newItem.Add(itemName + "_" + (inventoryArray.Count + 1));
        newItem.Add(description);
        newItem.Add("ID_" + (inventoryArray.Count + 1));
        inventoryArray.Add(newItem);

        Debug.Log("Added a new item!");
    }

    public void DebugPrint()
    {
        for (int i = 0; i < inventoryArray.Count; i++)
        {
            List<string> item = inventoryArray[i];
            Debug.Log("Item " + i + ": " + item[0] + ", " + item[1] + ", " + item[2]);
        }
    }
}
