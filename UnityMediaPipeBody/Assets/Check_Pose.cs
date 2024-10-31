using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static PipeServer;

public class PoseAngleCalculator : MonoBehaviour
{
    public PipeServer pipeServer;  // Reference to the PipeServer instance
    private PipeServer.Body body;
    public bool State= false;
    private void Start()
    {
        if (pipeServer == null)
        {
            Debug.LogError("PipeServer reference is missing!");
            return;
        }

        // Get reference to the body from the PipeServer
        body = pipeServer.body;
        if (body == null)
        {
            Debug.LogError("No Body data found in PipeServer!");
        }
    }

    private void Update()
    {
        
        // Ensure the body is active before calculations

        if (body != null && body.active)
        {
            // Retrieve landmark positions
            Vector3 leftHip = body.Position(Landmark.LEFT_HIP);
            Vector3 leftKnee = body.Position(Landmark.LEFT_KNEE);
            Vector3 leftAnkle = body.Position(Landmark.LEFT_ANKLE);
            Vector3 rightHip = body.Position(Landmark.RIGHT_HIP);
            Vector3 rightKnee = body.Position(Landmark.RIGHT_KNEE);
            Vector3 rightAnkle = body.Position(Landmark.RIGHT_ANKLE);

            Vector3 leftShoulder = body.Position(Landmark.LEFT_SHOULDER);
            Vector3 leftWrist = body.Position(Landmark.LEFT_WRIST);
            Vector3 rightShoulder = body.Position(Landmark.RIGHT_SHOULDER);
            Vector3 rightWrist = body.Position(Landmark.RIGHT_WRIST);
            Vector3 leftelbow = body.Position(Landmark.LEFT_ELBOW);
            Vector3 rightelbow = body.Position(Landmark.RIGHT_ELBOW);

            // Calculate the angles
            float leftHandAngle = CalculateAngle(leftShoulder, leftelbow, leftWrist);
            float rightHandAngle = CalculateAngle(rightShoulder,rightelbow , rightWrist);
            float leftLegAngle = CalculateAngle(leftHip, leftKnee, leftAnkle);
            //Debug.Log(leftHandAngle);
            // Check for specific angles

            if ((rightHandAngle >= 60 && rightHandAngle <= 100) && (leftHandAngle >= 60 && leftHandAngle <= 100))
            {
                if (leftLegAngle <= 100 && leftLegAngle >= 50)
                {
                    State = true;
                    Debug.Log(State);
                }
                else
                {
                    State = false;
                }

            }
            else
            {
                State = false;
            }

        }
    }

    // Method to calculate the angle between three points (similar to Python's calculate_angle function)
    private float CalculateAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 directionAB = pointA - pointB;
        Vector3 directionCB = pointC - pointB;
        return Vector3.Angle(directionAB, directionCB);
    }
}
