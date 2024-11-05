using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

    [SerializeField] private ObjectDatabaseSo objectDatabase;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private GameObject shopUIPrefab;
    [SerializeField] private Transform container;

    bool isOpen;

    void Start()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (ObjectData item in objectDatabase.objectsData)
        {
            GameObject itemUI = Instantiate(shopUIPrefab, container);
            ItemShopUI itemShopUIScript = itemUI.GetComponent<ItemShopUI>();
            itemShopUIScript.SetItemDetails(item.Name, item.Price, item.Prefab.GetComponent<SpriteRenderer>()?.sprite);

            //each button ui when click will call startplacement form PLacementSystem 
            int itemId = item.ID;  // Store item ID in a local variable for the callback
            itemUI.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartPlacement(itemId));
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
            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<CanvasGroup>().alpha = 1;
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetEase(Ease.InQuart).SetUpdate(true);
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
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            await GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            isOpen = true;
        }
    }
}
