using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [Header("--- Universal ---")]
    public int second;
    public float milisecond;
    public TextMeshProUGUI timeText;
    public GameObject gameManager;
    public CALORIES cal;

    private bool Timer = false;

    [Header("--- Timer ---")]
    public bool overwriteTimer;
    public int durationSecond;
    public bool minSec;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI milisecondText;

    [Header("--- Game 3 (Yoga) ---")]
    public PoseAngleCalculator poseAngleCalculator;
    public List<GameObject> poseHologram;
    public int currentPose = -1;
    private int poseLeft;
    private bool stopCount;

    private RectTransform rt;
    private Stopwatch stopwatch;
    private float idleTimeElapsed;

    // Start is called before the first frame update
    async void Start()
    {
        stopwatch = new Stopwatch();
        timeText.gameObject.SetActive(false);
        rt = timeText.GetComponent<RectTransform>();

        if (gameManager != null && gameManager.GetComponent<GameManager>())
        {
            gameManager.GetComponent<GameManager>().SetUp();
        }

        await WaitUntilSceneIsLoaded();
        await CountDown(3, "Start!");

        cal.enabled = true;

        if (gameManager != null)
        {
            gameManager.SetActive(true);

            if (gameManager.GetComponent<WallSpawner>())
            {
                gameManager.GetComponent<WallSpawner>().spawnWall();
            }
            if (gameManager != null && gameManager.GetComponent<GameManager>())
            {
                second = durationSecond;
                milisecond = 0;
                Timer = true;
                secondText.gameObject.SetActive(true);
                milisecondText.gameObject.SetActive(true);
            }
            
        }
        if (poseAngleCalculator != null)
        {
            Timer = true;
            poseLeft = poseHologram.Count;
            secondText.gameObject.SetActive(true);
            milisecondText.gameObject.SetActive(true);
            nextPose();
        }
    }

    async Task CountDown(int time, string endText)
    {
        timeText.gameObject.SetActive(true);
        for (int i = time; i > 0; i--)
        {
            //if (milisecondText != null) { milisecondText.alpha = 0.50f; StartCoroutine(MillisecondCountdown()); milisecondText.DOFade(0.30f, 1); }
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

            //if (milisecondText != null) { milisecondText.DOFade(0, 0.2f); }
            await timeText.DOFade(0, 0.2f).AsyncWaitForCompletion();
        }
        timeText.gameObject.SetActive(false);
        //if (milisecondText != null) { milisecondText.gameObject.SetActive(false); }
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

    IEnumerator MillisecondCountdown()
    {
        milisecond = 999;
        stopwatch.Restart(); // Start the stopwatch to measure time precisely

        while (stopwatch.ElapsedMilliseconds < 1000) // Run until 1 second (1000ms) has passed
        {
            float elapsedMs = stopwatch.ElapsedMilliseconds; // Get the total elapsed milliseconds

            // Update the milliseconds, subtract the time passed from 1000ms
            milisecond = 999 - elapsedMs;
            if (milisecond < 0)
                milisecond = 0;

            // Update the UI
            milisecondText.text = Mathf.FloorToInt(milisecond / 10f).ToString("D2");

            // Wait until the next frame
            yield return null;
        }

        // Make sure milliseconds hit 0 when the countdown finishes
        milisecond = 0;
        milisecondText.text = Mathf.FloorToInt(milisecond / 10f).ToString("D2");
    }

    void Update()
    {
        if (poseAngleCalculator != null && Timer) //Only game 3 (Yoga)
        {
            bool currentState = poseAngleCalculator.State;
            secondText.text = second.ToString("D2");
            milisecondText.text = Mathf.FloorToInt(milisecond/10).ToString("D2");

            if (currentState && stopCount == false)
            {
                idleTimeElapsed = 0;
                secondText.alpha = 1;
                milisecondText.alpha = 0.4f;

                if (milisecond > 0)
                {
                    milisecond -= Mathf.RoundToInt(Time.deltaTime * 1000);
                    if (milisecond <= 0 && second > 0)
                    {
                        milisecond = 999+milisecond;
                        second -= 1;
                    }
                    else if (milisecond <= 0 && second <= 0)
                    {
                        milisecond = 0;
                        second = 0;

                        nextPose();
                    }
                }
                else if (milisecond <= 0 && second > 0)
                {
                    milisecond = 999+milisecond;
                    second -= 1;
                }
                else
                {
                    milisecond = 0;
                    second = 0;

                    nextPose();
                }
                secondText.text = second.ToString("D2");
                milisecondText.text = Mathf.FloorToInt(milisecond/10).ToString("D2");
            }
            else
            {
                idleTimeElapsed += Time.deltaTime;
                secondText.alpha = 2-idleTimeElapsed;
                milisecondText.alpha = 2-idleTimeElapsed;
            }
        }
        else if (overwriteTimer && Timer && minSec)
        {
            if (milisecond > 0)
            {
                milisecond -= Time.deltaTime;
                if (milisecond <= 0 && second > 0)
                {
                    milisecond = 60+milisecond;
                    second -= 1;
                }
                else if (milisecond <= 0 && second <= 0)
                {
                    milisecond = 0;
                    second = 0;

                    EndGame();
                }
            }
            else if (milisecond <= 0 && second > 0)
            {
                milisecond = 60+milisecond;
                second -= 1;
            }
            else
            {
                milisecond = 0;
                second = 0;

                EndGame();
            }
            secondText.text = second.ToString("D2");
            milisecondText.text = Mathf.FloorToInt(milisecond).ToString("D2");
        }
    }

    async void nextPose()
    {
        milisecond = 0;
        second = durationSecond;
        stopCount = true;

        if (poseLeft == 0) //Game end
        {
            EndGame();
            return;
        }
        else
        {
            poseLeft -= 1;
        }

        int randIndex = Random.Range(0, poseHologram.Count - 1);
        int tryInterval = 0;
        while (randIndex == currentPose && tryInterval < 10)
        {
            randIndex = Random.Range(0, poseHologram.Count - 1);
            tryInterval++;
        }
        if (tryInterval == 10)
        {
            UnityEngine.Debug.Log("Unable to change pose");
            return;
        } 

        await poseHologram[currentPose < 0 ? 0 : currentPose].GetComponent<PoseHologram>().closeAnimation();
        poseHologram[randIndex].SetActive(true);
        poseHologram[randIndex].GetComponent<PoseHologram>().startAnimation();
        poseAngleCalculator.poseStat = randIndex;
        currentPose = randIndex;


        UnityEngine.Debug.Log("Changed to pose " + currentPose);
        stopCount = false;
    }

    public async void EndGame()
    {
        if (poseHologram.Count > 0) 
        {
            UnityEngine.Debug.Log("No pose left. Well done!");
            await poseHologram[currentPose < 0 ? 0 : currentPose].GetComponent<PoseHologram>().closeAnimation();
        }

        string[] words = {
            "Fantastic!", "Amazing!", "Well done!", "Excellent!", "Bravo!", 
            "Incredible!", "Superb!", "Outstanding!", "Awesome!", "Marvelous!",
            "Terrific!", "Brilliant!", "Stellar!", "Impressive!", "Magnificent!",
            "Spectacular!", "Splendid!", "Remarkable!", "Fabulous!", "Sensational!"
        };

        int rand = Random.Range(0, words.Length);
        await CountDown(0, words[rand]);

        await Task.Delay(2000);

        GetComponent<UIManager>().loadScene("Home");
    }
}
