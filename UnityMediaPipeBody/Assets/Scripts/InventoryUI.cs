using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    List<InventoryItem> inventory = new List<InventoryItem>();

    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private ObjectDatabaseSo objectDatabase;
    [SerializeField] Transform container;


    bool isOpen;

    void Start()
    {
        // Clear existing UI elements in the container
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        /*// Get captured cats from InventoryManager
        List<Cat> capturedCats = InventoryManager.Instance.capturedCats;

        // Populate the inventory UI with captured cats
        foreach (Cat cat in capturedCats)
        {
            // Assuming InventoryItem can be created with the given cat properties
            InventoryItem item = new InventoryItem(cat.CatName, 1, cat.CatImage);
            GameObject itemUI = Instantiate(itemUIPrefab, container);
            itemUI.GetComponent<ItemUI>().SetItemDetails(item);

            int itemId = item.ID; // ***change to cat.ID later
            itemUI.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartPlacement(itemId));
        }*/ // no save data right now

        foreach (ObjectData item in objectDatabase.objectsData)
        {
            if (item.ID >= 100 && item.ID <= 200)
            {
                GameObject itemUI = Instantiate(itemUIPrefab, container);
                ItemUI itemUIScript = itemUI.GetComponent<ItemUI>();
                itemUIScript.SetItemDetails(item.Name, item.Price);


                // Store item ID in a local variable for the callback
                int itemId = item.ID;
                itemUI.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartPlacement(itemId));
            }
        }
    }

    private void StartPlacement(int itemId)
    {
        placementSystem.StartPlacement(itemId);  // Call PlacementSystem's StartPlacement
        CloseMenu();
    }

    public async void CloseMenu()
    {
        if (isOpen == true)
        {
            transform.localScale = new Vector3(1,1,1);
            GetComponent<CanvasGroup>().alpha = 1;
            transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.5f).SetEase(Ease.InQuart).SetUpdate(true);
            await GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            gameObject.SetActive(false);
            isOpen = false;
        }
    }

    public async void OpenMenu()
    {
        if (isOpen == false)
        {
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>().alpha = 0;
            transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            transform.DOScale(new Vector3(1,1,1), 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            await GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            isOpen = true;
        }
    }
}
