using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPrefabs : MonoBehaviour
{
    public QTE qteSystem;

    private void Start()
    {
        qteSystem = GameObject.FindObjectOfType<QTE>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Landmark"))
        {
            if (qteSystem != null)
            {
                // Start QTE and pass a callback function to handle the result
                qteSystem.StartQTE(gameObject, OnQTEResult);
                Debug.Log("QTE SYSTEM START");
            }
            else
            {
                Debug.LogError("QTE system not found in the scene!");
            }
        }
    }

    // Callback function to handle QTE result
    private void OnQTEResult(bool success)
    {
        if (success)
        {
            // Add to inventory on success
            InventoryManager.Instance.CaptureCat(GetComponent<Cat>());
        }

        // Destroy the cat object regardless of success
        Destroy(gameObject);
    }
}
