using UnityEngine;

public class Resource : MonoBehaviour
{
    public string resourceName;
    public int amountPerHit;
    public int maxHits;
    public int maxStackSize;

    private int currentAmount;

    void Start()
    {
        currentAmount = maxHits;
    }

    public int Gather()
    {
        if (currentAmount <= 0)
        {
            Debug.Log(resourceName + " is depleted!");
            return 0;
        }

        int gathered = Mathf.Min(amountPerHit, currentAmount);
        currentAmount -= gathered;

        if (currentAmount <= 0)
        {
            // Optional: Destroy after depletion
            Destroy(gameObject);
        }

        return gathered;
    }
}

