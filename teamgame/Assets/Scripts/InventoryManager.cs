using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Singleton instance so other scripts can easily access the InventoryManager
    public static InventoryManager instance { get; private set; }

    // Reference to the Inventory UI GameObject
    public GameObject inventoryUI;

    // Inventory data structure:
    // Each item is a List<string> containing:
    // [0] = ID
    // [1] = Item name
    // [2] = Amount
    private List<List<string>> inventoryArray = new List<List<string>>();

    // Array of UI slots used to display inventory items
    public InventorySlot[] slots;

    private void Awake()
    {
        // Hide inventory UI when the game starts
        inventoryUI.SetActive(false);

        // Set the singleton instance
        instance = this;

        // Automatically find InventorySlot components if none are assigned
        if (slots == null || slots.Length == 0)
        {
            slots = inventoryUI.GetComponentsInChildren<InventorySlot>();
        }
    }

    // Checks if an item already exists in the inventory
    bool ItemExcists(string itemName)
    {
        foreach (List<string> item in inventoryArray)
        {
            // item[1] = item name
            if (item[1] == itemName)
            {
                return true;
            }
        }

        return false;
    }

    // Adds an item to the inventory
    public void AddItem(string itemName, int maxStackSize)
    {
        // Prevent adding items if inventory is full
        if (inventoryArray.Count >= slots.Length)
        {
            Debug.Log("Inventory Full!");
            return;
        }

        // Try to add the item to an existing stack
        for (int i = 0; i < inventoryArray.Count; i++)
        {
            List<string> item = inventoryArray[i];

            // Check if this stack matches the item name
            if (item[1] == itemName)
            {
                int currentAmount = int.Parse(item[2]);

                // Increase stack amount if under max stack size
                if (currentAmount < maxStackSize)
                {
                    currentAmount += 1;
                    item[2] = currentAmount.ToString();
                    return;
                }
            }
        }

        // If no existing stack was found or stacks are full, add a new one
        if (inventoryArray.Count < slots.Length)
        {
            AddNewStack(itemName);
        }
    }

    // Creates a new stack for an item
    private void AddNewStack(string itemName)
    {
        List<string> newItem = new List<string>();

        // Generate a simple ID
        newItem.Add("ID_" + (inventoryArray.Count + 1));

        // Store item name
        newItem.Add(itemName);

        // Starting amount is 1
        newItem.Add("1");

        // Add item to inventory
        inventoryArray.Add(newItem);
    }
    
    public void RemoveItems(string itemName, int amountToRemove)
    {
        for (int i = 0; i < inventoryArray.Count; i++)
        {
            List<string> item = inventoryArray[i];
            // Check if this stack matches the item name
            if (item[1] == itemName)
            {
                int currentAmount = int.Parse(item[2]);
                if (currentAmount >= amountToRemove)
                {
                    currentAmount -= amountToRemove;
                    item[2] = currentAmount.ToString();
                    // Remove the stack if amount reaches zero
                    if (currentAmount == 0)
                    {
                        inventoryArray.RemoveAt(i);
                    }
                    return;
                }
                else
                {
                    Debug.Log("Not enough items to remove!");
                    return;
                }
            }
        }
        Debug.Log("Item not found in inventory!");
    }


    // Shows or hides the inventory UI
    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        // Update UI when inventory is opened
        if (inventoryUI.activeSelf)
        {
            UpdateUI();
        }
    }

    // Updates all inventory UI slots
    public void UpdateUI()
    {
        // Clear all slots first
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        // Fill slots with inventory data
        for (int i = 0; i < inventoryArray.Count && i < slots.Length; i++)
        {
            string itemName = inventoryArray[i][1];
            int amount = int.Parse(inventoryArray[i][2]);

            slots[i].SetSlot(itemName, amount);
        }
    }
}
