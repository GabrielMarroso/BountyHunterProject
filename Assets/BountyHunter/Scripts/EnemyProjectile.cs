using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the projectile is destroyed
    public float damage = 10; // Damage dealt by the projectile


    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime expires
    }

    void OnTriggerEnter(Collider collision)
    {
       
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hitPlayer");

            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.PlayerTakeDamage(damage);
            }


        }
    }

}
