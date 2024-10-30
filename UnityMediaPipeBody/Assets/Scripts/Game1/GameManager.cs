using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    [Header("Ground Management")]
    public GameObject groundPrefabs;
    public int numberOfGround = 5;
    public float resetPositionZ = -30f;
    public float speedMultiplier = 1.5f;

    private List<GameObject> grounds = new List<GameObject>();
    private float groundWidth;

    [Header("Speed Management")]
    public PoseDetect poseDetect;
    public float idleSpeed = 0f;
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float speedChangeSmoothness = 0.2f;
    private float currentSpeed = 0f;

    [Header("UI Management")]
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    private float elapsedTime = 0f;

    [Header("Cat Spawn Management")]
    private float catSpawnInterval;
    private float nextCatSpawnTime = 0f;

    [Header("Coin & Score")]
    public int coin = 0;
    public int score = 0;
    public float coinMultiplier = 1f;

    // Reference to TileItemManager
    private TileItemManager tileItemManager;
    private int currentTileIndex = 0;

    void Awake()
    {
        // Implement singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    void Start()
    {
        Renderer renderer = groundPrefabs.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            groundWidth = renderer.bounds.size.z;
        }

        for (int i = 0; i < numberOfGround; i++)
        {
            GameObject newGround = SpawnGround(i * groundWidth);
        }

        SetRandomCatSpawnTime(); // Set the first random spawn time
    }

    void Update()
    {
        AdjustGroundSpeed();
        MoveGround();
        TimerDisplay();
        CheckCatSpawn(); // Check if it's time to spawn a cat
    }

    void TimerDisplay()
    {
        elapsedTime += Time.deltaTime;
        TimerText.text = $"{Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
    }

    public void AddCoins(int amount)
    {
        // Apply the coin multiplier when adding coins
        coin += Mathf.RoundToInt(amount * coinMultiplier);
        UpdateCoinUI();
    }

    // Method to set the coin multiplier
    public void SetCoinMultiplier(float multiplier)
    {
        coinMultiplier = multiplier;
    }

    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coin.ToString();
    }

    void MoveGround()
    {
        for (int i = 0; i < grounds.Count; i++)
        {
            // Move the ground based on current speed
            grounds[i].transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);

            // Reset ground position when it reaches the reset point
            if (grounds[i].transform.position.z < resetPositionZ)
            {
                ResetGround(i);
            }
        }
    }

    GameObject SpawnGround(float zPosition)
    {
        GameObject newGround = Instantiate(groundPrefabs, new Vector3(0, 0, zPosition), Quaternion.identity);
        grounds.Add(newGround);
        return newGround;
    }

    void ResetGround(int index)
    {
        float maxZPosition = float.MinValue;
        foreach (GameObject ground in grounds)
        {
            if (ground.transform.position.z > maxZPosition)
            {
                maxZPosition = ground.transform.position.z;
            }
        }

        float newZPosition = maxZPosition + groundWidth;
        grounds[index].transform.position = new Vector3(0, 0, newZPosition);
    }

    // Adjust ground speed based on player's running speed
    void AdjustGroundSpeed()
    {
        float targetSpeed;
        float runningSpeed = poseDetect.GetRunningSpeed();

        // Detect sprint, run, walk, or idle based on the PoseDetect running speed
        if (runningSpeed < 0.2)
        {
            targetSpeed = idleSpeed * speedMultiplier;
        }
        else if (runningSpeed < 1)
        {
            targetSpeed = walkSpeed * speedMultiplier;
        }
        else
        {
            targetSpeed = runSpeed * speedMultiplier;
        }

        // Smoothly interpolate the current speed towards the target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeSmoothness * Time.deltaTime);
    }

    void SetRandomCatSpawnTime()
    {
        catSpawnInterval = Random.Range(5, 10);
        nextCatSpawnTime = elapsedTime + catSpawnInterval * 60f;
    }

    void CheckCatSpawn()
    {
        if (elapsedTime >= nextCatSpawnTime)
        {
            SpawnCatOnFifthTile();
            SetRandomCatSpawnTime();
        }
    }

    void SpawnCatOnFifthTile()
    {
        int targetTileIndex = (currentTileIndex + 5) % grounds.Count;

        TileItemManager targetTileItemManager = grounds[targetTileIndex].GetComponent<TileItemManager>();
        if (targetTileItemManager != null)
        {
            targetTileItemManager.SpawnCat();
        }
        else
        {
            Debug.LogError("TileItemManager not found on tile at index " + targetTileIndex);
        }

        currentTileIndex = (currentTileIndex + 1) % grounds.Count;
    }
}
