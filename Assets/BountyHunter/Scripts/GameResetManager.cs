using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    public PlayerReset playerReset;
    public WorldReset worldReset;

    public void ResetGame()
    {
        if (playerReset != null)
        {
            playerReset.ResetPlayer();
        }

        if (worldReset != null)
        {
            worldReset.ResetWorld();
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown("r"))
        {
            ResetGame(); 
        }
    }

}
