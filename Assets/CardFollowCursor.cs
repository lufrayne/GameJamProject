using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFollowCursor : MonoBehaviour
{
    public LayerMask floorLayer; // layer containing the floor object
    public float raycastDistance = 100f; // maximum distance of the raycast
    public float smoothTime = 0.01f; // how quickly to move towards the target position

    private Vector3 targetPosition;
    private Vector3 currentVelocity;

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
            // set the target position to the hit point on the floor
            targetPosition = hit.point;
        }

        // smoothly move towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

    }
}   
