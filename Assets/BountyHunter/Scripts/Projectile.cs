using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the projectile is destroyed
    public int damage = 10; // Damage dealt by the projectile


    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime expires
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the FlyingSkull script and apply damage
            FlyingEnemy enemy = collision.gameObject.GetComponent<FlyingEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the projectile on impact
            Destroy(gameObject);
        }
    }

}
