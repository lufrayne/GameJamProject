using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserSpawnScript : MonoBehaviour
{
    public GameObject chaserToSpawn;
    public Transform spawnPlane;
    public float spawnRadius = 5f;
    public float spawnInterval = 2f;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    private void SpawnObject()
    {
        Vector3 randomPoint = GetRandomPointOnPlane();
        GameObject spawnedObject = Instantiate(chaserToSpawn, randomPoint, Quaternion.identity);
        // You can customize the spawned object's properties or add additional components here
    }

    private Vector3 GetRandomPointOnPlane()
    {
        Vector3 planeCenter = spawnPlane.position;
        Vector3 planeNormal = spawnPlane.up;
        Vector2 randomPointOnCircle = RandomPointOnUnitCircle();
        Vector3 randomPoint = planeCenter + new Vector3(randomPointOnCircle.x, 0f, randomPointOnCircle.y) * spawnRadius;
        randomPoint -= Vector3.Project(randomPoint - planeCenter, planeNormal); // Project point onto the plane
        return randomPoint;
    }

    private Vector2 RandomPointOnUnitCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
