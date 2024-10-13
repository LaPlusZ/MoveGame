using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.HighDefinition;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public int gameIndex;
    public bool paused;
    private bool db;

    private Volume volume; //Post Processing Volume
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments obj)) 
        {
            colorAdjustments = obj;
        }
    }

    public void Lose()
    {
        LoseText.gameObject.SetActive(true);
        Pause();
    }

    public void Pause()
    {
        if (db == true) {return;}
        db = true;

        if (paused == false) 
        {
            if (gameIndex == 2)
            {
                FindObjectOfType<WallSpawner>().Stop();
            }

            float t = 1f; // Starting value

            DOTween.To(() => t, x => t = x, 0f, 1f)
                .SetEase(Ease.InOutSine)
                .OnUpdate(() => 
                {
                    Time.timeScale = t;
                    colorAdjustments.saturation.value = (t - 1) * 100;
                })
                .OnComplete(() => 
                {
                    Debug.Log("Tween completed!");
                    db = false;
                });
        }
        else
        {
            if (gameIndex == 2)
            {
                FindObjectOfType<WallSpawner>().Continue();
            }

            float t = 0f; // Starting value

            DOTween.To(() => t, x => t = x, 1f, 1f)
                .SetEase(Ease.InOutSine)
                .OnUpdate(() => 
                {
                    Time.timeScale = t;
                    colorAdjustments.saturation.value = (t - 1) * 100;
                })
                .OnComplete(() => 
                {
                    Debug.Log("Tween completed!");
                    db = false;
                });
        }
    }
}
