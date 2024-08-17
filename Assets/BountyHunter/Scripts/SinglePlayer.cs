using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
    //Player Controller
    public float speed = 6.0f; // Movement speed
    public float lookSensitivity = 2.0f; // Mouse look sensitivity
    public float maxLookX = 45.0f; // Maximum look up angle
    public float minLookX = -45.0f; // Maximum look down angle
    public Camera playerCamera; // Reference to the player's camera
    private Rigidbody rb; // Reference to the Rigidbody component

    private float rotX; // Rotation around the X-axis

    //Porjectile Shooting
    public GameObject projectilePrefab; // Prefab of the projectile to be fired
    public Transform shootPoint; // Point from where the projectile will be fired
    public float projectileSpeed = 20f; // Speed of the projectile
    public float fireRate = 1f; // Rate of fire (shots per second)

    private float nextFireTime; // Time when the player can shoot next


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {

            Shoot();

    }

    private void FixedUpdate()
    {

            Move();
            CameraLook();

    }

    void Move() //Player movement
    {
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        movement.y = rb.velocity.y; // Keep the existing vertical velocity (for jumping or falling)
        rb.velocity = movement;


    }

    void CameraLook() //Player Look
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        rb.MoveRotation(transform.rotation * Quaternion.AngleAxis(mouseX, Vector3.up));
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootProjectile();
            nextFireTime = Time.time + 1f / fireRate; // Calculate next fire time based on fire rate
        }
    }

    void ShootProjectile()
    {
        // Calculate the direction to shoot the projectile
        Vector3 shootDirection = playerCamera.transform.forward;

        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootDirection * projectileSpeed;

        // Optionally, add some logic here to handle projectile lifetime or other behaviors
    }
}