using UnityEngine;

//[RequireComponent(typeof(InventoryManager))]
public class ResourceGathering : MonoBehaviour
{
    public float gatherRange = 3f;
    public LayerMask resourceLayer;
    public Camera playerCamera;

    public InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GetComponentInChildren<InventoryManager>();
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
                InventoryManager.instance.AddItem(resource.resourceName, resource.maxStackSize);
            }
        }
        else
        {
            Debug.Log("No resource in range!");
        }
    }
}
