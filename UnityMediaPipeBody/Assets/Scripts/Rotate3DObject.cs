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
    #endregion

    #region Variables
    private bool _rotateAllowed;
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
        Cursor.lockState = CursorLockMode.Locked;
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
        if (mouseLookInputAction != null)
            return mouseLookInputAction.ReadValue<Vector2>();

        return Vector2.zero;
    }

    private void Update()
    {
        if (!_rotateAllowed)
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
}
