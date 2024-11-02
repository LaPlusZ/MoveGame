using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    public Transform nose;
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform leftElbow;
    public Transform rightElbow;
    public Transform leftWrist;
    public Transform rightWrist;
    public Transform leftHip;
    public Transform rightHip;
    public Transform leftKnee;
    public Transform rightKnee;
    public Transform leftHeel;
    public Transform rightHeel;

    private Transform NOSE;
    private Transform LEFT_SHOULDER;
    private Transform RIGHT_SHOULDER;
    private Transform LEFT_ELBOW;
    private Transform RIGHT_ELBOW;
    private Transform LEFT_WRIST;
    private Transform RIGHT_WRIST;
    private Transform LEFT_HIP;
    private Transform RIGHT_HIP;
    private Transform LEFT_KNEE;
    private Transform RIGHT_KNEE;
    private Transform LEFT_HEEL;
    private Transform RIGHT_HEEL;

    int tryInterval;

    // Start is called before the first frame update
    async void Start()
    {
        while (NOSE != null && tryInterval < 10)
        {
            NOSE = GameObject.Find("NOSE").transform;
            LEFT_SHOULDER = GameObject.Find("LEFT_SHOULDER").transform;
            RIGHT_SHOULDER = GameObject.Find("RIGHT_SHOULDER").transform;
            LEFT_ELBOW = GameObject.Find("LEFT_ELBOW").transform;
            RIGHT_ELBOW = GameObject.Find("RIGHT_ELBOW").transform;
            LEFT_WRIST = GameObject.Find("LEFT_WRIST").transform;
            RIGHT_WRIST = GameObject.Find("RIGHT_WRIST").transform;
            LEFT_HIP = GameObject.Find("LEFT_HIP").transform;
            RIGHT_HIP = GameObject.Find("RIGHT_HIP").transform;
            LEFT_KNEE = GameObject.Find("LEFT_KNEE").transform;
            RIGHT_KNEE = GameObject.Find("RIGHT_KNEE").transform;
            LEFT_HEEL = GameObject.Find("LEFT_HEEL").transform;
            RIGHT_HEEL = GameObject.Find("RIGHT_HEEL").transform;
            tryInterval++;
            await Task.Delay(100);
        }
    }

    // Update is called once per frame
    void Update()
    {

        NOSE = GameObject.Find("NOSE").transform;
        LEFT_SHOULDER = GameObject.Find("LEFT_SHOULDER").transform;
        RIGHT_SHOULDER = GameObject.Find("RIGHT_SHOULDER").transform;
        LEFT_ELBOW = GameObject.Find("LEFT_ELBOW").transform;
        RIGHT_ELBOW = GameObject.Find("RIGHT_ELBOW").transform;
        LEFT_WRIST = GameObject.Find("LEFT_WRIST").transform;
        RIGHT_WRIST = GameObject.Find("RIGHT_WRIST").transform;
        LEFT_HIP = GameObject.Find("LEFT_HIP").transform;
        RIGHT_HIP = GameObject.Find("RIGHT_HIP").transform;
        LEFT_KNEE = GameObject.Find("LEFT_KNEE").transform;
        RIGHT_KNEE = GameObject.Find("RIGHT_KNEE").transform;
        LEFT_HEEL = GameObject.Find("LEFT_HEEL").transform;
        RIGHT_HEEL = GameObject.Find("RIGHT_HEEL").transform;


        if (NOSE != null) nose.position = NOSE.position;
        if (LEFT_SHOULDER != null) leftShoulder.position = LEFT_SHOULDER.position;
        if (RIGHT_SHOULDER != null) rightShoulder.position = RIGHT_SHOULDER.position;
        if (LEFT_ELBOW != null) leftElbow.position = LEFT_ELBOW.position;
        if (RIGHT_ELBOW != null) rightElbow.position = RIGHT_ELBOW.position;
        if (LEFT_WRIST != null) leftWrist.position = LEFT_WRIST.position;
        if (RIGHT_WRIST != null) rightWrist.position = RIGHT_WRIST.position;
        if (LEFT_KNEE != null) leftKnee.position = LEFT_KNEE.position;
        if (RIGHT_KNEE != null) rightKnee.position = RIGHT_KNEE.position;
        if (LEFT_HEEL != null) leftHeel.position = LEFT_HEEL.position;
        if (RIGHT_HEEL != null) rightHeel.position = RIGHT_HEEL.position;
        if (LEFT_HIP != null) leftHip.position = LEFT_HIP.position;
        if (RIGHT_HIP != null) rightHip.position = RIGHT_HIP.position;
    }
}
