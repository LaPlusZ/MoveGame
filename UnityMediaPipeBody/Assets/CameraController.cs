using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [Header("LookAt Settings")]
    [SerializeField]
    float positionInterpolation;
    [SerializeField]
    float rotationInterpolation;

    Vector3 focus;
    Vector3 orgPos;
    Quaternion orgRot;

    [Header("Handheld Effect Settings")]
    public float shakeIntensity = 0.1f; // Intensity of the shake
    public float shakeFrequency = 20f; // Frequency of the shake
    private float shakeTimer; // Timer to control shake duration
    private float timeCounter;

    private void Start()
    {
        orgPos = transform.position;
        orgRot = transform.rotation;
    }

    public void Calibrate(Transform idk)
    {
        this.focus = FindObjectOfType<KeepCenter>().centerPos;
        transform.position = Vector3.Lerp(orgPos, focus, positionInterpolation);
        transform.rotation = Quaternion.Lerp(orgRot, Quaternion.LookRotation((focus-transform.position).normalized), rotationInterpolation);
    }

    private void LateUpdate()
    {
        Vector3 t = Vector3.Lerp(orgPos, focus, positionInterpolation);
        Vector3 lookAtPos = (positionInterpolation != 0f) ? Vector3.Lerp(transform.position,  t, Time.deltaTime*2.5f) : orgPos;

        Quaternion r = Quaternion.Lerp(orgRot, Quaternion.LookRotation((focus-transform.position).normalized), rotationInterpolation);
        Quaternion lookAtRot = (rotationInterpolation != 0f) ? Quaternion.Lerp(transform.rotation, r, Time.deltaTime * 1f) : orgRot;

        // Increment the time counter
        timeCounter += 0.0001f * shakeFrequency;

        // Calculate random shake offsets
        float x = Mathf.PerlinNoise(timeCounter, 1)-0.5f * shakeIntensity;
        float y = Mathf.PerlinNoise(2, timeCounter)-0.5f * shakeIntensity;
        float z = Mathf.PerlinNoise(timeCounter, 2)-0.5f * shakeIntensity;

        // Apply the shake offsets to the camera's position
        transform.position = lookAtPos + new Vector3(x, y, z);

        // Optionally apply rotation shake
        float rotX = Mathf.PerlinNoise(timeCounter, 1)-0.5f * shakeIntensity - (shakeIntensity / 2); // Range: [-shakeIntensity/2, shakeIntensity/2]
        float rotY = Mathf.PerlinNoise(2, timeCounter)-0.5f * shakeIntensity - (shakeIntensity / 2);
        float rotZ = Mathf.PerlinNoise(timeCounter, 2)-0.5f * shakeIntensity - (shakeIntensity / 2);

        transform.rotation = lookAtRot * Quaternion.Euler(rotX, rotY, rotZ);
    }
}
