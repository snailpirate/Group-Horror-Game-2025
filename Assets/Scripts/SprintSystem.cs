using UnityEngine;
using UnityEngine.UI;

public class SprintSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float drainRate = 20f;
    public float regenRate = 10f;

    [Header("Speed Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    
    // This variable determines how fast the player moves
    public float currentSpeed; 

    // We change this variable from the OTHER script
    [HideInInspector] public bool isSprintButtonPressed;

    [Header("Exhaustion Logic")]
    public bool isExhausted = false;
    public float exhaustionThreshold = 25f; 

    [Header("References")]
    public Image staminaBarFill;

    private void Start()
    {
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // 1. Check Exhaustion
        if (currentStamina <= 0) isExhausted = true;
        else if (currentStamina >= exhaustionThreshold) isExhausted = false;

        // 2. Determine if we are actually sprinting
        // We sprint if the button is held AND we are not exhausted
        bool isSprinting = isSprintButtonPressed && !isExhausted;

        if (isSprinting)
        {
            // Drain Stamina & Set Sprint Speed
            currentStamina -= drainRate * Time.deltaTime;
            currentSpeed = sprintSpeed;
        }
        else
        {
            // Regen Stamina & Set Walk Speed
            if (currentStamina < maxStamina)
                currentStamina += regenRate * Time.deltaTime;
            
            currentSpeed = walkSpeed;
        }

        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaBarFill.fillAmount = currentStamina / maxStamina;
        staminaBarFill.color = isExhausted ? Color.red : Color.green;
    }
}