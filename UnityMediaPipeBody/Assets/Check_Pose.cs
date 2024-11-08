using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static PipeServer;

public class PoseAngleCalculator : MonoBehaviour
{
    public PipeServer pipeServer;  // Reference to the PipeServer instance
    private PipeServer.Body body;
    public bool State= false;
    public int poseStat = 0;
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
        poseStat = FindObjectOfType<GameController>().currentPose;
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

            // Check for specific angles
            if (poseStat == 0)
            {
                float leftHandAngle = CalculateAngle(leftShoulder, leftelbow, leftWrist);
                float rightHandAngle = CalculateAngle(rightShoulder, rightelbow, rightWrist);
                float leftLegAngle = CalculateAngle(leftHip, leftKnee, leftAnkle);
                if ((rightHandAngle >= 60 && rightHandAngle <= 110) && (leftHandAngle >= 60 && leftHandAngle <= 110))
                {
                    if (leftLegAngle <= 75 && leftLegAngle >= 40)
                    {
                        State = true;
                        //Debug.Log(State);
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
            else if (poseStat == 1)
            {
                float rightLegAngle = CalculateAngle(rightHip, rightKnee, rightAnkle);
                float rightHandAngle2 = CalculateAngle(rightAnkle, rightShoulder, rightHip);
                float leftHandAngle2 = CalculateAngle(leftAnkle, leftShoulder, leftHip);
                //Debug.Log("LEG" + rightLegAngle + " " + "Hand" + leftHandAngle2);
                if ((rightHandAngle2 <= 30) && (leftHandAngle2 <= 30))
                {
                    if (rightLegAngle >= 75 && rightLegAngle <= 120)
                    {
                        State = true;
                        //Debug.Log(State);
                    }
                    else
                    {
                        State = false;
                    }

                }

            }
            else if (poseStat == 2)
            {
                float rightHandAngle3 = CalculateAngle(rightAnkle, rightShoulder, rightHip);
                float leftHandAngle3 = CalculateAngle(leftAnkle, leftShoulder, leftHip);
                float leftLegAngle3 = CalculateAngle(leftHip, leftKnee, leftAnkle);
                //Debug.Log("LEG" + leftLegAngle3 + " " + "Hand" + leftHandAngle3);
                if ((rightHandAngle3 <= 20) && (rightHandAngle3 <= 20))
                {
                    if (leftLegAngle3 >= 40 && leftLegAngle3 <= 75)
                    {
                        State = true;
                        //Debug.Log(State);
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
        }

    // Method to calculate the angle between three points (similar to Python's calculate_angle function)
    private float CalculateAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 directionAB = pointA - pointB;
        Vector3 directionCB = pointC - pointB;
        return Vector3.Angle(directionAB, directionCB);
    }
}
