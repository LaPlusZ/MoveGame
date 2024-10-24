using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetect : MonoBehaviour
{
    public PipeServer pipeServer; // Reference to the PipeServer to get landmarks
    public float runThreshold = 2.0f; // Speed threshold to determine running
    public float walkThreshold = 0.5f; // Speed threshold to determine walking
    public float distanceThreshold = 0.5f; // To detect foot movement
    public float timeBetweenChecks = 0.2f; // Time interval for checking the pose

    private float lastCheckTime;
    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;
    private Vector3 lastHipPos;

    private bool isRunning = false;
    private bool isWalking = false;

    void Start()
    {
        // Initialize the foot and hip positions from the PipeServer landmarks
        lastLeftFootPos = pipeServer.body.Position(Landmark.LEFT_FOOT_INDEX);
        lastRightFootPos = pipeServer.body.Position(Landmark.RIGHT_FOOT_INDEX);
        lastHipPos = pipeServer.body.Position(Landmark.LEFT_HIP); // Use the left hip as a general hip position
    }

    void Update()
    {
        // Check pose at intervals
        if (Time.time - lastCheckTime >= timeBetweenChecks)
        {
            DetectPose();
            lastCheckTime = Time.time;
        }
    }

    void DetectPose()
    {
        // Get current positions of feet and hip
        Vector3 currentLeftFootPos = pipeServer.body.Position(Landmark.LEFT_FOOT_INDEX);
        Vector3 currentRightFootPos = pipeServer.body.Position(Landmark.RIGHT_FOOT_INDEX);
        Vector3 currentHipPos = pipeServer.body.Position(Landmark.LEFT_HIP);

        // Calculate distances moved by feet and hip
        float leftFootDistance = Vector3.Distance(currentLeftFootPos, lastLeftFootPos);
        float rightFootDistance = Vector3.Distance(currentRightFootPos, lastRightFootPos);
        float hipSpeed = Vector3.Distance(currentHipPos, lastHipPos) / timeBetweenChecks;

        // Detect running
        if (leftFootDistance > distanceThreshold && rightFootDistance > distanceThreshold && hipSpeed > runThreshold)
        {
            isRunning = true;
            isWalking = false;
        }
        // Detect walking
        else if (leftFootDistance > distanceThreshold && rightFootDistance > distanceThreshold && hipSpeed > walkThreshold)
        {
            isRunning = false;
            isWalking = true;
        }
        else
        {
            isRunning = false;
            isWalking = false;
        }

        // Update the last known positions
        lastLeftFootPos = currentLeftFootPos;
        lastRightFootPos = currentRightFootPos;
        lastHipPos = currentHipPos;

        // Debug logs
        if (isRunning)
        {
            Debug.Log("Running detected");
        }
        else if (isWalking)
        {
            Debug.Log("Walking detected");
        }
        else
        {
            Debug.Log("Idle detected");
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
