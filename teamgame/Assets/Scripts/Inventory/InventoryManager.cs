using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    public GameObject inventoryUI;

    private List<List<string>> inventoryArray = new List<List<string>>();

    public InventorySlot[] slots;

    private void Awake()
    {
        inventoryUI.SetActive(false);
        instance = this;

        if (slots == null || slots.Length == 0)
        {
            slots = inventoryUI.GetComponentsInChildren<InventorySlot>();
        }
    }

    bool ItemExcists(string itemName)
    {
        foreach (List<string> item in inventoryArray)
        {
            if (item[1] == itemName)
            {
                return true;
            }
        }

        return false;
    }

    public void AddItem(string itemName, int maxStackSize)
    {
        if (inventoryArray.Count >= slots.Length)
        {
            Debug.Log("Inventory Full!");
            return;
        }

        for (int i = 0; i < inventoryArray.Count; i++)
        {
            List<string> item = inventoryArray[i];

            if (item[1] == itemName)
            {
                int currentAmount = int.Parse(item[2]);
                if (currentAmount < maxStackSize)
                {
                    currentAmount += 1;
                    item[2] = currentAmount.ToString();
                    return;
                }
            }    
        }

        if (inventoryArray.Count < slots.Length)
        {
            AddNewStack(itemName);
        }
    }

    private void AddNewStack(string itemName)
    {
        List<string> newItem = new List<string>();
        newItem.Add("ID_" + (inventoryArray.Count + 1));
        newItem.Add(itemName);
        newItem.Add("1"); //starting amount is 1
        inventoryArray.Add(newItem);
    }

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        if (inventoryUI.activeSelf)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        // Clear all slots first
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        // Fill slots with items
        for (int i = 0; i < inventoryArray.Count && i < slots.Length; i++)
        {
            string itemName = inventoryArray[i][1];
            int amount = int.Parse(inventoryArray[i][2]);

            slots[i].SetSlot(itemName, amount);
        }
    }
}
