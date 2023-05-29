using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Card collision detection started.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Card collided with something");
        // Check if the collision involves the other object type
        if (other.gameObject.CompareTag("Chaser"))
        {
            Debug.Log("Card collided with chaser");
            Destroy(other.gameObject);
        }
    }
}
