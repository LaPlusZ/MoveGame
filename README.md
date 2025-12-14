# MoveGame 🎮🏃

**MoveGame** is a real-time motion capture game project that bridges **Python (MediaPipe)** with **Unity**. It captures body movements via webcam and streams the data to Unity to control characters or game elements.

---

## 🚀 Getting Started

### Prerequisites

* **Python 3.7+**
* **Unity Hub & Editor** (Recommended: 2020.3 LTS or newer)
* **Webcam**

### 1. Python Setup (Tracking)

The Python script handles the computer vision and sends data to Unity.

1.  Navigate to the python folder:
    ```bash
    cd mediapipebody
    ```

2.  Install the required dependencies:
    ```bash
    pip install mediapipe opencv-python
    ```

3.  Run the tracking script:
    ```bash
    python main.py
    ```
    *A window should open showing your webcam feed with skeletal landmarks.*

### 2. Unity Setup (Game)

1.  Open **Unity Hub**.
2.  Click **Add** and select the `UnityMediaPipeBody` folder from this repository.
3.  Open the project.
4.  Navigate to `Assets/Scenes` and open the main scene.
5.  Press the **Play** button.

---

## 📡 How It Works

1.  **Capture**: The Python script uses `OpenCV` to capture video and `MediaPipe` to detect 33 body landmarks in real-time.
2.  **Transmit**: Coordinate data is processed and sent to localhost (`127.0.0.1`) via **UDP** (User Datagram Protocol).
3.  **Render**: Unity listens for the UDP packets, parses the coordinates, and updates the game character's movement.

---

## 🍎 macOS Users

If you encounter issues with the camera or libraries in the standard folder, please use the scripts located in **`mediapipebody MACOS/`**.

---

## 🛠️ Troubleshooting

* **Unity Character Not Moving**:
    * Ensure the Python script is running *while* Unity is in Play mode.
    * Check that the **Port** number in the Python script matches the `UDPReceive` script in Unity (usually port `5052`).
* **Camera Not Opening**:
    * Close other applications using the camera (Zoom, Discord, Teams).
    * Check your privacy settings to allow the terminal/IDE to access the camera.

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
