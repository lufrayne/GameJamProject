using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerScript : MonoBehaviour
{
    public GameObject cardToSpawn;
    public float speed = 5f;
    public float spawnInterval = 5f;


    private GameObject spawnedCard;
    private bool isMoving = true;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move the spawned prefab across the screen
            if (spawnedCard != null)
            {
                spawnedCard.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }

        // Check if it's time to spawn a new prefab
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCard();
            timer = 0f;
        }
    }

    public void SpawnCard()
    {
        // Spawn the card from the current game object
        spawnedCard = Instantiate(cardToSpawn, transform.position, Quaternion.identity);
    }
}

