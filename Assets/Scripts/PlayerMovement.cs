using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float gravity = -9.81f;

    [Header("Pulo")]
    public float jumpHeight = 1.2f;

    [Header("Agachar")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 10f;

    [Header("Stamina")]
    public float maxStamina = 2f;
    public float staminaDrainRate = 1f;
    public float staminaRegenRate = 1f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.15f;

    [Header("Referências")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float currentStamina;
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool staminaExhausted = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (cameraTransform == null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null)
                cameraTransform = cam.transform;
            else
                Debug.LogError("Nenhuma câmera encontrada como filha do Player!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleCrouch();
        HandleStamina();
        HandleMovement();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching) isRunning = false;
        }

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        float newHeight = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        float heightDifference = newHeight - controller.height;
        controller.height = newHeight;
        controller.center = new Vector3(0, newHeight / 2f, 0);

        if (heightDifference < 0)
            transform.position += new Vector3(0, heightDifference / 2f, 0);

        float targetCamY = isCrouching ? crouchHeight - 0.2f : standingHeight - 0.4f;
        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, crouchTransitionSpeed * Time.deltaTime);
        cameraTransform.localPosition = camPos;
    }

    void HandleStamina()
    {
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

        if (staminaExhausted && currentStamina >= maxStamina)
            staminaExhausted = false;

        if (wantsToRun && currentStamina > 0f && !staminaExhausted)
        {
            isRunning = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                staminaExhausted = true;
                isRunning = false;
            }
        }
        else
        {
            isRunning = false;
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed;
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = speed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Pulo — só pode pular se estiver no chão e não agachado
        if (controller.isGrounded)
        {
            velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && !isCrouching)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public float GetStaminaPercent() => currentStamina / maxStamina;
    public bool IsExhausted() => staminaExhausted;
}