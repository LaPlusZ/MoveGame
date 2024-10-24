using System.Collections;
using UnityEngine;
using TMPro;

public class QTE : MonoBehaviour
{
    public GameObject qtePanel; // UI Panel for QTE
    public TextMeshProUGUI qteInstructionText; // Instruction text for the pose to perform
    public float qteDuration = 5.0f; // Time duration for the QTE event
    public PipeServer pipeServer; // Reference to PipeServer instead of PoseDetect
    private bool qteActive = false;
    private bool qteSuccess = false;

    // Pose requirements (you can expand this with different types of poses)
    private enum QTEPose { Squat, RaiseBothHands, RaiseLeftHands, RaiseRightHands, CrouchSwing }
    private QTEPose requiredPose;

    void Start()
    {
        // Hide QTE panel at the start
        if (qtePanel != null)
        {
            qtePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (qteActive)
        {
            CheckForPose();
        }
    }

    // Start a QTE event
    public void StartQTE()
    {
        if (qtePanel != null && qteInstructionText != null && pipeServer != null)
        {
            qtePanel.SetActive(true);
            qteActive = true;
            qteSuccess = false;

            // Randomly select a pose
            requiredPose = GetRandomPose();

            // Display the instruction to the player
            qteInstructionText.text = "Perform: " + requiredPose.ToString();

            // Start the countdown for QTE
            StartCoroutine(QTECountdown());
        }
    }

    // Coroutine to handle QTE timing
    IEnumerator QTECountdown()
    {
        yield return new WaitForSeconds(qteDuration);

        if (!qteSuccess)
        {
            EndQTE(false); // QTE failed
        }
    }

    // Check if the player performs the correct pose
    void CheckForPose()
    {
        if (pipeServer != null)
        {
            // Example pose detection logic
            switch (requiredPose)
            {
                case QTEPose.Squat:
                    if (IsSquatPose())
                    {
                        qteSuccess = true;
                        EndQTE(true);
                    }
                    break;

                case QTEPose.RaiseBothHands:
                    if (IsRaiseBothHandsPose())
                    {
                        qteSuccess = true;
                        EndQTE(true);
                    }
                    break;

                case QTEPose.RaiseLeftHands:
                    if (IsRaiseLeftHandsPose())
                    {
                        qteSuccess = true;
                        EndQTE(true);
                    }
                    break;

                case QTEPose.RaiseRightHands:
                    if (IsRaiseRightHandsPose())
                    {
                        qteSuccess = true;
                        EndQTE(true);
                    }
                    break;

                case QTEPose.CrouchSwing:
                    if (IsCrouchSwingPose())
                    {
                        qteSuccess = true;
                        EndQTE(true);
                    }
                    break;
            }
        }
    }

    // End the QTE and hide the panel
    void EndQTE(bool success)
    {
        qteActive = false;

        if (qtePanel != null)
        {
            qtePanel.SetActive(false);
        }

        if (success)
        {
            Debug.Log("QTE Success!");
        }
        else
        {
            Debug.Log("QTE Failed!");
        }
    }

    // Randomly choose a pose for the QTE
    QTEPose GetRandomPose()
    {
        QTEPose[] poses = { QTEPose.Squat, QTEPose.RaiseBothHands, QTEPose.CrouchSwing };
        return poses[Random.Range(0, poses.Length)];
    }

    bool IsSquatPose()
    {
        float kneeHeight = (pipeServer.body.Position(Landmark.LEFT_KNEE).y +
                            pipeServer.body.Position(Landmark.RIGHT_KNEE).y) / 2;
        float hipHeight = (pipeServer.body.Position(Landmark.LEFT_HIP).y +
                           pipeServer.body.Position(Landmark.RIGHT_HIP).y) / 2;

        return kneeHeight < hipHeight * 0.7f;
    }

    bool IsRaiseBothHandsPose()
    {
        float leftHandHeight = pipeServer.body.Position(Landmark.LEFT_WRIST).y;
        float rightHandHeight = pipeServer.body.Position(Landmark.RIGHT_WRIST).y;
        float headHeight = pipeServer.body.Position(Landmark.NOSE).y;

        return leftHandHeight > headHeight && rightHandHeight > headHeight;
    }

    bool IsRaiseLeftHandsPose()
    {
        float leftHandHeight = pipeServer.body.Position(Landmark.LEFT_WRIST).y;
        float headHeight = pipeServer.body.Position(Landmark.NOSE).y;

        return leftHandHeight > headHeight;
    }

    bool IsRaiseRightHandsPose()
    {
        float rightHandHeight = pipeServer.body.Position(Landmark.RIGHT_WRIST).y;
        float headHeight = pipeServer.body.Position(Landmark.NOSE).y;

        return rightHandHeight > headHeight;
    }

    // Detect CrouchSwing pose
    bool IsCrouchSwingPose()
    {
        float kneeHeight = (pipeServer.body.Position(Landmark.LEFT_KNEE).y +
                            pipeServer.body.Position(Landmark.RIGHT_KNEE).y) / 2;
        float hipHeight = (pipeServer.body.Position(Landmark.LEFT_HIP).y +
                           pipeServer.body.Position(Landmark.RIGHT_HIP).y) / 2;
        Vector3 leftHandPosition = pipeServer.body.Position(Landmark.LEFT_WRIST);
        Vector3 rightHandPosition = pipeServer.body.Position(Landmark.RIGHT_WRIST);

        bool isCrouching = kneeHeight < hipHeight * 0.7f;
        bool handsSwing = leftHandPosition.z < pipeServer.body.Position(Landmark.LEFT_HIP).z &&
                          rightHandPosition.z < pipeServer.body.Position(Landmark.RIGHT_HIP).z;

        return isCrouching && handsSwing;
    }
}
