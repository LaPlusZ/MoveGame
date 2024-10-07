using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public int timeSpacing = 5;
    public List<GameObject> walls;

    async void Start()
    {
        while (true) {
            foreach (GameObject wall in walls) {
                Instantiate(wall, transform.position, wall.transform.rotation);

                await Task.Delay(timeSpacing*1000);
            }
        }
    }
}
