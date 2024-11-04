using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CALORIES : MonoBehaviour
{
    public BLE BLE; // Reference to the BLE class
    public TMP_Text messageDisplays; // Text display for calories
    public float time;
    void Update()
    {
        int Weight = 72; // Weight in kg
        int Age = 17; // Age in years
        time += Time.deltaTime;

        // Ensure BLE.HeartRate is now a float
        float heartRate = BLE.HeartRate;

        // Calculate calories burned
        float cal = ((-55.0969f + (0.6309f * heartRate) + (0.1988f * Weight) + (0.2017f * Age)) / 4.184f) * (time/3600);

        // Display the calories calculated
        messageDisplays.text = $"{cal:F2}"; // Display with two decimal places
    }
}
