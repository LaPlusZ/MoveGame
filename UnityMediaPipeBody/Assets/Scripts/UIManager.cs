using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public RectTransform TransitionObject;
    public int gameIndex;
    public bool paused;
    public bool db;

    public bool disableOpeningTransition;

    private Volume volume; //Post Processing Volume
    private ColorAdjustments colorAdjustments;
    private Transform camera;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments obj)) 
        {
            colorAdjustments = obj;
        }
        if (disableOpeningTransition == false)
        {
            transition_open();
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
                FindObjectOfType<WallSpawner>(true).Stop();
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
                FindObjectOfType<WallSpawner>(true).Continue();
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

    public async void loadScene(string sceneName)
    {
        PipeServer ps = FindObjectOfType<PipeServer>();

        await transition_close();
        if (ps)
        {
            ps.Cleanup();
        }
        
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public async Task transition_close()
    {
        TransitionObject.rotation = Quaternion.Euler(Vector3.zero);
        TransitionObject.sizeDelta = new Vector2(3000, 3000);
        TransitionObject.DORotate(new Vector3(0,0,360), 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).SetUpdate(true);
        await TransitionObject.DOSizeDelta(Vector2.zero, 1.5f).SetEase(Ease.OutQuart).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async Task transition_open()
    {
        TransitionObject.rotation = Quaternion.Euler(Vector3.zero);
        TransitionObject.sizeDelta = Vector3.zero;
        TransitionObject.DORotate(new Vector3(0,0,360), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).SetUpdate(true);
        await TransitionObject.DOSizeDelta(new Vector2(3000, 3000), 1.5f).SetEase(Ease.InQuart).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void retry()
    {
        loadScene(SceneManager.GetActiveScene().name);
    }
}
