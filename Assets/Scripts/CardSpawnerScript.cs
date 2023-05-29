using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerScript : MonoBehaviour
{
    // Public fields
    public GameObject cardToSpawn;
    public float speed = 5f;
    public float spawnInterval = 5f;

    // Private variables
    private GameObject spawnedCard;
    private MouseDragScript mouseDragScript;
    private float timer = 0f;

    // Update is called once per frame
    private void Update()
    {
        // Move the spawned prefab across the screen
        if (spawnedCard != null)
        {
            if (!mouseDragScript.isDragging && !mouseDragScript.isOverGround)
            {
                spawnedCard.transform.Translate(speed * Time.deltaTime * Vector3.left);
            }
        }

        // Check if it's time to spawn a new prefab
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            if (spawnedCard != null)
            {
                Destroy(spawnedCard);
            }
            SpawnCard();
            timer = 0f;
        }
    }

    private void SpawnCard()
    {
        // Spawn the card from the current game object
        spawnedCard = Instantiate(cardToSpawn);
        mouseDragScript = spawnedCard.GetComponent<MouseDragScript>();
    }
}

