import threading
import time
import struct
import asyncio
from bleak import BleakClient

# UUID for the Heart Rate Measurement characteristic
HEART_RATE_MEASUREMENT_UUID = "00002a37-0000-1000-8000-00805f9b34fb"

# Global variable structure for control
class global_vars:
    KILL_THREADS = False
    heart_rate = None  # To store the heart rate value globally

class BodyThreadBLE(threading.Thread):
    def __init__(self):
        super().__init__()
        self.data = "Hello World"
        self.pipe = None
        self.timeSinceCheckedConnection = 0
        self.address = "E4:50:72:32:DF:89"  # Replace with your BLE device's MAC address
        self.loop = None  # To store the asyncio event loop

    # Callback function to handle heart rate data
    def heart_rate_handler(self, sender, data):
        heart_rate = data[1]  # The second byte contains the heart rate value
        global_vars.heart_rate = heart_rate  # Store heart rate globally
        print(f"Heart Rate: {heart_rate} bpm")

    # Async function to read heart rate from BLE device
    async def read_heart_rate(self, address):
        async with BleakClient(address) as client:
            print(f"Connected to {address}")
            # Register the heart rate handler
            await client.start_notify(HEART_RATE_MEASUREMENT_UUID, self.heart_rate_handler)

            try:
                while not global_vars.KILL_THREADS:
                    await asyncio.sleep(1)  # Keep the loop running, handle notifications
            except asyncio.CancelledError:
                await client.stop_notify(HEART_RATE_MEASUREMENT_UUID)
                print("Stopped receiving heart rate notifications")

    def run(self):
        # Create a new asyncio loop for BLE operations
        self.loop = asyncio.new_event_loop()
        asyncio.set_event_loop(self.loop)

        # Run the BLE heart rate reading in a separate task
        heart_rate_task = self.loop.create_task(self.read_heart_rate(self.address))

        # Run the event loop in a separate thread
        threading.Thread(target=self.loop.run_forever).start()

        # Pipe server logic
        while not global_vars.KILL_THREADS:
            if self.pipe is None and time.time() - self.timeSinceCheckedConnection >= 1:
                try:
                    # Try to connect to the Unity pipe
                    self.pipe = open(r'\\.\pipe\TestBLE', 'r+b', 0)
                    print("Connected to Unity Pipe")
                except FileNotFoundError:
                    print("Waiting for Unity project to run...")
                    self.pipe = None
                self.timeSinceCheckedConnection = time.time()

            if self.pipe is not None and global_vars.heart_rate is not None:
                try:
                    # Send the heart rate data to the Unity pipe
                    heart_rate_data = f"Heart Rate: {global_vars.heart_rate} bpm"
                    s = heart_rate_data.encode('utf-8')  # Encode the heart rate message
                    self.pipe.write(struct.pack('I', len(s)) + s)  # Pack the message with its length
                    self.pipe.seek(0)  # Reset the pipe's position to the beginning
                    print(f"Sent to Unity: {heart_rate_data}")
                except Exception as ex:
                    print(f"Failed to write to pipe: {ex}. Is the Unity project open?")
                    self.pipe = None  # Reset pipe connection on failure

            time.sleep(1)  # Sleep for 1 second between sending

        # Clean up
        if self.pipe:
            self.pipe.close()  # Close the pipe when the thread stops

        # Cancel the BLE task when the thread is stopped
        if heart_rate_task:
            heart_rate_task.cancel()
            self.loop.stop()

# Main logic that starts the body thread and runs for a given period
# if __name__ == "__main__":
#     body_thread = BodyThreadBLE()
#     body_thread.start()

#     # Run for 30 seconds for demo purposes
#     time.sleep(30)

#     global_vars.KILL_THREADS = True  # Stop the thread after the demo period
#     body_thread.join()  # Wait for the thread to terminate
