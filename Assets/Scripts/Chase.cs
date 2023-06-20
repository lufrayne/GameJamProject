using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    // Public variables
    public float minSpeed = 5.0f;
    public float maxSpeed = 12.0f;
    public float avoidChaserDistance = 5.0f;
    public float avoidRunnerDistance = 2.0f;
    public float avoidanceForce = 20.0f;

    // Private variables
    private GameObject runner;
    private GameObject[] chasers;
    private GameObject[] pillars;
    private List<GameObject> chaserList;
    private List<GameObject> pillarList;

    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.Find("Runner");
        chasers = GameObject.FindGameObjectsWithTag("Chaser");
    }

    // Update is called once per frame
    void Update()
    {
        pillars = GameObject.FindGameObjectsWithTag("Pillar");

        // Chase the runner, avoid collisions, but don't move up or down
        float startingTy = transform.position.y;

        Vector3 directionToTarget = (runner.transform.position - transform.position);
        float distanceToTarget = directionToTarget.magnitude;
        Vector3 desiredDirection = directionToTarget.normalized;
        Vector3 desiredVelocity = desiredDirection * Random.Range(minSpeed, maxSpeed);

        bool avoid = false;
        foreach (GameObject chaser in chasers)
        {
            if (chaser == null) continue;
            if (name == chaser.name) continue;

            Vector3 directionToChaser = (chaser.transform.position - transform.position);
            float distanceToChaser = directionToChaser.magnitude;
            if (distanceToChaser < avoidChaserDistance)
            {
                Vector3 avoidanceDirection = (transform.position - chaser.transform.position).normalized;
                Vector3 avoidanceVelocity = avoidanceDirection * avoidanceForce;

                transform.position += avoidanceVelocity * Time.deltaTime;
                avoid = true;
            }
        }

        foreach (GameObject pillar in pillars)
        {
            Vector3 directionToChaser = (pillar.transform.position - transform.position);
            float distanceToChaser = directionToChaser.magnitude;
            if (distanceToChaser < avoidChaserDistance)
            {
                Vector3 avoidanceDirection = (transform.position - pillar.transform.position).normalized;
                Vector3 avoidanceVelocity = avoidanceDirection * avoidanceForce;

                transform.position += avoidanceVelocity * Time.deltaTime;
                avoid = true;
            }
        }

        if (distanceToTarget < avoidRunnerDistance)
        {
            Vector3 avoidanceDirection = (transform.position - runner.transform.position).normalized;
            Vector3 avoidanceVelocity = avoidanceDirection * avoidanceForce;

            transform.position += avoidanceVelocity * Time.deltaTime;
            avoid = true;
        }

        if (!avoid)
        {
            transform.position += desiredVelocity * Time.deltaTime;
        }

        transform.position = new Vector3(transform.position.x, startingTy, transform.position.z);
    }

}
