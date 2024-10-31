using UnityEngine;

public class CoinPrefabs : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Landmark"))
        {
            // Add coins to the GameManager
            GameManager.instance.AddCoins(coinValue);

            Destroy(gameObject);
        }
    }
}
