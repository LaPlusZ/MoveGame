import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import numpy as np
################
from CopyOfBody import CaptureThread,BodyThread
import CopyOfGlobalVar as global_vars
from Walk_tracking import compute_real_world_landmarks
image_landmarks = results.pose_landmarks
iworld_landmarks = results.pose_world_landmarks
model_points = np.float32([[-l.x, -l.y, -l.z] for l in world_landmarks.landmark])
image_points = np.float32([[l.x * image.shape[1], l.y * image.shape[0]] for l in image_landmarks.landmark])
                        
body_world_landmarks_world = self.compute_real_world_landmarks(model_points,image_points,image.shape)