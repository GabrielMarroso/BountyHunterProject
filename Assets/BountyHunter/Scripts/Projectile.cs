using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the projectile is destroyed
    public float damage = 10; // Damage dealt by the projectile


    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime expires
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check if the projectile hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hitEnemy");

            // Get the FlyingSkull script and apply damage
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
           {
               enemy.TakeDamage(damage);
            }

            // Destroy the projectile on impact
            Destroy(gameObject);
        }
 
    }

}
