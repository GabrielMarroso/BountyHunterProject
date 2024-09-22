using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    public static MenuController instance;

    public enum Screens
    {
        None,
        Loading,
        Login,
        CreateAccount,
        RecoverAccount,
        Ranking,
        Shop,
        Lobby,
        SearchingOpponent,
        CharacterSelection
        
    }
    public Screens currentScreen;

    [Header("Screens")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject loginScreen;
    [SerializeField] GameObject createAccountScreen;
    [SerializeField] GameObject messageScreen;
    [SerializeField] GameObject recoverAccountScreen;
    [SerializeField] GameObject rankingScreen;
    [SerializeField] GameObject shopScreen;
    [SerializeField] GameObject lobbyScreen;
    [SerializeField] GameObject searchingOpponentScreen;
    [SerializeField] GameObject characterSelectionScreen;
    [SerializeField] TextMeshProUGUI messageTXT;

    [Header("Login Information")]
    [SerializeField] TMP_InputField inputUsernameOrEmailLogin;
    [SerializeField] TMP_InputField inputPasswordLogin;

    [Header("Create Account Information")]
    [SerializeField] TMP_InputField inputUsernameCreateAccount;
    [SerializeField] TMP_InputField inputEmailCreateAccount;
    [SerializeField] TMP_InputField inputPasswordCreateAccount;
    [SerializeField] TMP_InputField inputConfirmPasswordCreateAccount;

    [Header("Recover Account")]
    [SerializeField] TMP_InputField inputRecoverAccount;

    [Header("Shop")]
    [SerializeField] TextMeshProUGUI coinsTXT;


    [Header("Rankings")]
    [SerializeField] string rankingName;
    [SerializeField] TMP_InputField rankingValue;
    [SerializeField] TextMeshProUGUI rankingTxt;
    [SerializeField] TextMeshProUGUI rankingListTxt;

    [Header("Character Selecion")]
    [SerializeField] Button btnReady;
    [SerializeField] TextMeshProUGUI timeTXT;
    private float timeRemaining;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ShowScreen(Screens.Login);
            messageScreen.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(characterSelectionScreen.activeInHierarchy)
        {
            timeRemaining = Mathf.Max(timeRemaining - Time.deltaTime, 0);
            timeTXT.text = ((int)timeRemaining).ToString("00");
        }
    }

    public void ShowScreen(Screens _screen)
    {
        currentScreen = _screen;
        loadingScreen.SetActive(false);
        loginScreen.SetActive(false);
        createAccountScreen.SetActive(false);
        recoverAccountScreen.SetActive(false);
        rankingScreen.SetActive(false);
        shopScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        searchingOpponentScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);

        switch (currentScreen)
        {
            case Screens.None:
                break; 

            case Screens.Loading:
                loadingScreen.SetActive(true);
                break; 

            case Screens.Login:
                inputUsernameOrEmailLogin.text = "";
                inputPasswordLogin.text = "";
                inputUsernameCreateAccount.text = "";
                inputEmailCreateAccount.text = "";
                inputPasswordCreateAccount.text = "";
                inputConfirmPasswordCreateAccount.text = "";
                loginScreen.SetActive(true);
                break;

            case Screens.CreateAccount:
                createAccountScreen.SetActive(true);
                break;
            
            case Screens.RecoverAccount:
                recoverAccountScreen.SetActive(true);
                break;

            case Screens.Ranking:
                rankingScreen.SetActive(true);
                break;

            case Screens.Shop:
                shopScreen.SetActive(true);
                break;

            case Screens.Lobby:
                lobbyScreen.SetActive(true);
                break;

            case Screens.SearchingOpponent:
                searchingOpponentScreen.SetActive(true);
                break;

            case Screens.CharacterSelection:
                timeRemaining = NetworkManager.instance.timeToSelectHero;
                btnReady.interactable = false;
                characterSelectionScreen.SetActive(true);
                break;
        }
    }

    #region Login
    public void BtnLogin()
    {
        ShowScreen(Screens.Loading);

        string _usernameOrEmail = inputUsernameOrEmailLogin.text;
        string _password = inputPasswordLogin.text;

        if (string.IsNullOrEmpty(_usernameOrEmail) ||
            string.IsNullOrEmpty(_password))
        {
            ShowMessage("Please fill out all fields");
            ShowScreen(Screens.Login);
        }
        else if(_usernameOrEmail.Length < 3)
        {
            ShowMessage("Invalid user data");
            ShowScreen(Screens.Login);
        }
        else
        {
            PlayfabManager.instance.UserLogin(_usernameOrEmail, _password);
        }
    }

    public void BtnCreateAccount()
    {
        ShowScreen(Screens.Loading);

        string _username = inputUsernameCreateAccount.text;
        string _email = inputEmailCreateAccount.text;
        string _password = inputPasswordCreateAccount.text;
        string _confirmPassword = inputConfirmPasswordCreateAccount.text;

        if(string.IsNullOrEmpty(_username) || 
            string.IsNullOrEmpty(_email) ||
            string.IsNullOrEmpty(_password) || 
            string.IsNullOrEmpty(_confirmPassword) )
        {
            Debug.Log("Please fill out all fields");
            ShowMessage("Please fill out all fields");
            ShowScreen(Screens.CreateAccount);
        }
        else if(_username.Length < 3)
        {
            Debug.Log("Username must be at least 3 characters long");
            ShowMessage("Username must be at least 3 characters long");
            ShowScreen(Screens.CreateAccount);
        }
        else if(_password != _confirmPassword)
        {
            Debug.Log("Passwords do not match");
            ShowMessage("Passwords do not match");
            ShowScreen(Screens.CreateAccount);
        }
        else
        {
            PlayfabManager.instance.CreateAccount(_username,_email,_password);
        }
    }

    public void BtnBackToLogin()
    {
        ShowScreen(Screens.Login);
    }
    public void BtnGoToCreateAccount()
    {
        ShowScreen(Screens.CreateAccount);
    }

    public void BtnRecoverAccount()
    {
        PlayfabManager.instance.Recoverpassword(inputRecoverAccount.text);
    }
    public void BtnShowRecoverAccountScreen()
    {
        ShowScreen(Screens.RecoverAccount);
    }
    #endregion

    #region Others
    public void ShowMessage(string _message)
    {
        messageTXT.text = _message;
        messageScreen.SetActive(true);
    }
    public void BtnDeleteAccount()
    {
        PlayfabManager.instance.DeleteAccount();
    }
    public void StartGame()
    {
        //ShowScreen(Screens.Loading);
        //PlayfabManager.instance.GetLeaderboard(rankingName);
        
        ShowScreen(Screens.Lobby);
    }
    #endregion

    #region Ranking

    public void BtnRanking()
    {
        ShowScreen(Screens.Loading);
        PlayfabManager.instance.GetLeaderboard(rankingName);
        ShowScreen(Screens.Ranking);

    }
    public void UpdateRanking()
    {
        if (string.IsNullOrEmpty(rankingValue.text))
        {
            ShowMessage("Favor informar o valor a ser atualizado!");
            return;
        }
        ShowScreen(Screens.Loading);
        PlayfabManager.instance.UpdatePlayerScore(rankingName, int.Parse(rankingValue.text));
        
    }

    public void UpdatePlayerRanking(int _value)
    {
        rankingTxt.text = "Highscore: " + _value.ToString();
    }

    public void UpdateRankingList(string _list)
    {
        rankingListTxt.text = _list;
        ShowScreen(Screens.Ranking);
    }
    #endregion
    
    #region Shop
    public void BuyTwoHandedAxe()
    {
        PlayfabManager.instance.BuyItemToPlayer("WP001001", "GC", 0);
    }

    public void BuyBow()
    {
        PlayfabManager.instance.BuyItemToPlayer("WP001002", "GC",100);
    }

    public void BuyItem(int _price)
    {
        PlayfabManager.instance.BuyItemToPlayer("WP001002", "GC", _price);
    }

    public void UpdateCoins(int amount)
    {
        coinsTXT.text = amount.ToString("0000");
    }
    #endregion

    #region Lobby
    public void BtnPlay()
    {
        // ShowScreen(Screens.SearchingOpponent);
        lobbyScreen.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void BtnReturnToLobby()
    {
        ShowScreen(Screens.Lobby);
    }

    public void BtnQuitGame()
    {
        Application.Quit();
    }

    public void BtnOptions()
    {

    }



    #endregion

    #region CharacterSelection
    public void BtnErika()
    {
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            //Red
            NetworkManager.instance.SelectRedHero(NetworkManager.Heroes.Erika);
        }
        else
        {
            //Blue
            NetworkManager.instance.SelectBlueHero(NetworkManager.Heroes.Erika);
        }
        btnReady.interactable = true;
    }

    public void BtnMutant()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            //Red
            NetworkManager.instance.SelectRedHero(NetworkManager.Heroes.Mutant);
        }
        else
        {
            //Blue
            NetworkManager.instance.SelectBlueHero(NetworkManager.Heroes.Mutant);
        }
        btnReady.interactable = true;
    }

    public void BtnBrute()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            //Red
            NetworkManager.instance.SelectRedHero(NetworkManager.Heroes.Brute);
        }
        else
        {
            //Blue
            NetworkManager.instance.SelectBlueHero(NetworkManager.Heroes.Brute);
        }
        btnReady.interactable = true;
    }

    public void BtnReady()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            //Red
            NetworkManager.instance.PlayerRedReady();
        }
        else
        {
            //Blue
            NetworkManager.instance.PlayerBlueReady();
        }
    }
    #endregion
}
