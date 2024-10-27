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
    public void SetItemDetails(InventoryItem itemID)
    {
        this.itemID = itemID;

        label.text = itemID.itemName;
        countText.text = "x" + itemID.itemCount;
        if (itemID.itemImage != null)
        {
            displayImage.sprite = itemID.itemImage;
        }
        else
        {
            label.rectTransform.anchoredPosition = Vector2.zero;
            label.rectTransform.sizeDelta += new Vector2(0, 15);
            Destroy(displayImage.gameObject);
        }
    }
}
