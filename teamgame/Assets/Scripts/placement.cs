using UnityEngine;

public class placement : MonoBehaviour
{
    public GameObject wall;
    public float placedistance = 5f;
    public GameObject player;
    public Camera cam;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Place();
        }
    }
    void Place()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placedistance))
        {
            float playerRot = player.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, playerRot, 0);
            Instantiate(wall, hit.point, rotation);
        }
    }
}
