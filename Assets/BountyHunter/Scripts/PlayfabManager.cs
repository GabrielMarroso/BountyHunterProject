using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public string PlayFabID;
    
    //dados do playerData salvos localmente
    Dictionary<string, string> playerData = new Dictionary<string, string>(); 

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Login();
    }

    #region Login
    void Login()
    {
        var requestLogin = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(requestLogin, OnLoginSucess, OnLoginFail);
    }

    private void OnLoginFail(PlayFabError error)
    {
        Debug.Log("Error: " + error.ErrorMessage);
    }

    private void OnLoginSucess(LoginResult result)
    {
        Debug.Log("Login efetuado com sucesso!");
        Debug.Log("PlayfabID: " + result.PlayFabId);

        PlayFabID = result.PlayFabId;

        PlayerInfo _playerInfo = new PlayerInfo("Ocinzento", 1, 0);
        string _playerData = JsonUtility.ToJson(_playerInfo);

        AddUsernamePassword();


        //GetAllPlayerData();

        //GetUserDataWithJson(PlayFabID, "PlayerInfo");

        //SetUserDataWithJson("PlayerInfo", _playerData);

        //ClientGetTitleData();
        //GetUserData(PlayFabID);
    }

    #endregion


    public void ClientGetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if(result.Data == null || !result.Data.ContainsKey("MensagemInicial"))
                {
                    Debug.Log("Sem mensagem inicial");
                }
                else
                {
                    Debug.Log("Mensagem inicial: " + result.Data["MensagemInicial"]);
                }
            },
            error =>
            {
                Debug.Log("Erro: " + error.ErrorMessage);
            }
        );
    }


    void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
                {"XP","500" },
                {"Level","2" }
            }
        },
            result => {
                Debug.Log("Dados atualizados com sucesso!");
            },
            error => {
                Debug.Log("Error: " + error.ErrorMessage);
            }
        );
    }

    void GetUserData(string myPlayFabID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabID,
            Keys = null
        },
            result =>
            {
                if (result == null || !result.Data.ContainsKey("XP"))
                    Debug.Log("SEM CHAVE XP SALVA ou SEM VALOR!");
                else
                {
                    Debug.Log("XP: " + result.Data["XP"].Value);
                    Debug.Log("Level: " + result.Data["Level"].Value);
                }
            },
            error =>
            {
                Debug.Log("Erro: " + error.ErrorMessage);

                
            }
        );
    }

    void SetUserDataWithJson(string _id, string _data)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {_id, _data}
            }
        },
        result =>
        {
            Debug.Log("Dados atualizados com sucesso!");
        },
        error=>
        {
            Debug.Log("Erro: " + error.ErrorMessage);
        });
    }

    void GetUserDataWithJson(string _myPlayfabID, string _id)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _myPlayfabID,
            Keys = null
        },
        result=>
        {
            if(result.Data == null || !result.Data.ContainsKey(_id))
            {
                Debug.Log("Dados nao encontrados!");
            }
            else
            {
                PlayerInfo _playerInfo = JsonUtility.FromJson<PlayerInfo>(result.Data[_id].Value);
                Debug.Log(_playerInfo.nickName);
                Debug.Log(_playerInfo.level);
                Debug.Log(_playerInfo.currentXP);
            }
        },
        error=>
        {
            Debug.Log("Erro: " + error.ErrorMessage);
        });
    }

    void GetAllPlayerData()
    {
        var _request = new GetUserDataRequest()
        {
            PlayFabId = PlayFabID,
            Keys = null
        };
        PlayFabClientAPI.GetUserData(_request, GetAllPlayerDataSucess, GetAllPlayerDataFail);
    }

    private void GetAllPlayerDataSucess(GetUserDataResult result)
    {
        foreach (var item in result.Data)
        {
            playerData.Add(item.Key, item.Value.Value);
        }
        Debug.Log("Dados do PlayerData recuperados com sucesso");
        Debug.Log(playerData.Count);
        Debug.Log(playerData["XP"]);
    }

    private void GetAllPlayerDataFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
    }

    public void AddUsernamePassword()
    {
        var _request = new AddUsernamePasswordRequest()
        {
            Username = "GabrielMarroso",
            Password = "senha123",
            Email = "gabriel.marroso@infnet.edu.br"
        };
        PlayFabClientAPI.AddUsernamePassword(_request, AddUsernamePasswordSucess, AddUsernamePasswordFail);

    }

    private void AddUsernamePasswordSucess(AddUsernamePasswordResult result)
    {
        Debug.Log(result.Username + " - Dados salvos com sucesso!");
    }

    private void AddUsernamePasswordFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
    }
}
