using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManagerBounty : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;


    private void Start()
    {
        ConnectToPhoton();
        Debug.Log("Start");
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("ConnectToPhoton");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null);
        Debug.Log("OnJoinRandomFailed");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log("Playercount: " + PhotonNetwork.CurrentRoom.PlayerCount);
        // photonView.RPC("CreatePlayerAvatar", PhotonNetwork.LocalPlayer);
        //photonView, chamado remoto

        CreatePlayerAvatar();
        CreateEnemies();
    }

   // [PunRPC]
    //pode chamar o metodo de forma remota, chamar no computador alheio ou só de voce ou de uma pessoa especifica. Parecido com serializedfield, se põe na frente
    void CreatePlayerAvatar()
    {

        Vector3 _pos = new Vector3(Random.Range(-3f, 3f), 2f, Random.Range(-10f, 10f));
        //posições aleatorias

        //Instantiate
        //instantiate normal é local
        PhotonNetwork.Instantiate(playerPrefab.name, _pos, Quaternion.identity);
        //quaternion controla a rotação, 4 valores
        Debug.Log("Player instanciated");
    }

    void CreateEnemies()
    {
        Vector3 _pos = new Vector3(Random.Range(-3f, 3f), 2f, Random.Range(-10f, 10f));
        PhotonNetwork.Instantiate(enemyPrefab.name, _pos, Quaternion.identity);
    }

}
