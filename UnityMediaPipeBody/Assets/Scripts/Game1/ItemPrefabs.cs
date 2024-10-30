using System.Collections;
using UnityEngine;

public class ItemPrefabs : MonoBehaviour
{
    public float boostDuration = 20f; // Duration of the coin boost in seconds
    public float coinMultiplier = 2f; // Coin boost multiplier

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the Landmark tag (indicating player contact)
        if (other.CompareTag("Landmark"))
        {
            // Apply the coin boost
            StartCoroutine(ActivateCoinBoost());

            // Destroy the boost item after being collected
            Destroy(gameObject);
        }
    }

    private IEnumerator ActivateCoinBoost()
    {
        // Boost the coin multiplier in the GameManager
        GameManager.instance.SetCoinMultiplier(coinMultiplier);

        // Wait for the boost duration (20 seconds by default)
        yield return new WaitForSeconds(boostDuration);

        // Reset the coin multiplier back to normal (1x)
        GameManager.instance.SetCoinMultiplier(1f);
    }
}
