using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetect : MonoBehaviour
{
    public PipeServer pipeServer; // Reference to the PipeServer to get pose landmarks
    public float timeBetweenChecks = 0.2f; // Time interval for checking speed
    public float smoothingFactor = 0.8f; // Factor to smooth speed calculation
    public float legMovementThreshold = 0.1f; // Minimum movement to be considered running
    private float lastCheckTime;
    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;
    private Vector3 lastLeftKneePos;
    private Vector3 lastRightKneePos;

    private float runningSpeed; // Measured speed based on leg movement

    void Start()
    {
        // Initialize positions from the PipeServer landmarks
        lastLeftFootPos = pipeServer.body.Position(Landmark.LEFT_FOOT_INDEX);
        lastRightFootPos = pipeServer.body.Position(Landmark.RIGHT_FOOT_INDEX);
        lastLeftKneePos = pipeServer.body.Position(Landmark.LEFT_KNEE);
        lastRightKneePos = pipeServer.body.Position(Landmark.RIGHT_KNEE);
    }

    void Update()
    {
        // Check speed at intervals
        if (Time.time - lastCheckTime >= timeBetweenChecks)
        {
            CalculateRunningSpeedInPlace();
            lastCheckTime = Time.time;
        }
    }

    void CalculateRunningSpeedInPlace()
    {
        // Get current positions of key landmarks
        Vector3 currentLeftFootPos = pipeServer.body.Position(Landmark.LEFT_FOOT_INDEX);
        Vector3 currentRightFootPos = pipeServer.body.Position(Landmark.RIGHT_FOOT_INDEX);
        Vector3 currentLeftKneePos = pipeServer.body.Position(Landmark.LEFT_KNEE);
        Vector3 currentRightKneePos = pipeServer.body.Position(Landmark.RIGHT_KNEE);

        // Calculate vertical movement (up-and-down motion) of knees and feet
        float leftFootVerticalMovement = Mathf.Abs(currentLeftFootPos.y - lastLeftFootPos.y);
        float rightFootVerticalMovement = Mathf.Abs(currentRightFootPos.y - lastRightFootPos.y);
        float leftKneeVerticalMovement = Mathf.Abs(currentLeftKneePos.y - lastLeftKneePos.y);
        float rightKneeVerticalMovement = Mathf.Abs(currentRightKneePos.y - lastRightKneePos.y);

        // Aggregate total movement by averaging the vertical movement of feet and knees
        float totalMovement = (leftFootVerticalMovement + rightFootVerticalMovement +
                               leftKneeVerticalMovement + rightKneeVerticalMovement) / 4;

        // Check if the movement exceeds the threshold (indicating active running)
        float currentSpeed = totalMovement > legMovementThreshold ? totalMovement / timeBetweenChecks : 0f;

        // Smooth the speed value for more stable results
        runningSpeed = Mathf.Lerp(runningSpeed, currentSpeed, smoothingFactor);

        // Update the last known positions
        lastLeftFootPos = currentLeftFootPos;
        lastRightFootPos = currentRightFootPos;
        lastLeftKneePos = currentLeftKneePos;
        lastRightKneePos = currentRightKneePos;

        // Debug log for tracking the calculated speed
        Debug.Log("Running speed (in place): " + runningSpeed + " units per second");
    }

    public float GetRunningSpeed()
    {
        return runningSpeed;
    }
}
