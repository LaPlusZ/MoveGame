using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KeepCenter : MonoBehaviour
{
    public Vector3 multiplier = new Vector3(1,1,1);
   public GameObject obj;
   public GameObject childrenParent;
   public bool stickToFloor;
   public float floorHeight;
   public Vector3 centerPos;

   private Vector3 objOrgPos;
   private List<float> childX = new List<float>();
   private List<float> childY = new List<float>(); 
   private List<float> childZ = new List<float>(); 

    // Start is called before the first frame update
    void Start()
    {
        objOrgPos = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (childrenParent.transform.childCount == 0) {return;}

        foreach (Transform child in childrenParent.transform)
        {
            if (childX != null) 
            {
                childX.Add(child.position.x);
                childY.Add(child.position.y);
                childZ.Add(child.position.z);
            }
        }

        float Xcenter = (childX.Max() + childX.Min())/2;
        float Ycenter = (stickToFloor == false) ? (childY.Max() + childY.Min())/2 : childY.Min();
        float Zcenter = (childZ.Max() + childZ.Min())/2;

        float deltaX = 0 - Xcenter;
        float deltaY = (stickToFloor == false) ? 0 - Ycenter : 0 - Ycenter + floorHeight;
        float deltaZ = 0 - Zcenter;

        obj.transform.position = Vector3.Scale(new Vector3(deltaX, deltaY, deltaZ) + obj.transform.position, multiplier);

        childX.Clear();
        childY.Clear();
        childZ.Clear();
    }
}
