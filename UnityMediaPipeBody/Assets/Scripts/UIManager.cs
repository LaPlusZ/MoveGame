using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public RectTransform TransitionObject;
    public int gameIndex;
    public bool paused;
    public bool db;

    private Volume volume; //Post Processing Volume
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments obj)) 
        {
            colorAdjustments = obj;
        }
        transition_open();
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
                .SetUpdate(true)
                .OnUpdate(() => 
                {
                    Time.timeScale = t;
                    colorAdjustments.saturation.value = (t - 1) * 100;
                })
                .OnComplete(() => 
                {
                    Debug.Log("Tween completed!");
                    db = false;
                    paused = true;
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
                .SetUpdate(true)
                .OnUpdate(() => 
                {
                    Time.timeScale = t;
                    colorAdjustments.saturation.value = (t - 1) * 100;
                })
                .OnComplete(() => 
                {
                    Debug.Log("Tween completed!");
                    db = false;
                    paused = false;
                });
        }
    }

    public void loadScene(string sceneName)
    {
        transition_close();
        SceneManager.LoadScene(sceneName);
    }

    public void transition_close()
    {
        TransitionObject.rotation = Quaternion.Euler(Vector3.zero);
        TransitionObject.sizeDelta = new Vector2(3000, 3000);
        TransitionObject.DORotate(new Vector3(0,0,360), 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).SetUpdate(true);
        TransitionObject.DOSizeDelta(Vector2.zero, 1.5f).SetEase(Ease.OutQuart).SetUpdate(true);
    }

    public void transition_open()
    {
        TransitionObject.rotation = Quaternion.Euler(Vector3.zero);
        TransitionObject.sizeDelta = Vector3.zero;
        TransitionObject.DORotate(new Vector3(0,0,360), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).SetUpdate(true);
        TransitionObject.DOSizeDelta(new Vector2(3000, 3000), 1.5f).SetEase(Ease.InQuart).SetUpdate(true);
    }
}
