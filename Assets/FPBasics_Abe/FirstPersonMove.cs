using UnityEngine;
using UnityEngine.InputSystem; // Necessary for direct keyboard check
using UnityEngine.UI;

public class FirstPersonMove : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("UI References")]
    public Image staminaBarFill; 

    [Header("Movement Settings")]
    public float walkSpeed = 10.0f; 
    public float sprintSpeed = 20.0f; 
    public float jumpHeight = 2.0f;
    
    [Header("DEBUG VIEW (Read Only)")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isSprintPressed; // We will set this manually now
    [SerializeField] private bool isExhausted;

    [Header("Gravity Settings")]
    public float gravity = -9.8f;
    private bool isGrounded;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float drainRate = 20f;
    public float regenRate = 10f;
    public float exhaustionThreshold = 25f; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina; 
        currentSpeed = walkSpeed;
    }

    // --- INPUT METHODS (Move and Jump still use the Input System) ---
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // WE NO LONGER NEED OnSprint. We will check it directly in Update.

    // --- MAIN LOOP ---
    void Update()
    {
        // 1. HARDWIRED INPUT CHECK
        // This asks the keyboard directly, bypassing the Event system wiring.
        if (Keyboard.current != null)
        {
            isSprintPressed = Keyboard.current.leftShiftKey.isPressed;
        }

        HandleGravity();
        HandleStaminaAndSpeed(); 
        HandleMovement();
        UpdateUI();
    }

    void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void HandleStaminaAndSpeed()
    {
        // 1. Check Exhaustion
        if (currentStamina <= 0) isExhausted = true;
        else if (currentStamina >= exhaustionThreshold) isExhausted = false;

        // 2. Logic Check
        // Note: We removed the "isMoving" check as requested earlier
        bool shouldSprint = isSprintPressed && !isExhausted;

        if (shouldSprint)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= drainRate * Time.deltaTime;
        }
        else
        {
            currentSpeed = walkSpeed;
            if (currentStamina < maxStamina)
            {
                currentStamina += regenRate * Time.deltaTime;
            }
        }
        
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0.0f, moveInput.y);
        controller.Move(transform.TransformDirection(move) * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateUI()
    {
        if (staminaBarFill != null)
        {
            staminaBarFill.fillAmount = currentStamina / maxStamina;
            staminaBarFill.color = isExhausted ? Color.red : Color.white; 
        }
    }
}