using UnityEngine;

public class WorldReset : MonoBehaviour
{
    public GameObject[] enemies; // Assign enemies in the Inspector
    public GameObject[] collectibles; // Assign collectibles in the Inspector

    void Start()
    {
        // Find and store all enemies in the array at the start
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }


    public void ResetWorld()
    {
        // Ensure the enemies array is populated
        if (enemies.Length == 0)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        // Reset each enemy
        foreach (GameObject FlyingEnemy in enemies)
        {
            // Ensure the enemy has an Enemy component and call ResetEnemy
            FlyingEnemy enemyScript = FlyingEnemy.GetComponent<FlyingEnemy>();
            if (enemyScript != null)
            {
                enemyScript.ResetEnemy();
            }
        }
    }
}
