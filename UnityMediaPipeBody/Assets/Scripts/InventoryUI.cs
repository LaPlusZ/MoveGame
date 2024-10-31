using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    List<InventoryItem> inventory = new List<InventoryItem>();

    [SerializeField] GameObject itemUIPrefab;
    [SerializeField] Transform container;

    bool isOpen;

    void Start()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // captured cats from InventoryManager
        List<Cat> capturedCats = InventoryManager.Instance.capturedCats;

        // Populate the inventory with captured cats
        foreach (Cat cat in capturedCats)
        {
            // Assuming your InventoryItem can be constructed from a Cat or has a way to create an InventoryItem from a Cat
            InventoryItem item = new InventoryItem(cat.CatName, 1, cat.CatImage); // Modify to fit your data structure
            GameObject itemUI = Instantiate(itemUIPrefab, container);
            itemUI.GetComponent<ItemUI>().SetItemDetails(item);
        }
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
