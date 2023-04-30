using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset;
    private bool isDragging = false;
    private GameObject groundPlane;

    void Start()
    {
        groundPlane = GameObject.Find("Ground Plane");
    }
        // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                    if (hit.transform == transform)
                    {
                        offset = transform.position - hit.point;
                        isDragging = true;
                    }
            }
        }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            if (isDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane ground = new Plane(Vector3.up, groundPlane.transform.position);
                float distance;
                if (ground.Raycast(ray, out distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    float distanceFromGround = hitPoint.y - groundPlane.transform.position.y;
                    transform.position = new Vector3(hitPoint.x + offset.x, groundPlane.transform.position.y + distanceFromGround, hitPoint.z + offset.z);
                }
            }
     
    }
}
