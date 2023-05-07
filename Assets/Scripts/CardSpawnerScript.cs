using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerScript : MonoBehaviour
{
    public GameObject cardToSpawn;
    public float speed = 5f;

    private GameObject spawnedCard;
    private bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && spawnedCard != null)
        {
            // Check if the left mouse button is pressed on the spawned prefab
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == spawnedCard)
            {
                // Left mouse button pressed on the spawned prefab, end the script
                isMoving = false;
                return;
            }
        }

        if (isMoving)
        {
            // Move the spawned prefab across the screen
            if (spawnedCard != null)
            {
                spawnedCard.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    public void SpawnCard()
    {
        // Spawn the card from the current game object
        spawnedCard = Instantiate(cardToSpawn, transform.position, Quaternion.identity);
    }
}

