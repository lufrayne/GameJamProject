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
    private GameObject groundPlane;
    private GameObject cardBeam;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isFallingTx = false;
    private bool isFallingTy = false;
    private bool isFallingTz = false;
    private bool isFallingR = false;
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
    private bool isPlayingAudio = false;
    private bool hasLanded = true;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = GameObject.Find("Ground Plane");
        ground = new Plane(Vector3.up, groundPlane.transform.position);

        cardBeam = GameObject.Find("CardBeam");
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
                    offset = transform.position - hit.point;
                    isDragging = true;

                    // Set the flag to indicate that we're playing audio
                    isPlayingAudio = true;
                    StartCoroutine(PlayAudioWithDelayCoroutine(cardPickupSound, .1f));
                    Debug.Log("Picked Up");
                }
            }
        }

        if (!isPlayingAudio)
        {
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
                    stepTx = transform.position.x + dragFollowingRate * (targetTx - transform.position.x);
                    stepTy = transform.position.y + dragFollowingRate * (targetTy - transform.position.y);
                    stepTz = transform.position.z + dragFollowingRate * (targetTz - transform.position.z);
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
                    hasLanded = false;
                }
                isDragging = false;
            }

            if (isFallingTx || isFallingTy || isFallingTz || isFallingR)
            {
                // Gradually move down
                targetTx = hitPoint.x + offset.x;
                targetTy = groundPlane.transform.position.y + fallingRateDown;
                targetTz = hitPoint.z + offset.z;
                stepTx = transform.position.x + fallingRateSliding * (targetTx - transform.position.x);
                stepTy = transform.position.y + fallingRateDown * (targetTy - transform.position.y);
                stepTz = transform.position.z + fallingRateSliding * (targetTz - transform.position.z);
                if (stepTx <= fallingRateSliding)
                {
                    stepTx = targetTx;
                    isFallingTx = false;
                }
                if (stepTy <= fallingRateDown + groundPlane.transform.position.y + .01f)
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

                if (!hasLanded && stepTy <= fallingRateDown + groundPlane.transform.position.y + 1f)
                {
                    // Set the flag to indicate that we're playing audio
                    isPlayingAudio = true;
                    StartCoroutine(PlayAudioWithDelayCoroutine(cardDropSound, 0f));
                    hasLanded = true;
                    Debug.Log("Stopped Falling");
                }
            }
        }
    }
}
