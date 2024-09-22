using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using static MenuController;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public string PlayFabID;
    
    //dados do playerData salvos localmente
    Dictionary<string, string> playerData = new Dictionary<string, string>();

    public bool apiFinished = false;
    public int goldCoins;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    #region Login (Not in use)
    void LoginWithCustomID()
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
        
        apiFinished = true;
    }

    private void GetAllPlayerDataFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
        apiFinished = true;
    }

    public void AddUsernamePassword()
    {
        var _request = new AddUsernamePasswordRequest()
        {
            Username = "ProfCleberECDD",
            Password = "@123456",
            Email = "pcecdd@infnet.edu.br"
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


    public void CreateAccount(string _username, string _email,string _password)
    {
        var request = new RegisterPlayFabUserRequest()
        {
            Username = _username,
            Email = _email,
            Password = _password
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, CreateAccountSucess, CreateAccountFail);
    }

    private void CreateAccountSucess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Conta criada com sucesso!");
        MenuController.instance.ShowScreen(MenuController.Screens.Login);
        MenuController.instance.ShowMessage("Conta criada com sucesso!");
    }

    private void CreateAccountFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
        MenuController.instance.ShowScreen(MenuController.Screens.CreateAccount);
        MenuController.instance.ShowMessage("Erro: " + error.ErrorMessage);
    }

    public void UserLogin(string _username, string _password)
    {
        var _request = new LoginWithPlayFabRequest()
        {
            Username = _username,
            Password = _password
        };
        PlayFabClientAPI.LoginWithPlayFab(
            _request, 
            UserLoginSucess,
            error=>
            {
                Debug.Log("Efetuando login com email!");
                var _requestEmail = new LoginWithEmailAddressRequest()
                {
                    Email = _username,
                    Password = _password
                };
                PlayFabClientAPI.LoginWithEmailAddress(_requestEmail, UserLoginSucess, UserLoginFail);
            });
    }

    private void UserLoginSucess(LoginResult result)
    {
        Debug.Log("Login Efetuado com sucesso!");
        //MenuController.instance.ShowMessage("Login Efetuado com sucesso!");
        //MenuController.instance.ShowScreen(MenuController.Screens.None);
        PlayFabID = result.PlayFabId;

        MenuController.instance.ShowScreen(Screens.Loading);

        apiFinished = false;

        StartCoroutine("LoadGame");
        


    }

    IEnumerator LoadGame()
    {
        apiFinished = false;
        GetPlayerInventory();
        yield return new WaitUntil(() => apiFinished);

        apiFinished = false;
        GetAllPlayerData();
        yield return new WaitUntil(() => apiFinished);

        MenuController.instance.StartGame();
    }
    private void UserLoginFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
        MenuController.instance.ShowMessage("Erro: " + error.ErrorMessage);
        MenuController.instance.ShowScreen(MenuController.Screens.Login);
    }


    public void Recoverpassword(string _email)
    {
        var _request = new SendAccountRecoveryEmailRequest()
        {
            Email = _email,
            TitleId = PlayFabSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(_request, RecoverpasswordSucess, RecoverpasswordFail);
    }

    private void RecoverpasswordSucess(SendAccountRecoveryEmailResult result)
    {
        MenuController.instance.ShowMessage("Solicitação de Recuperação de Conta efetuada com sucesso! " +
            "\nFavor verificar seu email.");
    }

    private void RecoverpasswordFail(PlayFabError error)
    {
        MenuController.instance.ShowMessage("Erro: " + error.ErrorMessage);        
    }

    public void DeleteAccount()
    {
        var request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "deletePlayerAccount",
            GeneratePlayStreamEvent = true
        };
        PlayFabClientAPI.ExecuteCloudScript(request, DeleteAccountSucess, DeleteAccountFail);
    }

    private void DeleteAccountSucess(ExecuteCloudScriptResult result)
    {
        MenuController.instance.ShowScreen(MenuController.Screens.Login);
        MenuController.instance.ShowMessage("Conta excluida com sucesso!");
    }

    private void DeleteAccountFail(PlayFabError error)
    {
        MenuController.instance.ShowMessage("Erro: " + error.ErrorMessage);
    }

    public void UpdatePlayerScore(string _statisticName, int _score)
    {
        var _request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = _statisticName,
                    Value = _score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(_request, 
            sucess=>
            {
                Debug.Log("Ranking atualizado com sucesso!");
                GetLeaderboard(_statisticName);
            },
            error=>
            {
                Debug.Log("Erro: " + error.ErrorMessage);
            });
    }


    public void GetLeaderboard(string _statisticName)
    {
        var _request = new GetLeaderboardRequest
        {
            StatisticName = _statisticName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(_request, 
            results=>
            {
                string _list = "";
                MenuController.instance.UpdatePlayerRanking(0);
                foreach (var result in results.Leaderboard)
                {

                    if(result.PlayFabId == PlayFabID)
                    {
                        Debug.Log("Jogador: " + PlayFabID + " - " + result.StatValue);
                        MenuController.instance.UpdatePlayerRanking(result.StatValue);
                    }
                    _list += (result.Position + 1).ToString() + ": " + result.StatValue.ToString() + "\n";
                    
                }
                MenuController.instance.UpdateRankingList(_list);
            },
            error=>
            {
                Debug.Log("Erro: " + error.ErrorMessage);
            });
    }


    public void AddVirtualCurrency(string idCoin, int amount)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = idCoin,
            Amount = amount
        };
        
        PlayFabClientAPI.AddUserVirtualCurrency(request,
            sucess => 
            {
                Debug.Log("Moedas adicionadas com sucesso!");
            }, 
            error => 
            {
                Debug.Log("Erro: " + error.ErrorMessage);
            });
    }

    public void BuyItemToPlayer(string itemId, string coin, int value)
    {
        var request = new PurchaseItemRequest
        {
            ItemId = itemId,
            VirtualCurrency = coin,
            Price = value
        };
        PlayFabClientAPI.PurchaseItem(request, 
            sucess=>
            {
                if (coin == "GC")
                {
                    goldCoins -= value;
                    MenuController.instance.UpdateCoins(goldCoins);
                }

                Debug.Log("Item comprado com sucesso!");
                foreach (var item in sucess.Items)
                {
                    Debug.Log("Item comprado: " + item.ItemId + " - " + item.DisplayName);
                }
            }, 
            BuyItemToPlayerFail);
    }

    /*private void BuyItemToPlayerSucess(PurchaseItemResult result)
    {
        Debug.Log("Item comprado com sucesso!");
        foreach (var item in result.Items)
        {
            Debug.Log("Item comprado: " + item.ItemId + " - " + item.DisplayName);
        }
    }*/

    private void BuyItemToPlayerFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
    }

    public void GetPlayerInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetPlayerInventorySucess, GetPlayerInventoryFail);
    }

    private void GetPlayerInventorySucess(GetUserInventoryResult result)
    {
        goldCoins = 0;

        foreach (var coin in result.VirtualCurrency)
        {
            Debug.Log("Moeda: " + coin.Key + " - Quantidade: " + coin.Value);
            
            if(coin.Key == "GC") goldCoins = coin.Value;
        }

        foreach (var item in result.Inventory)
        {
            Debug.Log("ItemID: " + item.ItemId + " - " + item.DisplayName + " - ItemInstanceId: " + item.ItemInstanceId);
        }

        MenuController.instance.UpdateCoins(goldCoins);
        apiFinished = true;

    }

    private void GetPlayerInventoryFail(PlayFabError error)
    {
        Debug.Log("Erro: " + error.ErrorMessage);
        apiFinished = true;
    }

    
}
