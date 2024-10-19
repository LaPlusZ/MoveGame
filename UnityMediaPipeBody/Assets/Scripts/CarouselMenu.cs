using UnityEngine;

public class SmoothCarouselMenu : MonoBehaviour
{
    public Transform[] planes; // Array of planes (3 planes)
    public float dragSensitivity = 0.005f; // Sensitivity of mouse drag
    public float smoothingFactor = 10f; // Smoothing factor for smooth scrolling
    public float scrollCooldown = 0.2f; // Time to wait after scroll input before snapping

    public float scrollPosition = 2f; // Start at the middle plane (position 2)
    private float targetPosition = 1f; // Target position to scroll toward
    private float scrollCooldownTimer;

    // Positions and rotations for the planes at 0 to 4
    private Vector3[] positions = new Vector3[5]
    {
        new Vector3(-14f, 0f, 8f),  // Position 0 (far left)
        new Vector3(-10.7f, 0f, 0f), // Position 1 (left)
        new Vector3(0f, 0f, -4.81f), // Position 2 (center)
        new Vector3(10.7f, 0f, 0f),  // Position 3 (right)
        new Vector3(14f, 0f, 8f)     // Position 4 (far right)
    };

    private Vector3[] rotations = new Vector3[5]
    {
        new Vector3(-90f, 0f, 50f),   // Rotation 0 (far left)
        new Vector3(-90f, 0f, 25f),   // Rotation 1 (left)
        new Vector3(-90f, 0f, 0f),    // Rotation 2 (center)
        new Vector3(-90f, 0f, -25f),  // Rotation 3 (right)
        new Vector3(-90f, 0f, -50f)   // Rotation 4 (far right)
    };

    void Start()
    {
        // Initialize the target position
        targetPosition = scrollPosition;
        UpdatePlanePositions();
    }

    void Update()
    {
        // Handle input from mouse wheel and mouse drag
        HandleInput();

        // Smoothly scroll toward the target position
        scrollPosition = Mathf.Lerp(scrollPosition, targetPosition, Time.deltaTime * smoothingFactor);
        UpdatePlanePositions();

        // Check if the cooldown timer should snap after user finishes scrolling
        if (scrollCooldownTimer > 0)
        {
            scrollCooldownTimer -= Time.deltaTime;
            if (scrollCooldownTimer <= 0)
            {
                SnapToNearestPage();
            }
        }
    }

    void HandleInput()
    {
        // Handle scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetPosition -= scrollInput;
            ClampTargetPosition();
            scrollCooldownTimer = scrollCooldown; // Reset cooldown timer after scrolling
        }

        // Handle mouse drag input
        if (Input.GetMouseButton(0))
        {
            float dragDeltaX = Input.GetAxis("Mouse X");
            targetPosition -= dragDeltaX * dragSensitivity;
            ClampTargetPosition();
        }

        // Snap to nearest page on mouse release
        if (Input.GetMouseButtonUp(0))
        {
            SnapToNearestPage();
        }
    }

    void SnapToNearestPage()
    {
        // Snap target position to nearest whole number (1 to 3)
        targetPosition = Mathf.Round(targetPosition);
        ClampTargetPosition();
    }

    void ClampTargetPosition()
    {
        // Clamp the target position between 1 and 3 (only scroll between pages 1 and 3)
        targetPosition = Mathf.Clamp(targetPosition, 1f, 3f);
    }

        void UpdatePlanePositions()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            // Calculate the relative offset of each plane based on the current scroll position
            float planeOffset = i + 1 - scrollPosition;

            // Clamp the plane offset between -2 and 2 to cover far-left to far-right
            planeOffset = Mathf.Clamp(planeOffset, -2f, 2f);

            Vector3 newPosition;
            Vector3 newRotation;

            // Interpolate based on the plane offset
            if (planeOffset <= 0) // Interpolate between far-left (0) and center (2)
            {
                if (planeOffset >= -1)
                {
                    newPosition = Vector3.Lerp(positions[1], positions[2], planeOffset+1);
                    newRotation = Vector3.Lerp(rotations[1], rotations[2], planeOffset+1);
                }
                else 
                {   
                    newPosition = Vector3.Lerp(positions[0], positions[1], planeOffset+2);
                    newRotation = Vector3.Lerp(rotations[0], rotations[1], planeOffset+2);    
                }
            }
            else // Interpolate between center (2) and far-right (4)
            {
                if (planeOffset <= 1)
                {
                    newPosition = Vector3.Lerp(positions[2], positions[3], planeOffset);
                    newRotation = Vector3.Lerp(rotations[2], rotations[3], planeOffset);
                }
                else 
                {   
                    newPosition = Vector3.Lerp(positions[3], positions[4], planeOffset-1);
                    newRotation = Vector3.Lerp(rotations[3], rotations[4], planeOffset-1);  
                }
            }

            // Set the new position and rotation for the plane
            planes[i].position = newPosition;
            planes[i].rotation = Quaternion.Euler(newRotation);
        }
    }
}