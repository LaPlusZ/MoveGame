using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    private float t = 1f; // Starting value
    public int gameIndex;

    public void Lose()
    {
        LoseText.gameObject.SetActive(true);
        Pause();
    }

    private void Pause()
    {
        if (gameIndex == 2)
        {
            FindObjectOfType<WallSpawner>().Stop();
        }
        DOTween.To(() => t, x => t = x, 0f, 1f)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => Time.timeScale = t)
            .OnComplete(() => Debug.Log("Tween completed!"));
    }
}
