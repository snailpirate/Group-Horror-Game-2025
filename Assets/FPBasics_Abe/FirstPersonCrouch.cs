using UnityEngine;
using UnityEngine.InputSystem;

// Script for crouching with smooth transition of camera and capsule height. Also prevents standing if there is a ground object on top
public class FirstPersonCrouch : MonoBehaviour
{
    private CharacterController controller;

    [HideInInspector]
    public bool isCrouching = false;// Shared flag that other scripts (e.g., movement) can access

    // Ceiling detection
    private bool canStand;// Determines if there's space above to stand
    private float ceilingDistance = 0.4f;// Radius of the ceiling check sphere
    public Transform ceilingCheck;// Empty GameObject at the top of the head
    public LayerMask ceilingMask;// Layer(s) considered solid for standing clearance

    // Height and camera references
    public float crouchingHeight = 1.0f;
    public Transform cameraRoot;
    public float transitionDuration = 0.2f;

    // Cached values for original standing configuration
    private float standingHeight;
    private Vector3 standingCenter;
    private float bottomOffset;// Helps maintain feet grounded when resizing the capsule

    private Vector3 standingCamera;
    private Vector3 crouchingCamera;

    // Transition variables
    private float startHeight;
    private float targetHeight;

    private Vector3 startCenter;
    private Vector3 targetCenter;

    private Vector3 startCameraPos;
    private Vector3 targetCameraPos;

    private float elapsedTime = 0f;

    // Called once at the beginning to cache values
    void Start()
    {
        controller = GetComponent<CharacterController>();

        //Store original dimensions of player capsule
        standingHeight = controller.height;
        standingCenter = controller.center;
        bottomOffset = standingCenter.y - (standingHeight * 0.5f);//Calculate offset from feet (used to adjust center correctly when resizing)

        standingCamera = cameraRoot.localPosition; // Store original camera position (typically head-level)

        // Calculate lowered camera position when crouched
        crouchingCamera = new Vector3(cameraRoot.localPosition.x,
            cameraRoot.localPosition.y - (standingHeight - crouchingHeight),
            cameraRoot.localPosition.z
            );

        // Initialize transition data
        startHeight = standingHeight;
        targetHeight = standingHeight;

        startCenter = standingCenter;
        targetCenter = standingCenter;

        startCameraPos = standingCamera;
        targetCameraPos = standingCamera;

    }

    // Triggered when crouch input is performed
    public void onCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Allow crouch anytime; only allow stand if there's no ceiling above
            if (!isCrouching || canStand)
            {
                isCrouching = !isCrouching; //Toggle crouch state
                elapsedTime = 0f;// Restart transition time

                // Cache current state to use as the starting point for interpolation
                startHeight = controller.height;
                startCenter = controller.center;
                startCameraPos = cameraRoot.localPosition;

                // Determine where we are going based on crouch state
                targetHeight = isCrouching ? crouchingHeight : standingHeight;
                targetCenter = new Vector3(
                    standingCenter.x,
                    bottomOffset + (targetHeight * 0.5f),
                    standingCenter.z
                );
                targetCameraPos = isCrouching ? crouchingCamera : standingCamera;
            }
        }
    }

    //Called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / transitionDuration);//Transition time in 0 to 1

        //Check if there is an object right above the player and set canStand to false if there is
        if (isCrouching)
        {
            // Returns true if space above is clear to stand
            canStand = !Physics.CheckSphere(ceilingCheck.position, ceilingDistance, ceilingMask);
        }

        // Interpolate height, center, and camera smoothly based on time
        controller.height = Mathf.Lerp(startHeight, targetHeight, t);
        controller.center = Vector3.Lerp(startCenter, targetCenter, t);
        cameraRoot.localPosition = Vector3.Lerp(startCameraPos, targetCameraPos, t);

    }
}