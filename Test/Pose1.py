import cv2
import mediapipe as mp
import numpy as np

# Initialize Mediapipe pose class.
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(static_image_mode=False, min_detection_confidence=0.5, min_tracking_confidence=0.5)

# Initialize Mediapipe drawing utility.
mp_drawing = mp.solutions.drawing_utils

# Function to calculate angle between three points.
def calculate_angle(a, b, c):
    a = np.array(a)  # First point (hip)
    b = np.array(b)  # Middle point (shoulder)
    c = np.array(c)  # End point (wrist)
    
    # Calculate the angle between the vectors
    radians = np.arctan2(c[1] - b[1], c[0] - b[0]) - np.arctan2(a[1] - b[1], a[0] - b[0])
    angle = np.abs(radians * 180.0 / np.pi)  # Convert to degrees
    
    if angle > 180.0:
        angle = 360 - angle
        
    return angle

# Open the webcam.
cap = cv2.VideoCapture(0)

while cap.isOpened():
    ret, frame = cap.read()
    
    # Convert the frame from BGR to RGB as Mediapipe works with RGB images.
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    
    # Process the frame to detect pose landmarks.
    result = pose.process(rgb_frame)
    
    # If landmarks are detected, draw them and calculate arm angles.
    if result.pose_landmarks:
        mp_drawing.draw_landmarks(frame, result.pose_landmarks, mp_pose.POSE_CONNECTIONS)
        
        # Get landmark coordinates for the left side (hip, shoulder, wrist).
       
        # Get landmark coordinates for the left knee (hip, knee, ankle).
        left_hip = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_HIP].x,
                    result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_HIP].y]
        left_knee = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_KNEE].x,
                     result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_KNEE].y]
        left_ankle = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_ANKLE].x,
                      result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_ANKLE].y]
        right_hip = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_HIP].x,
                result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_HIP].y]
        right_knee = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_KNEE].x,
                result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_KNEE].y]
        right_ankle = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_ANKLE].x,
                result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_ANKLE].y]

        left_shoulder = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_SHOULDER].x,
                         result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_SHOULDER].y]
        left_wrist = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_WRIST].x,
                       result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_WRIST].y]
        right_shoulder = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_SHOULDER].x,
                         result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_SHOULDER].y]
        right_wrist = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_WRIST].x,
                       result.pose_landmarks.landmark[mp_pose.PoseLandmark.RIGHT_WRIST].y]

        
        # Calculate the angle from hip to shoulder to wrist (hip-shoulder-wrist)
        LHAngle = calculate_angle(left_hip, left_shoulder, left_wrist)
        RHAngle = calculate_angle(right_hip, right_shoulder, right_wrist)
        
        RLAngle = calculate_angle(right_hip, right_knee, right_ankle)

        if RHAngle >=80 and LHAngle>=80:
            if RLAngle <=130:
                state="Start"
                print(state)
    # Display the output frame.
    cv2.imshow('Body Detection', frame)
    
    # Exit on pressing 'q'.
    if cv2.waitKey(10) & 0xFF == ord('q'):
        break

# Release the webcam and close all OpenCV windows.
cap.release()
cv2.destroyAllWindows()
