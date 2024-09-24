using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using PlayFab;


public class Player : MonoBehaviour
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

    //Photon
    PhotonView view;

    public GameObject cameraGO;
    public GameObject playerMesh;
    public Animator anim;

    public float health = 100;
    public bool gameOver = false;
    public int score = 0;

    //Playfab
    [SerializeField] string rankingName;


    void Start()
    {
        anim = GetComponent<Animator>();
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            if (playerMesh != null)
            playerMesh.SetActive(false);
        }
        else
        {
            if(cameraGO != null)
            cameraGO.SetActive(false);
        }

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
       Cursor.visible = false; // Hide the cursor


    }

    void Update()
    {
        if (view.IsMine)
        {
            Shoot();
            CameraLook();
            Move();

        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine) 
        {
            //Move();
            //CameraLook();
        }
    }

    void Move() //Player movement
    {
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        movement.y = rb.velocity.y; // Keep the existing vertical velocity (for jumping or falling)
        rb.velocity = movement;

        anim.SetBool("Walking", moveX > 0 | moveZ>0);

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
        // Get the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Calculate the direction from the shoot point to where the ray hits
        Vector3 shootDirection = ray.direction;

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootDirection * projectileSpeed;

        // Optionally, add some logic here to handle projectile lifetime or other behaviors
    }

    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {     
            gameOver = true;
            Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
            Cursor.visible = true; // Hide the cursor
            GameOver();
        }
    }

    public void GameOver()
    {
        if (gameOver)
        {
            PlayfabManager.instance.UpdatePlayerScore(rankingName, score);

            SceneManager.LoadScene(0);

        }
    }

    public void addScore(int scorePoints)
    {
        score += scorePoints;
        Debug.Log(score);
    }
}
