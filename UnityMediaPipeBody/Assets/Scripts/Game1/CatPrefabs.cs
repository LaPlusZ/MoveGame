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
                qteSystem.StartQTE();
                Debug.Log("QTE SYSTEM START");
            }
            else
            {
                Debug.LogError("QTE system not found in the scene!");
            }

            Destroy(gameObject);
        }
    }
}
