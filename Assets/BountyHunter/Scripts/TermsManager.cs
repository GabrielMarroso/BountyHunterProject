using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TermsManager : MonoBehaviour
{
    public void OpenTerms()
    {
        Application.OpenURL("https://cottony-random-527.notion.site/Terms-of-Service-and-Conditions-of-Use-1cce9c050b14406a8f07b8b45cbba30f");
    }

    public void AcceptTerms()
    {
        SceneManager.LoadScene("Menu");
    }

    public void DeclineTerms()
    {
        Application.Quit();
    }
}
