using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public GameObject objectPrefab; // Prefab to place in the scene

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element with the mouse
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // When released, place the object in the scene
        PlaceObjectInScene();
    }

    private void PlaceObjectInScene()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // Set this to your desired z position

        if (objectPrefab != null)
        {
            Instantiate(objectPrefab, worldPosition, Quaternion.identity);
        }
    }
}
