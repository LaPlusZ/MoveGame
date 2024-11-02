using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class durationButtons
{
    public int duration; 
    public Button button; 
}


public class DurationChoose : MonoBehaviour
{
    [SerializeField] GameObject ui;
    public List<durationButtons> durations;
    [SerializeField] GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        ui.SetActive(true);
        foreach (durationButtons btn in durations)
        {
            btn.button.onClick.AddListener(() => chose(btn.duration));
        }
    }

    void chose(int duration)
    {
        gameController.durationSecond = duration;
        gameController.enabled = true;
        ui.SetActive(false);
    }
}
