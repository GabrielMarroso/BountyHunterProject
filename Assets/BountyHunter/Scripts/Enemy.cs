using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerPos; // Reference to the player's transform
    public GameObject projectilePrefab; // Prefab of the projectile to be fired
    public Transform shootPoint; // Point from where the projectile will be fired
    public float projectileSpeed = 10f; // Speed of the projectile
    public float fireRate = 2f; // Rate of fire (shots per second)

    public float enemyHealth = 30;

    public float moveSpeed = 3f; // Speed of random movement
    public float moveRange = 5f; // Range of random movement
    public float changeDirectionInterval = 2f; // Time interval to change direction

    private Rigidbody rb; // Reference to the Rigidbody component
    private float nextFireTime; // Time when the enemy can shoot next
    private Vector3 targetPosition; // The new target position for random movement
    private float nextChangeDirectionTime; // Time when the enemy will change direction

    void Start()
    {
        // Find the player by tag
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Handle shooting
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate; // Calculate next fire time based on fire rate
        }

        // Handle random movement
        if (Time.time >= nextChangeDirectionTime)
        {
            SetRandomTargetPosition();
            nextChangeDirectionTime = Time.time + changeDirectionInterval;
        }
    }

    void FixedUpdate()
    {
        MoveToTargetPosition();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
            player.addScore(1);  
            Destroy(gameObject);

        }
    }
    
    void Shoot()
    {
    /*
        // Calculate the direction to shoot the projectile towards the player
        Vector3 shootDirection = (playerPos.position - shootPoint.position).normalized;

        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootDirection * projectileSpeed;
    */
    }


    void SetRandomTargetPosition()
    {
        // Set a new random target position within the move range
        targetPosition = new Vector3(
            Random.Range(transform.position.x - moveRange, transform.position.x + moveRange),
            transform.position.y, // Keep the same y position
            Random.Range(transform.position.z - moveRange, transform.position.z + moveRange)
        );
    }

    void MoveToTargetPosition()
    {
        // Calculate the direction to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the new position to move towards
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;

        // Move the Rigidbody to the new position
        rb.MovePosition(newPosition);
    }

}
