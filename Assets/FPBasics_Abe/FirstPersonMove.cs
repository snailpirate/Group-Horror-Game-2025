using UnityEngine;
using UnityEngine.InputSystem;

//This script uses the new input system via callbacks, and moves the character in first person.
public class FirstPersonMove : MonoBehaviour
{

    private CharacterController controller; //reference to character controller
    private Vector2 moveInput; //stores input from players (WASD and others)
    private Vector3 velocity; //Movement for jumping and gravity (no physics)

    [Header("Movement Settings")]
    public float moveSpeed = 5.0f; //speed in meters per second
    public float jumpHeight = 2.0f; //jump height in meters

    [Header("Gravity Settings")]
    public float gravity = -9.8f; //fall speed (should be negative)
    private bool isGrounded; //check if is grounded

    [Header("Ground Check Settings")]
    public Transform groundCheck; //empty object located below the player to check if is grounded
    public float groundDistance = 0.4f; // Radius of the sphere used to detect the ground
    public LayerMask groundMask; // Layer(s) that count as ground


    // Called once at the beginning to cache the CharacterController
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    //Called automatically by the Input System when movement input is received. It stores directional input for use in PureData.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //Called automatically by the Input System when Jump is recioeved. It applies velocity up if the player is grounded.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            //Equation that calculates velocity needed to reach certain heirgh
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Called once per frame to update movement and gravity
    void Update()
    {
        // Check if the player is grounded using a physics sphere at groundCheck position
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //add a small downward force to keep the player tied to the ground as it moves through topology
        }

        //moving in the direction set by FirstPersonLook
        Vector3 move = new Vector3(moveInput.x, 0.0f, moveInput.y);
        controller.Move(transform.TransformDirection(move) * moveSpeed * Time.deltaTime);

        // Apply gravity manually (no physics)
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (jumping/falling)
        controller.Move(velocity * Time.deltaTime);
    }
}