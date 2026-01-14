using UnityEngine;

[RequireComponent(typeof(SimpleInventory))]
public class ResourceGathering : MonoBehaviour
{
    public float gatherRange = 3f;
    public LayerMask resourceLayer;
    public Camera playerCamera;

    private SimpleInventory inventory;

    void Start()
    {
        inventory = GetComponent<SimpleInventory>();
    }

    void Update()
    {
        // "Fire1" is left mouse button by default in the old input system
        if (Input.GetButtonDown("Fire1"))
        {
            TryGather();
        }
    }

    void TryGather()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, gatherRange, resourceLayer))
        {
            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null)
            {
                int amount = resource.Gather();
                inventory.AddResource(resource.resourceName, amount);
                Debug.Log($"Gathered {amount}x {resource.resourceName}");
            }
        }
        else
        {
            Debug.Log("No resource in range!");
        }
    }
}
