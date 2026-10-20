using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementStats stats;
    [SerializeField] private Transform cameraTransform;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLimit = 80f;

    [Header("Ground Check")]
    [SerializeField] private float groundDistance = 0.15f;
    [SerializeField] private LayerMask groundMask = ~0;

    private InputAction jumpAction;
    private InputAction lookAction;
    private Vector2 moveInput;

    private Rigidbody rb;
    private Collider col;
    private bool isGrounded;
    private bool jumpQueued;
    private bool jumpLocked;
    private float lastJumpTime;

    // look state
    private float verticalRotation;
    private float pendingYaw;     
    private bool lookEnabled = true;

    private readonly RaycastHit[] hits = new RaycastHit[8];

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.freezeRotation = true;
        // Smooths the body/camera between physics steps so mouse look doesn't stutter
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        EnableMovement();

        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");

        if (jumpAction != null)
            jumpAction.performed += OnJumpPerformed;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += DisableMovement;
        GameManager.OnGameStart += EnableMovement;
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= DisableMovement;
        GameManager.OnGameStart -= EnableMovement;
    }

    void DisableMovement()
    {
        stats.currSpeed = 0;
        stats.currTurnSpeed = 0;
        lookEnabled = false;
        pendingYaw = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void EnableMovement()
    {
        stats.currSpeed = stats.speed;
        stats.currTurnSpeed = stats.turnSpeed;
        lookEnabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;
    }

    void Update()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        if (!lookEnabled || lookAction == null || cameraTransform == null) return;

        Vector2 delta = lookAction.ReadValue<Vector2>() * mouseSensitivity;

        // pitch: applied directly to the camera
        verticalRotation = Mathf.Clamp(verticalRotation - delta.y, -upDownLimit, upDownLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // yaw: queued, applied to the rigidbody in FixedUpdate
        pendingYaw += delta.x;
    }

    void FixedUpdate()
    {
        CheckGround();

        // Release jump lock after landing
        if (jumpLocked && isGrounded && Time.time > lastJumpTime + 0.15f && rb.linearVelocity.y <= 0.01f)
            jumpLocked = false;

        // apply queued yaw through the physics engine
        if (Mathf.Abs(pendingYaw) > 0f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, pendingYaw, 0f));
            pendingYaw = 0f;
        }

        // movement relative to where the player is facing (x = strafe, y = forward)
        Quaternion facing = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
        Vector3 moveDirection = facing * new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // apply movement velocity
        Vector3 targetVelocity = moveDirection * stats.currSpeed;
        Vector3 velocity = rb.linearVelocity;

        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        if (!isGrounded)
        {
            // applies extra downward force, increasing fall velocity every frame
            velocity.y += Physics.gravity.y * (stats.fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        rb.linearVelocity = velocity;

        // jump logic
        if (jumpQueued && isGrounded && !jumpLocked)
        {
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * stats.jumpHeight);
            rb.linearVelocity = new Vector3(velocity.x, jumpVelocity, velocity.z);
            jumpLocked = true;
            lastJumpTime = Time.time;
            isGrounded = false;
        }

        jumpQueued = false;
    }

    private void CheckGround()
    {
        // do a raycast on the ground to check if it is touching it
        Bounds b = col.bounds;
        Vector3 origin = b.center;
        float distance = b.extents.y + groundDistance;

        int count = Physics.RaycastNonAlloc(origin, Vector3.down, hits, distance,
                                            groundMask, QueryTriggerInteraction.Ignore);

        isGrounded = false;
        for (int i = 0; i < count; i++)
        {
            // skip the player's own colliders
            if (hits[i].collider.transform.IsChildOf(transform)) continue;
            if (hits[i].collider.attachedRigidbody == rb) continue;

            isGrounded = true;
            break;
        }
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        jumpQueued = true;
    }
}