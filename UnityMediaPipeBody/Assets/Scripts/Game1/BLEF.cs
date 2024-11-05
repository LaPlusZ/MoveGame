using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BLE : MonoBehaviour
{
    private const string pipeName = "TestBLE";
    private NamedPipeServerStream pipeServer;
    public TMP_Text messageDisplay;
    public float HeartRate; // Change this to float

    private Queue<string> messageQueue = new Queue<string>(); // Queue for messages
    private bool isRunning = true; // Control the while loop in StartPipeServer

    void Start()
    {
        // Start the named pipe server on a separate thread
        Task.Run(() => StartPipeServer());
    }

    void StartPipeServer()
    {
        try
        {
            // Create the pipe server
            using (pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 10, PipeTransmissionMode.Byte))
            {
                Debug.Log("Waiting for client connection...");

                // Wait for the Python client to connect
                pipeServer.WaitForConnection();
                Debug.Log("Client connected!");

                while (isRunning && pipeServer.IsConnected)
                {
                    // Read the incoming message length
                    byte[] lengthBuffer = new byte[4];
                    pipeServer.Read(lengthBuffer, 0, 4); // Message length is packed as an int (4 bytes)
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    // Read the actual message
                    byte[] messageBuffer = new byte[messageLength];
                    pipeServer.Read(messageBuffer, 0, messageLength);

                    string receivedMessage = Encoding.UTF8.GetString(messageBuffer);

                    // Enqueue the received message
                    lock (messageQueue) // Lock for thread safety
                    {
                        messageQueue.Enqueue(receivedMessage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Pipe Server Error: {ex.Message}");
        }
        finally
        {
            // Ensure the server is properly closed
            if (pipeServer != null)
            {
                pipeServer.Close();
                Debug.Log("Pipe server closed.");
            }
        }
    }

    void Update()
    {
        // Check the queue for new messages and update the UI
        if (messageQueue.Count > 0)
        {
            string message;
            lock (messageQueue) // Lock for thread safety
            {
                message = messageQueue.Dequeue();
            }
            messageDisplay.text = message; // Update the TMP_Text component on the main thread
            UpdateHeartRate(message);
        }
    }

    void OnApplicationQuit()
    {
        // Signal to stop the server loop
        isRunning = false;

        // Close the pipe when the application quits
        if (pipeServer != null && pipeServer.IsConnected)
        {
            try
            {
                pipeServer.Close();
                Debug.Log("Pipe server closed on application quit.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error closing pipe server: {ex.Message}");
            }
        }
    }

    public void UpdateHeartRate(string newHeartRate)
    {
        // Attempt to parse the incoming string to a float
        if (float.TryParse(newHeartRate, out float result))
        {
            HeartRate = result; // Assign the parsed float value to HeartRate
        }
        else
        {
            Debug.LogError("Invalid heart rate input: " + newHeartRate);
        }
    }
}
