using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // REQUIRED: Added to detect scene changes

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

    public float currentSpeed;
    [HideInInspector] public bool isSprintButtonPressed;

    [Header("Exhaustion Logic")]
    public bool isExhausted = false;
    public float exhaustionThreshold = 25f;

    [Header("References")]
    // We don't drag this in anymore. The script finds it.
    public Image staminaBarFill;

    // ---------------------------------------------------------
    // NEW CODE: Scene Management to fix the "Broken Bar"
    // ---------------------------------------------------------
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This runs every time a new room loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindStaminaUI();
    }
    // ---------------------------------------------------------

    private void Start()
    {
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
        FindStaminaUI(); // Find it immediately when game starts
    }

    // This looks for the Image object by Tag
    void FindStaminaUI()
    {
        // IMPORTANT: You must tag your Green Fill Image as "StaminaFill"
        GameObject uiObj = GameObject.FindGameObjectWithTag("StaminaFill");

        if (uiObj != null)
        {
            staminaBarFill = uiObj.GetComponent<Image>();
        }
    }

    private void Update()
    {
        // 1. Check Exhaustion
        if (currentStamina <= 0) isExhausted = true;
        else if (currentStamina >= exhaustionThreshold) isExhausted = false;

        // 2. Determine if we are actually sprinting
        bool isSprinting = isSprintButtonPressed && !isExhausted;

        if (isSprinting)
        {
            currentStamina -= drainRate * Time.deltaTime;
            currentSpeed = sprintSpeed;
        }
        else
        {
            if (currentStamina < maxStamina)
                currentStamina += regenRate * Time.deltaTime;

            currentSpeed = walkSpeed;
        }

        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        // Safety Check: If the UI hasn't been found yet, stop here so we don't get errors
        if (staminaBarFill == null) return;

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaBarFill.fillAmount = currentStamina / maxStamina;
        staminaBarFill.color = isExhausted ? Color.red : Color.green;
    }
}