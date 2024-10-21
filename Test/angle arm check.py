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
        left_shoulder = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_SHOULDER].x,
                         result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_SHOULDER].y]
        left_elbow = [
            result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_ELBOW].x,
            result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_ELBOW].y
        ]

        left_hip = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_HIP].x,
                    result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_HIP].y]
        left_wrist = [result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_WRIST].x,
                       result.pose_landmarks.landmark[mp_pose.PoseLandmark.LEFT_WRIST].y]
        
        # Calculate the angle from hip to shoulder to wrist (hip-shoulder-wrist)
        hip_to_shoulder_to_wrist_angle = calculate_angle(left_shoulder,left_elbow, left_wrist)

        # Display the angle on the frame.
        cv2.putText(frame, f'Hip to Shoulder to Wrist Angle: {int(hip_to_shoulder_to_wrist_angle)} degrees', 
                    (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)

    # Display the output frame.
    cv2.imshow('Body Detection', frame)
    
    # Exit on pressing 'q'.
    if cv2.waitKey(10) & 0xFF == ord('q'):
        break

# Release the webcam and close all OpenCV windows.
cap.release()
cv2.destroyAllWindows()
