using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aoeScript : MonoBehaviour
{
    public float AoERadius = 5f; // Adjust the radius of the area of effect collider here

    private MouseDragScript mouseDragScriptInstance;
    private Collider[] colliders;
    private void Awake()
    {
        colliders = new Collider[10]; // Maximum number of colliders to detect (change as needed)
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseDragScriptInstance = GetComponent<MouseDragScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseDragScriptInstance.hasLanded && mouseDragScriptInstance.isOverGround && !mouseDragScriptInstance.isDragging)
        {
            Debug.Log("Triggering AOE...");
            Vector3 explosionPosition = transform.position;
            int numColliders = Physics.OverlapSphereNonAlloc(explosionPosition, AoERadius, colliders);

            for (int i = 0; i < numColliders; i++)
            {
                // Do something with each collider within the area of effect
                Collider collider = colliders[i];
                // Example: If the collider has a specific tag, apply a force, disable, or destroy it
                if (collider.CompareTag("Chaser"))
                {
                    Rigidbody enemyRigidbody = collider.GetComponent<Rigidbody>();
                    if (enemyRigidbody != null)
                    {
                        enemyRigidbody.AddExplosionForce(10f, explosionPosition, AoERadius, 1f, ForceMode.Impulse);
                    }
                    collider.gameObject.SetActive(false);
                }
            }
        }
    }
}
