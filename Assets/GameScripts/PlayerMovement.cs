using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    private Transform origin_orientation;

    float horizontalInput;
    float verticalInput;

    float runningVel = 1.5f;
    bool running = false;

    Vector3 moveDirection;

    [HideInInspector]
    public bool canMove;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        origin_orientation = orientation;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Check if is on ground
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.4f, whatIsGround);

            getInput();
            movePlayer();
            speedControl();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            orientation = origin_orientation;
        }

        running = Input.GetKey(KeyCode.LeftControl);
    }

    private void getInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float runningMultiplier = running ? runningVel : 1;
        
        if(grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f * runningMultiplier, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void resetJump()
    {
        readyToJump = true;
    }


    public Vector3 getPos()
    {
        return rb.position;
    }

    public void eject()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * 50, ForceMode.Impulse);
    }
}
