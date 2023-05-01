using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject groundPlane;
    private GameObject cardBeam;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isFallingTx = false;
    private bool isFallingTy = false;
    private bool isFallingTz = false;
    private bool isFallingR = false;
    private float movingRate = .05f;
    private float fallingRateT = .04f;
    private float fallingRateR = .04f;
    private float fallStepMinT = 1f;
    private float fallStepMinR = 1f;
    private Ray ray;
    private RaycastHit hit;
    private Plane ground;
    private float distance;
    private Vector3 hitPoint;
    private float targetTx;
    private float targetTy;
    private float targetTz;
    private Quaternion targetR;
    private float stepTx;
    private float stepTy;
    private float stepTz;
    private Quaternion stepR;

    void Start()
    {
        groundPlane = GameObject.Find("Ground Plane");
        ground = new Plane(Vector3.up, groundPlane.transform.position);

        cardBeam = GameObject.Find("CardBeam");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    offset = transform.position - hit.point;
                    isDragging = true;
                }
            }
        }

        if (isDragging)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (ground.Raycast(ray, out distance))
            {
                // Gradually move above the ground target
                hitPoint = ray.GetPoint(distance);
                targetTx = hitPoint.x + offset.x;
                targetTy = transform.position.y;
                targetTz = hitPoint.z + offset.z;
                stepTx = transform.position.x + movingRate * (targetTx - transform.position.x);
                stepTy = transform.position.y + movingRate * (targetTy - transform.position.y);
                stepTz = transform.position.z + movingRate * (targetTz - transform.position.z);
                transform.position = new Vector3(stepTx, stepTy, stepTz);
                cardBeam.transform.position = new Vector3(stepTx, cardBeam.transform.position.y, stepTz);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isFallingTx = true;
                isFallingTy = true;
                isFallingTz = true;
                isFallingR = true;
            }
            isDragging = false;
        }

        if (isFallingTx || isFallingTy || isFallingTz || isFallingR)
        {
            // Gradually move down
            targetTx = hitPoint.x + offset.x;
            targetTy = groundPlane.transform.position.y + .5f;
            targetTz = hitPoint.z + offset.z;
            stepTx = transform.position.x + fallingRateT * (targetTx - transform.position.x);
            stepTy = transform.position.y + fallingRateT * (targetTy - transform.position.y);
            stepTz = transform.position.z + fallingRateT * (targetTz - transform.position.z);
            if (stepTx < fallStepMinT)
            {
                stepTx = targetTx;
                isFallingTx = false;
            }
            if (stepTy < fallStepMinT)
            {
                stepTy = targetTy;
                isFallingTy = false;
            }
            if (stepTz < fallStepMinT)
            {
                stepTz = targetTz;
                isFallingTz = false;
            }
            transform.position = new Vector3(stepTx, stepTy, stepTz);
            cardBeam.transform.position = new Vector3(stepTx, stepTy-10, stepTz);

            // Gradually rotate flat
            targetR = Quaternion.Euler(0, -180, 0);
            float angle = Quaternion.Angle(transform.rotation, targetR);
            if (angle < fallStepMinR)
            {
                stepR = targetR;
                isFallingR = false;
            }
            else
            {
                stepR = Quaternion.Lerp(transform.rotation, targetR, fallingRateR);
            }
            transform.rotation = stepR;
        }

    }
}
