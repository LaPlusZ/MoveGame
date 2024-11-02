using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    public int range = 40;
    public int Yrange = 60;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(Random.Range(-range, range), Random.Range(-Yrange, Yrange), Random.Range(-range, range));
    }
}
