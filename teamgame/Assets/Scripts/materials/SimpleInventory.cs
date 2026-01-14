using UnityEngine;
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    public void AddResource(string name, int amount)
    {
        if (amount <= 0) return;

        if (!resources.ContainsKey(name))
            resources[name] = 0;

        resources[name] += amount;

        Debug.Log($"Inventory: {name} = {resources[name]}");
    }

    public int GetResourceAmount(string name)
    {
        return resources.ContainsKey(name) ? resources[name] : 0;
    }
}
