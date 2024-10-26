import threading
import time
import struct
import asyncio
from bleak import BleakClient


# Example global variable structure for simulation
class global_vars:
    KILL_THREADS = False


class BodyThread(threading.Thread):
    def __init__(self):
        super().__init__()
        self.data = "Hello World"
        self.pipe = None
        self.timeSinceCheckedConnection = 0
        self.address = "E4:50:72:32:DF:89"  # Replace with your device's MAC address
        self.heart_rate = None
    async def heart_rate_handler(self, sender, data):
        heart_rate = data[1]  # The second byte contains the heart rate value
        # print(f"Heart Rate: {heart_rate} bpm")

    async def read_heart_rate(self, address):
        HEART_RATE_MEASUREMENT_UUID = "00002a37-0000-1000-8000-00805f9b34fb"
        async with BleakClient(address) as client:
            print(f"Connected to {address}")
            await client.start_notify(HEART_RATE_MEASUREMENT_UUID, self.heart_rate_handler)

            try:
                while not global_vars.KILL_THREADS:
                    await asyncio.sleep(1)  # Keep the loop running, handle notifications
            except asyncio.CancelledError:
                await client.stop_notify(HEART_RATE_MEASUREMENT_UUID)
                print("Stopped receiving heart rate notifications")

    def run(self):
        loop = asyncio.new_event_loop()
        asyncio.set_event_loop(loop)
        while not global_vars.KILL_THREADS:
            if self.pipe is None and time.time() - self.timeSinceCheckedConnection >= 1:
                try:
                    # Try to connect to the Unity pipe
                    self.pipe = open(r'\\.\pipe\UnityMediaPipeBody', 'r+b', 0)
                    print("Connected to Unity Pipe")
                except FileNotFoundError:
                    print("Waiting for Unity project to run...")
                    self.pipe = None
                self.timeSinceCheckedConnection = time.time()

            if self.pipe is not None:
                try:
                    # Start reading heart rate asynchronously
                    loop.run_until_complete(self.read_heart_rate(self.address))

                    # Send "Hello World" to the Unity pipe
                    s = self.heart_rate.encode('utf-8')  # Encode the string "Hello World"
                    self.pipe.write(struct.pack('I', len(s)) + s)  # Pack the message with its length
                    self.pipe.seek(0)  # Reset the pipe's position to the beginning
                    print(f"Sent to Unity: {self.heart_rate}")
                except Exception as ex:
                    print(f"Failed to write to pipe: {ex}. Is the Unity project open?")
                    self.pipe = None  # Reset pipe connection on failure

            time.sleep(1)  # Sleep for 1 second between sending

        if self.pipe:
            self.pipe.close()  # Close the pipe when the thread stops


# Simulate the main logic that starts the body thread
if __name__ == "__main__":
    body_thread = BodyThread()
    body_thread.start()

    # Run for 10 seconds for demo purposes
    time.sleep(10)

    global_vars.KILL_THREADS = True  # Stop the thread after 10 seconds
    body_thread.join()  # Wait for the thread to terminate
