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
    private List<GameObject> chaserList;
    private GameObject cylinder1;
    private GameObject cylinder2;
    private GameObject cylinder3;
    private GameObject cylinder4;
    private GameObject cylinder5;
    private GameObject cylinder6;
    private List<GameObject> cylinderList;

    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.Find("Runner");

        chasers = GameObject.FindGameObjectsWithTag("Chaser");  
    
        cylinder1 = GameObject.Find("Cylinder 1");
        cylinder2 = GameObject.Find("Cylinder 2");
        cylinder3 = GameObject.Find("Cylinder 3");
        cylinder4 = GameObject.Find("Cylinder 4");
        cylinder5 = GameObject.Find("Cylinder 5");
        cylinder6 = GameObject.Find("Cylinder 6");
        cylinderList = new List<GameObject> { cylinder1, cylinder2, cylinder3, cylinder4, cylinder5, cylinder6 }; 
    }

    // Update is called once per frame
    void Update()
    {
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

        foreach (GameObject cylinder in cylinderList)
        {
            Vector3 directionToChaser = (cylinder.transform.position - transform.position);
            float distanceToChaser = directionToChaser.magnitude;
            if (distanceToChaser < avoidChaserDistance)
            {
                Vector3 avoidanceDirection = (transform.position - cylinder.transform.position).normalized;
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
