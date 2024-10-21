using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameObject gameManager;
    private RectTransform rt;

    // Start is called before the first frame update
    async void Start()
    {
        timeText.gameObject.SetActive(false);
        rt = timeText.GetComponent<RectTransform>();

        await WaitUntilSceneIsLoaded();
        await CountDown(3, "Start!");

        gameManager.SetActive(true);
    }

    async Task CountDown(int time, string endText)
    {
        timeText.gameObject.SetActive(true);
        for (int i = time; i > 0; i--)
        {
            timeText.alpha = 1;
            rt.localScale = Vector3.one * 3;
            timeText.text = i.ToString();
            rt.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);

            await Task.Delay(800);

            await timeText.DOFade(0, 0.2f).AsyncWaitForCompletion();
        }
        if (endText != "")
        {
            timeText.alpha = 1;
            rt.localScale = Vector3.one * 5;
            timeText.text = endText;
            await rt.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine).AsyncWaitForCompletion();

            await timeText.DOFade(0, 0.2f).AsyncWaitForCompletion();
        }
        timeText.gameObject.SetActive(false);
    }

    async Task WaitUntilSceneIsLoaded()
    {
        // Wait until the scene is fully loaded
        await Task.Yield();  // Make sure the task doesn't block the main thread

        while (!SceneManager.GetActiveScene().isLoaded)
        {
            await Task.Yield();  // Continuously yield control back until the scene is loaded
        }
    }
}
