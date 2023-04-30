using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public float speed = 10.0f;  // The movement speed of the player

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the horizontal and vertical input from the arrow keys or WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input and the player's orientation
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
        direction = transform.TransformDirection(direction);

        // Move the player in the calculated direction
        transform.position += direction * speed * Time.deltaTime;
    }
}
