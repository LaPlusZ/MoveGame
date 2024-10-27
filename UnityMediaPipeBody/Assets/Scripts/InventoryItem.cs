using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    // Static variable for a shared label or other common data
    public static string Label = "Item Label"; 

    // Instance variables for each inventory item
    public string itemName;
    public int itemCount;
    public Sprite itemImage;

    // Constructor to easily create new items
    public InventoryItem(string name, int count, Sprite image)
    {
        itemName = name;
        itemCount = count;
        itemImage = image;
    }
}