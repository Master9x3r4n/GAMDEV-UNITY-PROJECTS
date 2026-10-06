using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputAction jumpAction;
    private Vector2 moveInput;
    
    [SerializeField] private MovementStats stats;
    [SerializeField] private float groundDistance = 0.15f;
    [SerializeField] private LayerMask groundMask = ~0;

    private Rigidbody rb;
    private Collider col;
    private bool isGrounded;
    private bool jumpQueued;
    private bool jumpLocked;      
    private float lastJumpTime;

    private readonly RaycastHit[] hits = new RaycastHit[8];

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        EnableMovement();
        jumpAction = InputSystem.actions.FindAction("Jump");

        // setup jump action
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
    }

    void EnableMovement()
    {
        stats.currSpeed = stats.speed;
        stats.currTurnSpeed = stats.turnSpeed;
    }

    void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;
    }

    void FixedUpdate()
    {
        CheckGround();

        // Release jump lock after landing
        if (jumpLocked && isGrounded && Time.time > lastJumpTime + 0.15f && rb.linearVelocity.y <= 0.01f)
            jumpLocked = false;

        // calculate movement direction from raw input (Y = horizontal, X = vertical)
        Vector3 moveDirection = new Vector3(moveInput.y, 0f, -moveInput.x).normalized;

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

        // rotate character to face the movement direction
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, stats.currTurnSpeed * Time.fixedDeltaTime));
        }

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

            isGrounded = true; // validates if in ground based on raycast
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