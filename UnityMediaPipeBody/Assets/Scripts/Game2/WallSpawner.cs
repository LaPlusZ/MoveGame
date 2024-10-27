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

    public void spawnWall()
    {
        GameObject wall = walls[currentWall];
        Instantiate(wall, transform.position, wall.transform.rotation);

        if (currentWall < walls.Count - 1) { currentWall += 1; } else { gameObject.SetActive(false); }
    }

    public void Stop()
    {
        stopped = true;
        MovingWall[] walls = FindObjectsOfType<MovingWall>();
        foreach (MovingWall wall in walls)
        {
            wall.stopped = true;
        }
    }

    public void Continue()
    {
        stopped = false;
        MovingWall[] walls = FindObjectsOfType<MovingWall>();
        foreach (MovingWall wall in walls)
        {
            wall.stopped = false;
            wall.checkTimeOut();
        }
    }
}
