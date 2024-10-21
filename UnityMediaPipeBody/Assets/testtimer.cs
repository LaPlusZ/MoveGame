using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public PoseAngleCalculator poseAngleCalculator;  // Reference to PoseAngleCalculator

    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] float time;

    void Start()
    {
        
    }

    void Update()
    { // Check the current value of the State variable from PoseAngleCalculator
        if (poseAngleCalculator != null)
        {
            bool currentState = poseAngleCalculator.State;
            if (currentState)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                }
                else if (time < 0)
                {
                    time = 0;
                }
                int seconds = Mathf.FloorToInt(time % 60);
                Timer.text = string.Format("{0:00 }", seconds);
            }
        }
    }

}