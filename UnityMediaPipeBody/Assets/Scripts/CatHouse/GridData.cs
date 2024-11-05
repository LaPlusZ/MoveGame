using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObjects = new();
    private GameObject gridVisualization;

    public Vector2Int gridBoundaryMin;
    public Vector2Int gridBoundaryMax;

    public GridData(GameObject gridVisualization)
    {
        this.gridVisualization = gridVisualization;
        GetGridBoundary();
    }

    private void GetGridBoundary()
    {
        gridBoundaryMin.x = - Mathf.RoundToInt((gridVisualization.transform.localScale.x / 2) * 10);
        gridBoundaryMin.y = - Mathf.RoundToInt((gridVisualization.transform.localScale.z / 2) * 10);
        gridBoundaryMax.x = Mathf.RoundToInt((gridVisualization.transform.localScale.x / 2) * 10) - 1;
        gridBoundaryMax.y = Mathf.RoundToInt((gridVisualization.transform.localScale.z / 2) * 10) - 1;
    }

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex, float angle)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize, angle);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, float angle)
    {
        List<Vector3Int> occupiedPositions = new();

        // Handle 1x1 object without rotation adjustments
        if (objectSize == Vector2Int.one)
        {
            occupiedPositions.Add(gridPosition);
            return occupiedPositions;
        }

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                Vector3Int offset = angle switch
                {
                    90 => new Vector3Int(y, 0, objectSize.x - 2 - x),       // Rotate 90 degrees clockwise
                    180 => new Vector3Int(objectSize.x - 2 - x, 0, - y), // Rotate 180 degrees //wtf math
                    270 => new Vector3Int(- y, 0, x),      // Rotate 270 degrees clockwise //wtf math
                    _ => new Vector3Int(x, 0, y)                            // Default (0 degrees)
                };

                Vector3Int finalPosition = gridPosition + offset;

                // Skip if the position is outside boundaries
                /*if (finalPosition.x < gridBoundaryMin.x || finalPosition.x > gridBoundaryMax.x ||
                    finalPosition.z < gridBoundaryMin.y || finalPosition.z > gridBoundaryMax.y)
                {
                    continue;
                }*/

                occupiedPositions.Add(finalPosition);
            }
        }

        return occupiedPositions;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, float angle)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize, angle);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    public bool ObjectExistsAt(Vector3Int gridPosition)
    {
        return placedObjects.ContainsKey(gridPosition);
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition)) return -1;
        return placedObjects[gridPosition].PlaceObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition)) return;

        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlaceObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placeObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlaceObjectIndex = placeObjectIndex;
    }
}
