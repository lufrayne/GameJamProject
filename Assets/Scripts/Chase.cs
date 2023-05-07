using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    // Public fields
    public float chaseRate = .001f;

    // Private variables
    private GameObject runner;
    private float targetTx;
    private float targetTz;
    private float stepTx;
    private float stepTz;

    // Start is called before the first frame update
    void Start()
    {
        runner = GameObject.Find("Runner");
    }

    // Update is called once per frame
    void Update()
    {
        // Chase the runner.
        targetTx = runner.transform.position.x;
        targetTz = runner.transform.position.z;
        stepTx = transform.position.x + chaseRate * (targetTx - transform.position.x);
        stepTz = transform.position.z + chaseRate * (targetTz - transform.position.z);
        transform.position = new Vector3(stepTx, transform.position.y, stepTz);
    }
}
