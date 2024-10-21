#pipe server
from body import BodyThread
import time
import struct
import global_vars
from sys import exit


thread = BodyThread()
thread.start()

while True:
    i = input("Type 'exit' to close: ")
    if i.lower() == "exit":  # Check if input is 'exit'
        print("Exiting…")
        global_vars.KILL_THREADS = True
        time.sleep(0.5)
        exit()