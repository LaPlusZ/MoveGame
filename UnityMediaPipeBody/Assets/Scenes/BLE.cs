using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BLUE : MonoBehaviour
{
    private const string pipeName = "TestBLE";
    private NamedPipeServerStream pipeServer;

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
            using (pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte))
            {
                Debug.Log("Waiting for client connection...");

                // Wait for the Python client to connect
                pipeServer.WaitForConnection();
                Debug.Log("Client connected!");

                while (pipeServer.IsConnected)
                {
                    // Read the incoming message length
                    byte[] lengthBuffer = new byte[4];
                    pipeServer.Read(lengthBuffer, 0, 4); // Message length is packed as an int (4 bytes)
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    // Read the actual message
                    byte[] messageBuffer = new byte[messageLength];
                    pipeServer.Read(messageBuffer, 0, messageLength);

                    string receivedMessage = Encoding.UTF8.GetString(messageBuffer);
                    Debug.Log($"Received from client: {receivedMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Pipe Server Error: {ex.Message}");
        }
    }

    void OnApplicationQuit()
    {
        // Close the pipe when the application quits
        if (pipeServer != null && pipeServer.IsConnected)
        {
            pipeServer.Close();
            Debug.Log("Pipe server closed.");
        }
    }
}
