using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; //singleton

    public enum Heroes
    {
        None,
        Erika,
        Brute,
        Mutant
    }


    [SerializeField] GameObject[] playerPrefabTeamRed; 
    [SerializeField] GameObject[] playerPrefabTeamBlue; 

    public Heroes playerRedHero;
    public Heroes playerBlueHero;

    public bool playerRedReady;
    public bool playerBlueReady;

    public float timeToSelectHero = 30f;
    public float timeToStartBattle;

    private bool gameStarted;
    private bool timeFinished;
    private bool gameOver;

    public Transform[] spawnPos;
    int createdPlayers;

    public string bronze = "Bronze0000";
    public string prata = "Prata0000";
    public string ouro = "Ouro0000";

    public void SelectRedHero(Heroes _hero)
    {
        switch (_hero)
        {
            case Heroes.Erika:
                photonView.RPC("SelectRedHeroLan",RpcTarget.All, 0);
                break;
            case Heroes.Brute:
                photonView.RPC("SelectRedHeroLan", RpcTarget.All, 1);
                break;
            case Heroes.Mutant:
                photonView.RPC("SelectRedHeroLan", RpcTarget.All, 2);
                break;
        }
    }
    [PunRPC]
    public void SelectRedHeroLan(int _hero)
    {
        switch (_hero)
        {
            case 0:
                playerRedHero = Heroes.Erika;
                break;

            case 1:
                playerRedHero = Heroes.Brute;
                break; 
            
            case 2:
                playerRedHero = Heroes.Mutant;
                break;
        }
    }

    public void SelectBlueHero(Heroes _hero)
    {
        switch (_hero)
        {
            case Heroes.Erika:
                photonView.RPC("SelectBlueHeroLan", RpcTarget.All, 0);
                break;
            case Heroes.Brute:
                photonView.RPC("SelectBlueHeroLan", RpcTarget.All, 1);
                break;
            case Heroes.Mutant:
                photonView.RPC("SelectBlueHeroLan", RpcTarget.All, 2);
                break;
        }
    }
    [PunRPC]
    public void SelectBlueHeroLan(int _hero)
    {
        switch (_hero)
        {
            case 0:
                playerBlueHero = Heroes.Erika;
                break;

            case 1:
                playerBlueHero = Heroes.Brute;
                break;

            case 2:
                playerBlueHero = Heroes.Mutant;
                break;
        }
    }


    [HideInInspector] public Transform cameraPlayer;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    private void Start()
    {
        Debug.Log("Start");
        ConnectToPhoton();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom.PlayerCount < 2 || gameOver || timeFinished) return;


        if (!gameStarted)
        {
            gameStarted = true;
            timeToStartBattle = Time.time + timeToSelectHero;
            StartCoroutine("Game");
        }
        else if( (Time.time > timeToStartBattle) && (!playerBlueReady || !playerRedReady))
        {
            timeFinished = true;
        }
    }

    public void ConnectToPhoton()
    {
        Debug.Log("ConnectToPhoton");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();

        /*
        int _rankAtual = 0;
        if (_rankAtual > 750)        
            PhotonNetwork.JoinRoom(ouro);        
        else if (_rankAtual > 500)
            PhotonNetwork.JoinRoom(prata);
        else
            PhotonNetwork.JoinRoom(bronze);
        */
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        /*
         * int _rankAtual = 0;
        if (_rankAtual > 750)
            PhotonNetwork.CreateRoom(ouro);
        else if (_rankAtual > 500)
            PhotonNetwork.CreateRoom(prata);
        else
            PhotonNetwork.CreateRoom(bronze);
        */
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log("Playercount: " + PhotonNetwork.CurrentRoom.PlayerCount);
        /*
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            return;


        int _rankAtual = 0;
        if (_rankAtual > 750)
            if (PhotonNetwork.CurrentRoom.Name == ouro)
            {
                CreateRoomNames(_rankAtual);
            }
        else if (_rankAtual > 500)
        {
            if (PhotonNetwork.CurrentRoom.Name == prata)
            {
                CreateRoomNames(_rankAtual);
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.Name == bronze)
            {
                CreateRoomNames(_rankAtual);
            }
        }*/
    }

    [PunRPC]
    public void StartGame()
    {
        StartCoroutine("Game");
    }
    IEnumerator Game()
    {
        photonView.RPC("SetStartTime", RpcTarget.All);
        yield return new WaitUntil(()=>(playerBlueReady && playerRedReady) || timeFinished);

        if (timeFinished)
        {
            if (playerRedReady)
            {
                //Red win!
                Debug.Log("Red Win");
            }
            else
            {
                //Red lose!
                Debug.Log("Red Lose");
            }

            if (playerBlueReady)
            {
                //Blue win!
                Debug.Log("Blue Win");
            }
            else
            {
                //Blue lose!
                Debug.Log("Blue Lose");
            }

        }
        else
        {
            //Create Avatars
            photonView.RPC("CreatePlayerAvatar", RpcTarget.All);
        }
    }


    [PunRPC]
    public void SetStartTime()
    {
        gameStarted = true;
        MenuController.instance.ShowScreen(MenuController.Screens.CharacterSelection);
    }

    [PunRPC]
    public void CreateRedPlayer()
    {
        //Crio o avatar na posicao 0
        switch (playerRedHero)
        {
            case Heroes.Erika:
                foreach (var prefab in playerPrefabTeamRed)
                {
                    if (prefab.name == "ErikaTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
            case Heroes.Brute:
                foreach (var prefab in playerPrefabTeamRed)
                {
                    if (prefab.name == "BruteTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
            case Heroes.Mutant:
                foreach (var prefab in playerPrefabTeamRed)
                {
                    if (prefab.name == "BruteTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
        }
    }

    [PunRPC]
    public void CreateBluePlayer()
    {
        //Crio o avatar na posicao 1
        switch (playerBlueHero)
        {
            case Heroes.Erika:
                foreach (var prefab in playerPrefabTeamBlue)
                {
                    if (prefab.name == "ErikaTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
            case Heroes.Brute:
                foreach (var prefab in playerPrefabTeamBlue)
                {
                    if (prefab.name == "BruteTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
            case Heroes.Mutant:
                foreach (var prefab in playerPrefabTeamBlue)
                {
                    if (prefab.name == "BruteTeamRed")
                    {
                        PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                    }
                }
                break;
        }
    }

    [PunRPC]
    void CreatePlayerAvatar()
    {
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {            
            //Crio o avatar na posicao 0
            switch (playerRedHero)
            {
                case Heroes.Erika:
                    foreach (var prefab in playerPrefabTeamRed)
                    {
                        if(prefab.name == "ErikaTeamRed")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                        }
                    }
                    break;
                case Heroes.Brute:
                    foreach (var prefab in playerPrefabTeamRed)
                    {
                        if (prefab.name == "BruteTeamRed")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                        }
                    }
                    break;
                case Heroes.Mutant:
                    foreach (var prefab in playerPrefabTeamRed)
                    {
                        if (prefab.name == "BruteTeamRed")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[0].position, spawnPos[0].rotation);
                        }
                    }
                    break;
            }
        }
        else
        {
            //Crio o avatar na posicao 1
            switch (playerBlueHero)
            {
                case Heroes.Erika:
                    foreach (var prefab in playerPrefabTeamBlue)
                    {
                        if (prefab.name == "ErikaTeamBlue")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[1].position, spawnPos[1].rotation);
                        }
                    }
                    break;
                case Heroes.Brute:
                    foreach (var prefab in playerPrefabTeamBlue)
                    {
                        if (prefab.name == "BruteTeamBlue")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[1].position, spawnPos[1].rotation);
                        }
                    }
                    break;
                case Heroes.Mutant:
                    foreach (var prefab in playerPrefabTeamBlue)
                    {
                        if (prefab.name == "BruteTeamBlue")
                        {
                            PhotonNetwork.Instantiate(prefab.name, spawnPos[1].position, spawnPos[1].rotation);
                        }
                    }
                    break;
            }
        }
        MenuController.instance.ShowScreen(MenuController.Screens.None);
    }


    public void PlayerBlueReady()
    {
        photonView.RPC("PlayerBlueReadyLan", RpcTarget.All);
    }
    [PunRPC]
    public void PlayerBlueReadyLan()
    {
        playerBlueReady = true;
    }

    public void PlayerRedReady()
    {
        photonView.RPC("PlayerRedReadyLan", RpcTarget.All);
    }
    [PunRPC]
    public void PlayerRedReadyLan()
    {
        playerRedReady = true;
    }



    public void CreateRoomNames(int _rankAtual)
    {
        if (_rankAtual > 750)
            ouro = "Ouro" + Random.Range(0, 9999).ToString("0000");
        else if (_rankAtual > 500)
            prata = "Prata" + Random.Range(0, 9999).ToString("0000");
        else
            bronze = "Bronze" + Random.Range(0, 9999).ToString("0000");
    }
}
