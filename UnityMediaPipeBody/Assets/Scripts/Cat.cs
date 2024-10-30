using UnityEngine;

[System.Serializable]
public class Cat : MonoBehaviour
{
    [SerializeField] private int id; // Unique identifier for each cat
    [SerializeField] private string catRarity; // Rarity of the cat
    [SerializeField] private string catName; // Name of the cat
    [SerializeField] private Sprite catImage; // Image of the cat

    public int ID
    {
        get => id;
        private set => id = value; 
    }

    public string CatRarity
    {
        get => catRarity;
        set => catRarity = value;
    }

    public string CatName
    {
        get => catName;
        set => catName = value;
    }

    public Sprite CatImage
    {
        get => catImage;
        set => catImage = value;
    }

    // debug
    public void DisplayInfo()
    {
        Debug.Log($"Cat Name: {catName}, Rarity: {catRarity}, ID: {id}");
    }
}
