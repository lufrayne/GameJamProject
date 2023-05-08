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
    private GameObject chaser1;
    private GameObject chaser2;
    private GameObject chaser3;
    private GameObject chaser4;
    private GameObject chaser5;
    private GameObject chaser6;
    private GameObject chaser7;
    private GameObject chaser8;
    private GameObject chaser9;
    private List<GameObject> chaserList;

    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.Find("Runner");
        chaser1 = GameObject.Find("Chaser 1");
        chaser2 = GameObject.Find("Chaser 2");
        chaser3 = GameObject.Find("Chaser 3");
        chaser4 = GameObject.Find("Chaser 4");
        chaser5 = GameObject.Find("Chaser 5");
        chaser6 = GameObject.Find("Chaser 6");
        chaser7 = GameObject.Find("Chaser 7");
        chaser8 = GameObject.Find("Chaser 8");
        chaser9 = GameObject.Find("Chaser 9");
        chaserList = new List<GameObject> { chaser1, chaser2, chaser3, chaser4, chaser5, chaser6, chaser7, chaser8, chaser9 };
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
        foreach (GameObject chaser in chaserList)
        {
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
