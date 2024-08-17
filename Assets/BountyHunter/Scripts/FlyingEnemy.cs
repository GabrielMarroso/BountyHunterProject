using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 5f; // Speed at which the enemy moves towards the player
    public int maxHealth = 30; // Maximum health of the enemy
    public int damage = 100; // Amount of damage dealt to the player on contact
    public float detectionRange = 50f; // Range within which the enemy detects the player
    public Transform player; // Reference to the player's transform
    private int currentHealth; // Current health of the enemy
    private bool isChasing = false; // Flag to start chasing the player

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ResetEnemy()
    {
        Destroy(gameObject);
        // Reset position, state, etc.
    }
    void Update()
    {
        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Start chasing the player if within detection range
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        // If chasing, move towards the player
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Calculate the direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the enemy to face the player
        transform.LookAt(player);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player (implement player's health script to apply this damage)
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Destroy the enemy on impact
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce the enemy's health by the damage amount
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);

        // Check if the enemy's health has reached zero or below
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Flying Skull destroyed!");
        // Optionally, play death animation or effects here

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
