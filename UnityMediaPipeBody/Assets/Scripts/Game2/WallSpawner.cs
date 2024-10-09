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

    async void Start()
    {
        while (stopped == false) {
            foreach (GameObject wall in walls) {
                Instantiate(wall, transform.position, wall.transform.rotation);

                await Task.Delay(timeSpacing*1000);
            }
        }
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
}
