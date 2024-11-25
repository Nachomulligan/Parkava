using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assignables")]
    public Transform playerCam;
    public Transform orientation;


    [Header("Rotation and Look Settings")]
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    private float desiredX;

    [Header("Movement Settings")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    [Header("MoveSettings")]
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [Header("Movement Multipliers")]
    [SerializeField] public float airMultiplier = 0.5f;
    [SerializeField] public float slideMultiplier = 0f;

    [Header("Crouch & Slide Settings")]
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    [Header("Jump Settings")]
    private bool readyToJump = true;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] public float jumpForce = 550f;
    // Coyote time
    [SerializeField] public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    [SerializeField] private int maxJumpCount = 2;
    public int currentJumpCount = 0;

    // Input
    float x, y;
    bool jumping, sprinting, crouching;

    // Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;


    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        MyInput();
        Look();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
        }
        crouching = Input.GetKey(KeyCode.LeftControl);

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    /// <summary>
    /// Adjusts player scale for crouching and applies slide force if the player is moving while grounded.
    /// </summary>
    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    /// <summary>
    /// Resets the player scale after crouching and repositions the player slightly upwards.
    /// </summary>
    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    /// <summary>
    /// Manages player movement by applying forces based on input, gravity, and surface angle.
    /// Limits the speed and adjusts for air and slide conditions.
    /// </summary>
    private void Movement()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        CounterMovement(x, y, mag);

        if (jumping)
        {
            Jump();
            jumping = false;
        }

        float maxSpeed = this.maxSpeed;
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = airMultiplier;
            multiplierV = airMultiplier;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = slideMultiplier;

        // Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    /// <summary>
    /// Detects when the player collides with the ground, reinitializing coyote time and resetting the jump count.
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        // Reinitialize coyote time
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            coyoteTimeCounter = coyoteTime;
            currentJumpCount = 0;
        }
    }

    /// <summary>
    /// Detects when the player leaves contact with the ground, marking them as airborne.
    /// </summary>
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
        }
    }

    private void LateUpdate()
    {
        if (!grounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Manages jump mechanics, including ground jump and mid-air jump with coyote time and jump count.
    /// </summary>
    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;
            currentJumpCount ++;
            
            var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
            if (audioService != null)
            {
                audioService.PlaySFX("Jump");

            }

            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            // Reset Y vel if we are falling
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);


            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (!grounded && currentJumpCount < maxJumpCount)
        {
            var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
            if (audioService != null)
            {
                audioService.PlaySFX("Jump");

            }
            currentJumpCount++;
            rb.AddForce(Vector2.up * jumpForce * 1.2f);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
        if (grounded)
        {
            currentJumpCount = 0;
        }
    }

    public void SetMaxJumpCount(int value)
    {
        maxJumpCount = value;
    }

    /// <summary>
    /// Rotates the player camera and orientation based on mouse input, clamping the rotation to avoid over-rotation.
    /// </summary>
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        // Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // make the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }


    /// <summary>
    /// Counters player movement to reduce sliding or excessive speed in unintended directions.
    /// </summary>
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        // Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        // Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        // Limit diagonal running
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);
        float yMag = magnitude * Mathf.Sin(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    
}