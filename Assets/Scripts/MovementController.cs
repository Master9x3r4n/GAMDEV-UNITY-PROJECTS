using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputAction jumpAction;
    private Vector2 moveInput;

    [SerializeField] private float speed = 7f;
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float jumpHeight = 1.5f;
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
        jumpAction = InputSystem.actions.FindAction("Jump");

        // setup jump action
        if (jumpAction != null)
            jumpAction.performed += OnJumpPerformed;
    }

    void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;
    }

    void FixedUpdate()
    {
        CheckGround();

        // release the lock only after a real landing (short delay so the
        // frames right after takeoff can't count as landed)
        if (jumpLocked && isGrounded && Time.time > lastJumpTime + 0.15f && rb.linearVelocity.y <= 0.01f)
            jumpLocked = false;

        // movement
        Vector3 targetVelocity = transform.forward * (moveInput.y * speed);
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        // rotation
        Quaternion turn = Quaternion.Euler(0f, moveInput.x * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // jump
        if (jumpQueued && isGrounded && !jumpLocked)
        {
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
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