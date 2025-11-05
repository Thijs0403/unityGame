using UnityEngine;

public class Resource : MonoBehaviour
{
    public string resourceName = "Wood";
    public int amountPerHit = 1;
    public int maxAmount = 5;

    private int currentAmount;

    void Start()
    {
        currentAmount = maxAmount;
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

