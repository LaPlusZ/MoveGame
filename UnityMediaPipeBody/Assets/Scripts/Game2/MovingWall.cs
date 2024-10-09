using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class MovingWall : MonoBehaviour
{
    public float tweenTime = 10f;
    public int targetZOffset;
    
    private Rigidbody rb;
    private UIManager uiManager;
    public bool stopped;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    async void Start()
    {
        rb = GetComponent<Rigidbody>();
        float orgYPos = rb.position.y + 32;
        
        uiManager = FindObjectOfType<UIManager>();

        targetPosition = rb.position + new Vector3(0,0,targetZOffset);
        Vector3 step1 = new Vector3(rb.position.x, orgYPos, rb.position.z + ((2f/tweenTime)*targetZOffset));

        await rb.DOMove(step1, 2).SetEase(Ease.Linear).AsyncWaitForCompletion();
        await rb.DOMoveZ(targetPosition.z, tweenTime-2).SetEase(Ease.Linear).AsyncWaitForCompletion();
        GameObject.Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) 
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Landmark") && stopped == false)
        {
            uiManager.Lose();
        }
    }
}
