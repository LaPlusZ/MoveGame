using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MovingWall : MonoBehaviour
{
    public int movingSpeed;
    public int duration;
    // Start is called before the first frame update
    async void Start()
    {
        int distance = movingSpeed*duration;
        float orgY = transform.position.y;
        transform.position = new Vector3(transform.position.x, -26, transform.position.z);
        transform.DOMoveY(orgY, 2).SetEase(Ease.InOutQuart);
        await transform.DOMoveZ(transform.position.z - distance, duration).SetEase(Ease.Linear).AsyncWaitForCompletion();
        GameObject.Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Landmark"))
        {
            GameObject lose = GameObject.Find("Lose");
            lose.SetActive(true);

            Time.timeScale = 0;
        }
    }
}
