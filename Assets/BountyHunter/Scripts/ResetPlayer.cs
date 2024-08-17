using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public Transform initialPosition; // Assign the initial position in the Inspector
    private PlayerHealth playerHealth; // Reference to the player health script

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        if (initialPosition != null)
        {
            transform.position = initialPosition.position;
            transform.rotation = initialPosition.rotation;
        }

        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Reset other player attributes as needed
    }
}
