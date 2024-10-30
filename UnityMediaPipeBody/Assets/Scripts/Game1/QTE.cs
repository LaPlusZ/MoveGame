using System.Collections;
using System.Collections.Generic; // Add this line
using UnityEngine;
using TMPro;
using System;

public class QTE : MonoBehaviour
{
    public GameObject qtePanel;
    public TextMeshProUGUI qteInstructionText;
    public float qteDuration = 5.0f;
    public PipeServer pipeServer;
    private bool qteActive = false;
    private bool qteSuccess = false;

    private Action<bool> onQTEComplete;  // Callback for QTE result

    private enum QTEPose { Squat, RaiseBothHands, RaiseLeftHands, RaiseRightHands, CrouchSwing }
    private QTEPose requiredPose;

    void Start()
    {
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

    public void StartQTE(GameObject cat, Action<bool> callback)
    {
        if (qtePanel != null && qteInstructionText != null && pipeServer != null)
        {
            qtePanel.SetActive(true);
            qteActive = true;
            qteSuccess = false;
            onQTEComplete = callback;  // Store callback

            requiredPose = GetRandomPose();
            qteInstructionText.text = "Perform: " + requiredPose.ToString();

            StartCoroutine(QTECountdown());
        }
    }

    IEnumerator QTECountdown()
    {
        yield return new WaitForSeconds(qteDuration);

        if (!qteSuccess)
        {
            EndQTE(false);
        }
    }

    void EndQTE(bool success)
    {
        qteActive = false;
        if (qtePanel != null)
        {
            qtePanel.SetActive(false);
        }

        qteSuccess = success;

        // Capture the cat if successful
        if (success)
        {
            CaptureCat(); // Capture logic can be called here
        }

        // Invoke callback to notify result
        onQTEComplete?.Invoke(success);
        onQTEComplete = null;  // Clear callback
    }


    void CheckForPose()
    {
        if (pipeServer != null)
        {
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

    void CaptureCat()
    {
        Cat catComponent = GetComponent<Cat>();
        if (catComponent != null && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.CaptureCat(catComponent);
        }
    }


    QTEPose GetRandomPose()
    {
        QTEPose[] poses = { QTEPose.Squat, QTEPose.RaiseBothHands, QTEPose.CrouchSwing };
        return poses[UnityEngine.Random.Range(0, poses.Length)];
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
