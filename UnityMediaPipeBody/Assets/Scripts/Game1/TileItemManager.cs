using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatGroup
{
    public GameObject[] Cats; 
    public float Rarity; 
}

public class TileItemManager : MonoBehaviour
{
    [SerializeField] GameObject ObstaclesPrefabs;
    [SerializeField] GameObject CoinPrefabs;
    [SerializeField] List<CatGroup> Cats;

    private void Start()
    {
        SpawnCoin();
        SpawnCat();
    }

    void Update()
    {

    }

    void SpawnObstacles()
    {
        // Random 3 points
        int obstacleSpawnIndex = Random.Range(1, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        // Spawn obstacles
        Instantiate(ObstaclesPrefabs, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        for (int i = 1; i < 5; i++)
        {
            Transform spawnPoint = transform.GetChild(i).transform;
            Instantiate(CoinPrefabs, spawnPoint.position, Quaternion.identity, transform);
        }
    }

    public void SpawnCat()
    {
        // Weighted random algo
        float total_weight = 0;

        for (int i = 0; i < Cats.Count; i++)
        {
            total_weight += Cats[i].Rarity;
        }

        int randNum = Mathf.CeilToInt(Random.value * total_weight); // Fix randNum declaration
        float cursor = 0;

        for (int i = 0; i < Cats.Count; i++)
        {
            cursor += Cats[i].Rarity;
            if (cursor >= randNum)
            {
                // Random cat in current rarity
                int catIndex = Random.Range(0, Cats[i].Cats.Length);
                // Random spawn point
                int catSpawnIndex = Random.Range(3, 5);
                Transform catSpawnPoint = transform.GetChild(catSpawnIndex).transform;
                Instantiate(Cats[i].Cats[catIndex], catSpawnPoint.position, Quaternion.identity, transform);
                return;
            }
        }
    }
}
