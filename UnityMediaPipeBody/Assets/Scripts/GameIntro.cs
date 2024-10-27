using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using Unity.Mathematics;

public class GameIntro : MonoBehaviour
{
    public GameObject content;
    public TMP_Text title;
    public Image background;
    private Transform camera;
    public CanvasGroup gameUIParent;

    // Start is called before the first frame update
    async void  Start()
    {
        camera = Camera.main.transform;
        camera.rotation = quaternion.Euler(new Vector3(-90,0,0));
        gameUIParent.alpha = 0;
        gameUIParent.gameObject.SetActive(false);

        await WaitUntilSceneIsLoaded();

        title.GetComponent<RectTransform>().localScale = new Vector3(7,7,7);
        title.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutBounce).SetUpdate(true);
        title.alpha = 0;
        await title.DOFade(1, 1).SetEase(Ease.InOutQuad).SetUpdate(true).AsyncWaitForCompletion();

        await Task.Delay(1000);

        title.GetComponent<RectTransform>().DOAnchorPosY(200, 1).SetEase(Ease.InOutQuart).SetUpdate(true);
        background.DOFade(0, 1).SetEase(Ease.InOutQuart).SetUpdate(true);
        
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

    public async void EnterGame()
    {
        content.GetComponent<CanvasGroup>().DOFade(0, 1f).SetEase(Ease.InOutCubic);
        await camera.DORotate(Vector3.zero, 2f).SetEase(Ease.InOutCubic).AsyncWaitForCompletion();
        content.SetActive(false);
        gameUIParent.gameObject.SetActive(true);
        gameUIParent.DOFade(1, 1);
    }
}
