using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the projectile is destroyed

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime expires
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optionally, add logic here for what happens when the projectile hits something
        // For example: deal damage to enemy, play impact effects, etc.
        Destroy(gameObject); // Destroy the projectile on impact
    }
}
