using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float crouchSpeed = 2.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float mouseSensitivity = 2.0f;
    public float crouchHeight = 0.5f;
    public float bobFrequency = 1.5f;
    public float bobHeight = 0.1f;
    public AudioClip[] footstepSounds;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Camera playerCamera;
    private AudioSource audioSource;
    private float verticalRotation = 0.0f;
    private float verticalVelocity = 0.0f;
    private float footstepInterval = 0.5f;
    private float footstepTimer = 0;
    private float originalHeight;
    private float bobTimer = 0;
    private float originalSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalHeight = controller.height;
        originalSpeed = walkSpeed;
    }

    void Update()
    {
        // Mouse look
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


        // Movement
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            // Sprint logic
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= sprintSpeed;
            }
            else
            {
                moveDirection *= walkSpeed;
            }

            // Jump logic
            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }

        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        // Footstep sounds
        if (controller.isGrounded && controller.velocity.magnitude > 2f && footstepTimer <= 0)
        {
            PlayFootstepSound();
            footstepTimer = footstepInterval;
        }
        footstepTimer -= Time.deltaTime;

        // Head bobbing
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * controller.velocity.magnitude * bobFrequency;
            playerCamera.transform.localPosition = new Vector3(0, Mathf.Sin(bobTimer) * bobHeight, 0);
        }
        else
        {
            bobTimer = 0;
            playerCamera.transform.localPosition = Vector3.zero;
        }

        // Crouch mechanic
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 10);
            walkSpeed = crouchSpeed;
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, originalHeight, Time.deltaTime * 10);
            walkSpeed = originalSpeed;
        }
    }

    void PlayFootstepSound()
    {
        int index = Random.Range(0, footstepSounds.Length);
        audioSource.PlayOneShot(footstepSounds[index]);
    }
}