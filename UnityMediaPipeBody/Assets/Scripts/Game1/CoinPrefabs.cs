using UnityEngine;

public class CoinPrefabs : MonoBehaviour
{
    public int coinValue = 1; // Value of the coin

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the Landmark tag
        if (other.CompareTag("Landmark"))
        {
            // Add coins to the GameManager
            GameManager.instance.AddCoins(coinValue);

            Destroy(gameObject);
        }
    }
}
