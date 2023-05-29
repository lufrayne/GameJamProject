using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragScript : MonoBehaviour
{
    // Public fields
    public float dragFollowingRate;
    public float fallingRateDown;
    public float fallingRateSliding;
    public float fallingRateRotating;
    public AudioSource audioSource;
    public AudioClip cardPickupSound;
    public AudioClip cardDropSound;

    // Private variables
    private GameObject groundObj;
    private Plane groundPlane;
    private GameObject tableObj;
    private Plane tablePlane;
    private GameObject cardBeam;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 offset;
    private float distance;
    private Vector3 hitPoint;

    private float startingY;
    private float startingZ;
    private float startingBeamY;
    private float targetTx;
    private float targetTy;
    private float targetTz;
    private float targetBeamTy;
    private Quaternion targetR;
    private float stepTx;
    private float stepTy;
    private float stepTz;
    private float stepBeamTy;
    private Quaternion stepR;

    public bool isDragging = false;
    public bool isOverGround = false;
    private bool isFallingTx = false;
    private bool isFallingTy = false;
    private bool isFallingTz = false;
    private bool isFallingR = false;
    private bool isPlayingAudio = false;
    public bool hasLanded = true;

    // Start is called before the first frame update
    void Start()
    {
        groundObj = GameObject.Find("Ground");
        groundPlane = new Plane(Vector3.up, groundObj.transform.position);

        tableObj = GameObject.Find("Table");
        tablePlane = new Plane(Vector3.up, tableObj.transform.position);

        cardBeam = GameObject.Find("CardBeam");

        startingY = transform.position.y;
        startingZ = transform.position.z;

        startingBeamY = cardBeam.transform.position.y;
    }

    private IEnumerator PlayAudioWithDelayCoroutine(AudioClip clip, float delay)
    {
        // Play the audio clip
        audioSource.clip = clip;
        audioSource.Play();

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Set the flag to indicate that we're done playing audio
        isPlayingAudio = false;
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
                    isDragging = true;
                    offset = transform.position - hit.point;

                    // Set the flag to indicate that we're playing audio
                    isPlayingAudio = true;
                    StartCoroutine(PlayAudioWithDelayCoroutine(cardPickupSound, .1f));
                }
            }
        }

        if (!isPlayingAudio)
        {
            if (isDragging)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if ((isOverGround && hit.transform == tableObj.transform) || (!isOverGround && hit.transform != groundObj.transform))
                    {
                        isOverGround = false;
                        if (tablePlane.Raycast(ray, out distance))
                        {
                            // Gradually move above the table target
                            hitPoint = ray.GetPoint(distance);
                            targetTx = hit.point.x;
                            targetTy = startingY + 1.5f;
                            targetTz = startingZ - .75f;
                            stepTx = transform.position.x + dragFollowingRate * (targetTx - transform.position.x);
                            stepTy = transform.position.y + dragFollowingRate * (targetTy - transform.position.y);
                            stepTz = transform.position.z + dragFollowingRate * (targetTz - transform.position.z);
                            transform.position = new Vector3(stepTx, stepTy, stepTz);
                            
                            targetBeamTy = startingBeamY;
                            stepBeamTy = cardBeam.transform.position.y + dragFollowingRate * (targetBeamTy - cardBeam.transform.position.y);
                            cardBeam.transform.position = new Vector3(stepTx, stepBeamTy, stepTz);

                            // Gradually rotate back to starting
                            targetR = Quaternion.Euler(50, -180, 0);
                            stepR = Quaternion.Lerp(transform.rotation, targetR, fallingRateRotating);
                            transform.rotation = stepR;
                        }
                    }
                    else
                    {
                        isOverGround = true;
                        if (groundPlane.Raycast(ray, out distance))
                        {
                            // Gradually move above the ground target
                            hitPoint = ray.GetPoint(distance);
                            targetTx = hitPoint.x + offset.x;
                            targetTy = startingY;
                            targetTz = hitPoint.z + offset.z;
                            stepTx = transform.position.x + dragFollowingRate * (targetTx - transform.position.x);
                            stepTy = transform.position.y + dragFollowingRate * (targetTy - transform.position.y);
                            stepTz = transform.position.z + dragFollowingRate * (targetTz - transform.position.z);
                            transform.position = new Vector3(stepTx, stepTy, stepTz);
                            
                            targetBeamTy = startingBeamY;
                            stepBeamTy = cardBeam.transform.position.y + dragFollowingRate * (targetBeamTy - cardBeam.transform.position.y);
                            cardBeam.transform.position = new Vector3(stepTx, stepBeamTy, stepTz);

                            // Gradually rotate back to starting
                            targetR = Quaternion.Euler(50, -180, 0);
                            stepR = Quaternion.Lerp(transform.rotation, targetR, fallingRateRotating);
                            transform.rotation = stepR;
                        }
                    }
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
                    hasLanded = false;
                }
                isDragging = false;
            }

            if (isFallingTx || isFallingTy || isFallingTz || isFallingR)
            {
                if (!isOverGround)
                {
                    // Gradually move down to the table
                    targetTx = hit.point.x;
                    targetTy = startingY;
                    targetTz = startingZ;
                    stepTx = transform.position.x + fallingRateSliding * (targetTx - transform.position.x);
                    stepTy = transform.position.y + fallingRateDown * (targetTy - transform.position.y);
                    stepTz = transform.position.z + fallingRateSliding * (targetTz - transform.position.z);
                    if (stepTx <= fallingRateSliding)
                    {
                        stepTx = targetTx;
                        isFallingTx = false;
                    }
                    if (stepTy <= fallingRateDown + startingY + .01f)
                    {
                        stepTy = targetTy;
                        isFallingTy = false;
                    }
                    if (stepTz <= fallingRateSliding)
                    {
                        stepTz = targetTz;
                        isFallingTz = false;
                    }
                    transform.position = new Vector3(stepTx, stepTy, stepTz);
                    cardBeam.transform.position = new Vector3(stepTx, startingBeamY, stepTz);

                    // Gradually rotate to starting
                    targetR = Quaternion.Euler(50, -180, 0);
                    float angle = Quaternion.Angle(transform.rotation, targetR);
                    if (angle <= fallingRateRotating)
                    {
                        stepR = targetR;
                        isFallingR = false;
                    }
                    else
                    {
                        stepR = Quaternion.Lerp(transform.rotation, targetR, fallingRateRotating);
                    }
                    transform.rotation = stepR;

                    if (!hasLanded && stepTy <= fallingRateDown + startingY + 1f)
                    {
                        // Set the flag to indicate that we're playing audio
                        isPlayingAudio = true;
                        StartCoroutine(PlayAudioWithDelayCoroutine(cardDropSound, 0f));
                        hasLanded = true;
                        isOverGround = false;
                    }
                }
                else
                {
                    // Gradually move down to the ground
                    targetTx = hitPoint.x + offset.x;
                    targetTy = groundObj.transform.position.y + fallingRateDown;
                    targetTz = hitPoint.z + offset.z;
                    stepTx = transform.position.x + fallingRateSliding * (targetTx - transform.position.x);
                    stepTy = transform.position.y + fallingRateDown * (targetTy - transform.position.y);
                    stepTz = transform.position.z + fallingRateSliding * (targetTz - transform.position.z);
                    if (stepTx <= fallingRateSliding)
                    {
                        stepTx = targetTx;
                        isFallingTx = false;
                    }
                    if (stepTy <= fallingRateDown + groundObj.transform.position.y + .01f)
                    {
                        stepTy = targetTy;
                        isFallingTy = false;
                    }
                    if (stepTz <= fallingRateSliding)
                    {
                        stepTz = targetTz;
                        isFallingTz = false;
                    }
                    transform.position = new Vector3(stepTx, stepTy, stepTz);
                    cardBeam.transform.position = new Vector3(stepTx, stepTy - 5, stepTz);

                    // Gradually rotate flat
                    targetR = Quaternion.Euler(0, -180, 0);
                    float angle = Quaternion.Angle(transform.rotation, targetR);
                    if (angle <= fallingRateRotating)
                    {
                        stepR = targetR;
                        isFallingR = false;
                    }
                    else
                    {
                        stepR = Quaternion.Lerp(transform.rotation, targetR, fallingRateRotating);
                    }
                    transform.rotation = stepR;

                    if (!hasLanded && stepTy <= fallingRateDown + groundObj.transform.position.y + 1f)
                    {
                        // Set the flag to indicate that we're playing audio
                        isPlayingAudio = true;
                        StartCoroutine(PlayAudioWithDelayCoroutine(cardDropSound, 0f));
                        hasLanded = true;
                    }

                    if (hasLanded)
                    {
                        //Destroy(gameObject);
                    }
                }
            }
        }
    }
}
