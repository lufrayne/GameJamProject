using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{

    public GameObject pillarToSpawn;

    private GameObject spawnedPillar;
    private MouseDragScript mouseDragScriptInstance;
    private bool pillarHasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
       mouseDragScriptInstance = GetComponent<MouseDragScript>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnPillar();
    }

    private void spawnPillar()
    {
        if (mouseDragScriptInstance.hasLanded && mouseDragScriptInstance.isOverGround && !mouseDragScriptInstance.isDragging)
        {
            Debug.Log("Spawning pillar...");
            spawnedPillar = Instantiate(pillarToSpawn);
            spawnedPillar.transform.position = transform.position;
            pillarHasSpawned = true;
            Destroy(gameObject);
        }

    }
}
