using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour
{
    private InputAction jumpAction;
    private InputAction moveAction;
    private Vector2 moveInput;
    
    // movement
    [SerializeField] private float speed = 7f;
    [SerializeField] private float turnSpeed = 120f;
    
    // jumping and ground check
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundMask = ~0;
    
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent player falling
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        if (jumpAction == null)
            Debug.LogError("Jump action not found in Input Actions!");
        
        if (jumpAction != null)
            jumpAction.performed += OnJumpPerformed; // give the jumpAction the OnJumpPerformed function
        
    }
    
    void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        // check if ground overlap spehere 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    void FixedUpdate()
    {
        // Set move,emt velocity using rigid body
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 targetVelocity = forward * speed;
        
        Vector3 velocity = rb.linearVelocity; 
        
        // move set linear velocity to change in x/z
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;
        
        // set rotation
        Quaternion turn = Quaternion.Euler(0f, moveInput.x * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);
        
        // jump
        if (jumpQueued && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * jumpHeight);
            rb.linearVelocity = new Vector3(velocity.x, jumpVelocity, velocity.z);
        }
        jumpQueued = false; // block jumping while in air
        
        // transform.Translate(Vector3.forward * (moveInput.y * speed * Time.deltaTime));
        // transform.Rotate(Vector3.up * (moveInput.x * turnSpeed * Time.deltaTime));
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        jumpQueued = true; // allow jumping
    }
}
