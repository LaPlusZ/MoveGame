using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<Cat> capturedCats = new List<Cat>();
    [SerializeField] private GameObject testCat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //tests
        Cat catComponent = testCat.GetComponent<Cat>();
        CaptureCat(catComponent);
    }

    public void CaptureCat(Cat cat)
    {
        if (!capturedCats.Contains(cat))
        {
            capturedCats.Add(cat);
            SaveInventory(); // Save each time a new cat is captured
        }
    }

    public void SaveInventory()
    {
        SaveData data = new SaveData();
        data.capturedCatIDs = capturedCats.Select(cat => cat.ID).ToList();  // Store cat IDs
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            capturedCats.Clear();
            foreach (int id in data.capturedCatIDs)
            {
                Cat cat = FindCatByID(id);  // Use the method to find cats by ID
                if (cat != null)
                    capturedCats.Add(cat);
            }
        }
    }

    public Cat FindCatByID(int id)
    {
        // Assuming you have a way to access all available cats.
        Cat[] allCats = FindObjectsOfType<Cat>(); // Replace with your own logic if needed
        foreach (var cat in allCats)
        {
            if (cat.ID == id)
            {
                return cat; // Return the cat with the matching ID
            }
        }
        return null; // Return null if no cat found
    }
}

[System.Serializable]
public class SaveData
{
    public List<int> capturedCatIDs;  // Store each cat by an ID
}
