using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new();

    public int PlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //angle from Quaternion "Rotation y axis"
        float angle = rotation.eulerAngles.y;

        //Rotation position from angle base on pivot left bottom
        Vector3 newPosition = CalculatePositionWithPivot(position, angle);

        //Modify position base on pivot
        GameObject newObject = Instantiate(prefab, newPosition, rotation);
        newObject.transform.position = newPosition;

        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex
            || placedGameObject[gameObjectIndex] == null) return;
        Destroy(placedGameObject[gameObjectIndex]);
        placedGameObject[gameObjectIndex] = null;
     
    }
    private Vector3 CalculatePositionWithPivot(Vector3 position, float angle, float yOffset = 0)
    {
        if (angle == 90)
            return new Vector3(position.x, position.y + yOffset, position.z + 1);
        else if (angle == 180)
            return new Vector3(position.x + 1, position.y + yOffset, position.z + 1);
        else if (angle == 270)
            return new Vector3(position.x + 1, position.y + yOffset, position.z);
        else
            return new Vector3(position.x, position.y + yOffset, position.z);
    }
}
