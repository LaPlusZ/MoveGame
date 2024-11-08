using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class InventoryItem
{
    // Static variable for a shared label or other common data
    public static string Label = "Item Label"; 

    // Instance variables for each inventory item
    public string itemName;
    public int itemCount;
    public Sprite itemImage;
    public int ID;
    public Vector2Int Size = Vector2Int.one;

    // Constructor to easily create new items
    public InventoryItem(string name, int count, Sprite image, int itemID = 100)
    {
        itemName = name;
        itemCount = count;
        itemImage = image;
        ID = itemID;
    }
}