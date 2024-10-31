using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);

        cellIndicator.SetActive(true);
        RotatePreview(0); // Reset rotation angle to 0 for new object
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
            cellIndicator.transform.rotation = Quaternion.identity; // Reset rotation
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        cellIndicator.transform.localScale = Vector3.one; // Reset scale
        cellIndicator.transform.rotation = Quaternion.identity; // Reset rotation
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    public void UpdatePosition(Vector3 position, bool validity, float angle)
    {
        if (previewObject != null)
        {
            MovePreview(position, angle);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position, angle);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position, float angle)
    {
        cellIndicator.transform.position = CalculatePositionWithPivot(position, angle, previewYOffset);
    }

    private void MovePreview(Vector3 position, float angle)
    {
        previewObject.transform.position = CalculatePositionWithPivot(position, angle, previewYOffset);
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

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }

    public void RotatePreview(float angle)
    {
        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0, angle, 0);
            cellIndicator.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
