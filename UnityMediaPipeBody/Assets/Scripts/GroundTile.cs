using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{

    [SerializeField] GameObject ObstaclesPrefabs;
    [SerializeField] GameObject CoinPrefabs;

    // Start is called before the first frame update
    private void Start()
    {
        SpawnObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacles()
    {
        //random 3 point
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        //spawn obstacles
        Instantiate(ObstaclesPrefabs, spawnPoint.position, Quaternion.identity, transform);
    }
}
