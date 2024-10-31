using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate3DObject : MonoBehaviour
{
    #region Input Actions
    [SerializeField]
    private InputActionAsset _actions;

    public InputActionAsset actions
    {
        get => _actions;
        set => _actions = value;
    }

    protected InputAction leftClickPressedInputAction { get; set; }
    protected InputAction mouseLookInputAction { get; set; }
    private InputAction toggleRotateModeAction;
    #endregion

    #region Variables
    private bool _rotateAllowed;
    private bool _rotateModeEnabled;
    private Camera _camera;
    [SerializeField] private float _speed = 100f;
    [SerializeField] private bool _inverted;
    [SerializeField] private float minYAngle = -45f;
    [SerializeField] private float maxYAngle = 45f;
    private float currentYRotation;
    #endregion

    private void Awake()
    {
        InitializeInputSystem();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Start with unlocked cursor for UI interactions
        _camera = Camera.main;
        currentYRotation = transform.eulerAngles.y;
    }

    private void InitializeInputSystem()
    {
        leftClickPressedInputAction = actions.FindAction("Left Click");
        if (leftClickPressedInputAction != null)
        {
            leftClickPressedInputAction.started += OnLeftClickPressed;
            leftClickPressedInputAction.performed += OnLeftClickPressed;
            leftClickPressedInputAction.canceled += OnLeftClickPressed;
        }

        mouseLookInputAction = actions.FindAction("Mouse Look");

        toggleRotateModeAction = new InputAction("ToggleRotateMode", binding: "<Keyboard>/t");
        toggleRotateModeAction.performed += ToggleRotateMode;
        toggleRotateModeAction.Enable();

        actions.Enable();
    }

    protected virtual void OnLeftClickPressed(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
            _rotateAllowed = true;
        else if (context.canceled)
            _rotateAllowed = false;
    }

    protected virtual Vector2 GetMouseLookInput()
    {
        return mouseLookInputAction != null ? mouseLookInputAction.ReadValue<Vector2>() : Vector2.zero;
    }

    private void ToggleRotateMode(InputAction.CallbackContext context)
    {
        _rotateModeEnabled = !_rotateModeEnabled;

        // Toggle cursor lock state based on rotate mode
        Cursor.lockState = _rotateModeEnabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_rotateModeEnabled; // Show the cursor when not in rotate mode
    }

    private void Update()
    {
        // Only allow rotation if rotate mode is enabled and the cursor is locked
        if (!_rotateAllowed || !_rotateModeEnabled || Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 mouseDelta = GetMouseLookInput();
        mouseDelta *= _speed * Time.deltaTime;

        // Update the current Y rotation based on mouse input
        currentYRotation += (_inverted ? 1 : -1) * mouseDelta.x;

        // Clamp the Y rotation within the set limits
        currentYRotation = Mathf.Clamp(currentYRotation, minYAngle, maxYAngle);

        // Apply the clamped rotation to the transform
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }

    private void OnDestroy()
    {
        toggleRotateModeAction.performed -= ToggleRotateMode;
    }
}
