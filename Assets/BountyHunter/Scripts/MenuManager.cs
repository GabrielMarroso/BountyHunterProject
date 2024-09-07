using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void Options()
    {
        Debug.Log("Show Options");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
