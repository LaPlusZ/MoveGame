using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image displayImage;

    // Set item details for shop display
    public void SetItemDetails(string itemName, int price, Sprite itemImage = null)
    {
        label.text = itemName;
        priceText.text = price + " cc";

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
