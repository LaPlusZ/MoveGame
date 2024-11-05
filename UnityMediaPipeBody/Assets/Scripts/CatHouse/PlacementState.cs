using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private float rotationAngle = 0f;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabaseSo database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabaseSo database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            rotationAngle = 0f;
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else throw new System.Exception($"No Object with ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        Vector2Int rotatedSize = GetRotatedSize(database.objectsData[selectedObjectIndex].Size, rotationAngle);
        
        // Check if placement is valid based on rotated size
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, rotatedSize, rotationAngle);
        if (!placementValidity) return;

        // Place the object with the correct rotation
        int index = objectPlacer.PlaceObject(
            database.objectsData[selectedObjectIndex].Prefab, 
            grid.CellToWorld(gridPosition), 
            Quaternion.Euler(0, rotationAngle, 0)
        );

        // Update GridData with the rotated size
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index, rotationAngle);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false, rotationAngle);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex, Vector2Int rotatedSize, float angle)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        // Iterate over each cell in the rotated size
        for (int x = 0; x < rotatedSize.x; x++)
        {
            for (int z = 0; z < rotatedSize.y; z++)
            {
                Vector3Int checkPosition = gridPosition + new Vector3Int(x, 0, z);

                // Check if checkPosition is within the grid boundaries
                /*if (checkPosition.x < floorData.gridBoundaryMin.x || checkPosition.x > floorData.gridBoundaryMax.x ||
                    checkPosition.z < floorData.gridBoundaryMin.y || checkPosition.z > floorData.gridBoundaryMax.y)
                {
                    return false;
                }*/

                // Check if the cell at checkPosition is occupied
                if (!selectedData.CanPlaceObjectAt(checkPosition, rotatedSize, rotationAngle))
                {
                    return false;
                }
            }
        }

        // If all checks pass, placement is valid
        return true;
    }


    private Vector2Int GetRotatedSize(Vector2Int originalSize, float angle)
    {
        angle = Mathf.Abs(angle) % 360;
        return (angle == 90 || angle == 270) ? new Vector2Int(originalSize.y, originalSize.x) : originalSize;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        Vector2Int rotatedSize = GetRotatedSize(database.objectsData[selectedObjectIndex].Size, rotationAngle);
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, rotatedSize, rotationAngle);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity, rotationAngle);
    }

    public void SetRotation(float angle)
    {
        rotationAngle = angle;
        previewSystem.RotatePreview(rotationAngle);
    }
}
