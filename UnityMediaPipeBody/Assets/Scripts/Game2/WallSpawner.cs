using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public int timeSpacing = 5;
    public List<GameObject> walls;
    private bool stopped;
    private float timeElapsed;
    private int currentWall;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > timeSpacing)
        {
            timeElapsed -= timeSpacing;

            spawnWall();
        }
    }

    public async void spawnWall()
    {
        GameObject wall = walls[currentWall];
        Instantiate(wall, transform.position, wall.transform.rotation);

        if (currentWall < walls.Count - 1) 
        {
            currentWall += 1;
        } 
        else 
        { 
            gameObject.SetActive(false);

            await DelayWithTimeScale(10000);

            FindObjectOfType<GameController>().EndGame();
        }
    }

    public void Stop()
    {
        MovingWall[] walls = FindObjectsOfType<MovingWall>();
        foreach (MovingWall wall in walls)
        {
            wall.stopped = true;
        }
    }

    public void Continue()
    {
        MovingWall[] walls = FindObjectsOfType<MovingWall>();
        foreach (MovingWall wall in walls)
        {
            wall.stopped = false;
            wall.checkTimeOut();
        }
    }

    private async Task DelayWithTimeScale(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.deltaTime; // Accumulates scaled time
            await Task.Yield();
        }
    }
}
