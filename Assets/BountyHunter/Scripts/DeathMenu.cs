using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("SinglePlayer");

    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");

    }
}
