using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefabs;
    public GameObject coinPrefabs;
    public GameObject obstaclePrefabs;
    public float moveSpeed = 5f;
    public float resetPositionZ = -30f;
    public float spawnZPosition = 10f;
    public int numberOfGround = 5;

    public float coinSpawnProbability = 0.5f;
    public float obstacleSpawnProbability = 0.5f;

    private List<GameObject> grounds = new List<GameObject>();
    private float groundWidth;

    void Start()
    {
        Renderer renderer = groundPrefabs.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            groundWidth = renderer.bounds.size.z;
        }
        else
        {
            Debug.LogError("No Renderer found on the groundPrefabs or its children.");
        }

        for (int i = 0; i < numberOfGround; i++)
        {
            SpawnGround(i * groundWidth);
        }
    }

    void Update()
    {
        MoveGround();
    }

    void MoveGround()
    {
        for (int i = 0; i < grounds.Count; i++)
        {
            // Smooth movement using Vector3.MoveTowards for each ground piece
            grounds[i].transform.position = Vector3.MoveTowards(
                grounds[i].transform.position,
                grounds[i].transform.position + Vector3.back * moveSpeed * Time.deltaTime,
                moveSpeed * Time.deltaTime
            );

            // Check if the ground has reached the reset position
            if (grounds[i].transform.position.z < resetPositionZ)
            {
                ResetGround(i);
            }
        }
    }

    void SpawnGround(float zPosition)
    {
        GameObject newGround = Instantiate(groundPrefabs, new Vector3(0, 0, zPosition), Quaternion.identity);
        grounds.Add(newGround);

        SpawnCoinsAndObstacles(newGround);
    }

    void SpawnCoinsAndObstacles(GameObject ground)
    {
        if (UnityEngine.Random.value < coinSpawnProbability)
        {
            Vector3 coinPosition = new Vector3(UnityEngine.Random.Range(-10f, 10f), -8f, ground.transform.position.z);
            GameObject coin = Instantiate(coinPrefabs, coinPosition, Quaternion.identity);
            coin.transform.parent = ground.transform;
        }

        if (UnityEngine.Random.value < obstacleSpawnProbability)
        {
            Vector3 obstaclePosition = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-8f, 5f), ground.transform.position.z);
            GameObject obstacle = Instantiate(obstaclePrefabs, obstaclePosition, Quaternion.identity);
            obstacle.transform.parent = ground.transform;
        }
    }

    void ResetGround(int index)
    {
        // Get the Z position of the last ground piece
        float lastGroundZPosition = grounds[grounds.Count - 1].transform.position.z;

        // Set the new Z position based on the last ground's Z position plus groundWidth
        float newZPosition = lastGroundZPosition + groundWidth;

        // Reset the position smoothly to make it appear continuous
        grounds[index].transform.position = new Vector3(0, 0, newZPosition);

        // Optional: Clear and respawn coins/obstacles if needed
        ClearAndRespawn(grounds[index]);
    }

    void ClearAndRespawn(GameObject ground)
    {
        // Clear old coins and obstacles
        foreach (Transform child in ground.transform)
        {
            Destroy(child.gameObject);
        }

        // Respawn coins and obstacles
        SpawnCoinsAndObstacles(ground);
    }
}
