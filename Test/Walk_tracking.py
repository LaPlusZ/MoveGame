import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import numpy as np
################
from CopyOfBody import CaptureThread,BodyThread
import CopyOfGlobalVar as global_vars
################

thread = BodyThread()
thread.run()
i = input()
print("Exiting…")        
global_vars.KILL_THREADS = True
time.sleep(0.5)
exit()