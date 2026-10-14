using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 0.1f;
    public float upDownLimit = 80.0f;

    private CharacterController controller;
    private InputSystem_Actions actions;

    private Vector3 velocity;
    private float verticalRotation = 0f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.Look.performed += OnLook;
        actions.Player.Look.canceled += OnLook;
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Move.performed -= OnMove;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Look.performed -= OnLook;
        actions.Player.Look.canceled -= OnLook;
        actions.Player.Disable();

        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
    }

    private void OnDestroy() => actions.Dispose();

    private void Start()
    {
        // fallback to Main Camera if unassigned in Inspector
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    private void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        Vector2 delta = lookInput * mouseSensitivity;

        verticalRotation -= delta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * delta.x);
    }

    private void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}