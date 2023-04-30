using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFollowCursor : MonoBehaviour
{
    public LayerMask floorLayer; // layer containing the floor object
    public float raycastDistance = 100f; // maximum distance of the raycast

    // Start is called before the first frame update
    void Start()
    {
       
    } 

    void Update()
    {
        // create a ray from the floor to the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // check if the ray hits the floor layer
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, floorLayer))
        {
            // move this object to the hit point on the floor
            transform.position = hit.point;
        }

    }
}   
