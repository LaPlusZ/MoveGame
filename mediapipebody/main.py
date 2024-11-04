#pipe server
from body import BodyThread
import time
import struct
import global_vars
from bluetooth import BodyThreadBLE
from sys import exit

threadBlE = BodyThreadBLE()
threadBlE.start()

thread = BodyThread()
thread.start()

i = input()
print("Exiting…")        
global_vars.KILL_THREADS = True
time.sleep(0.5)
exit()