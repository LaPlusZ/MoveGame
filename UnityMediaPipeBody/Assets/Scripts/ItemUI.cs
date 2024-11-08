using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public InventoryItem itemID;

    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Image displayImage;

    // Optional: Initialize the item details when setting up the item
    public void SetItemDetails(string itemName, int itemID, Sprite itemImage = null)
    {
        //this.itemID = itemID;

        label.text = itemName;
        countText.text = "x1";
        if (itemImage != null)
        {
            displayImage.sprite = itemImage;
        }
        else
        {
            label.rectTransform.anchoredPosition = Vector2.zero;
            label.rectTransform.sizeDelta += new Vector2(0, 15);
            Destroy(displayImage.gameObject);
        }
    }
}
