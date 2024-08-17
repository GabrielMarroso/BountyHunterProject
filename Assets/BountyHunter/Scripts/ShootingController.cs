using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 20f;
    //public ParticleSystem muzzleFlash;
    public AudioClip shootingSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Apply force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = projectileSpawnPoint.forward * projectileSpeed;

        // Play muzzle flash
        //muzzleFlash.Play();

        // Play shooting sound
        audioSource.PlayOneShot(shootingSound);
    }
}
