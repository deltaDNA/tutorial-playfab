using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using DeltaDNA;

public class PlayerManager : MonoBehaviour
{

    // Health, Level & Coins are set in PlayFab
    public int playerLevel = 1;
    public int playerHealth = 100;
    public int playerCoins = 100;

    public int foodRemaining = 0;
    public HudManager hud;

    public enum State { DEAD, ALIVE };
    public State state = State.DEAD;

    // Use this for initialization
    void Start()
    {

        GetPlayerInventory();   // Contains Virtual Currency Balance
        GetPlayerStatistics();  // Contains Numeric Stats (playerLevel, playerHealth..)

        //DdnaPlayerConfig();     // Run deltaDNA playerConfig AB Test
    }


    public void StartLevel(int levelNo)
    {
        // Player starts level

        // Record Mission Started 

        // Spawn new Snake 
        //Vector3 pos = new Vector3(0, 0, -1);
        //Instantiate(snake, pos, Quaternion.identity);
    }

    public void LevelUp()
    {
        playerLevel++;

        Debug.Log("Level Up - playerLevel " + playerLevel);

        // Record a deltaDNA Analytics Event
        GameEvent levelup = new GameEvent("levelUp")
            .AddParam("levelUpName", "Level " + playerLevel)
            .AddParam("userLevel", playerLevel)
            .AddParam("foodRemaining", foodRemaining);

        DDNA.Instance.RecordEvent(levelup)
            .Add(new GameParametersHandler(gameParameters =>
            {
                Debug.Log("Event Triggered Campaign Hit");
                UpdatePlayerParameters(gameParameters);
            }))
            .Run();

        UpdatePlayerStatistics();
    }



    private void GetPlayerInventory()
    {
        // Get User Inventory from PlayFab on launch
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result =>
            {
                if (result.VirtualCurrency != null && result.VirtualCurrency.ContainsKey("GC"))
                {
                    playerCoins = result.VirtualCurrency["GC"];
                    Debug.Log("Retrieved from Playfab : Gold Coins " + playerCoins);
                    hud.SetCoins(playerCoins);
                }
            },
            error =>
            {
                Debug.Log("Got error getting UserInventory:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }



    private void GetPlayerStatistics()
    {
        // Get User Statistics from PlayFab
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            result =>
            {
                if (result.Statistics != null)// && result.Statistics..Contains("playerLevel"))
                {
                    foreach (var eachStat in result.Statistics)
                    {
                        Debug.Log("Retrieved fromPlayFab :  " + eachStat.StatisticName + " : " + eachStat.Value);

                        switch (eachStat.StatisticName)
                        {
                            case "playerLevel":
                                playerLevel = eachStat.Value;
                                hud.SetLevel(playerLevel);
                                break;
                            case "playerHealth":
                                playerHealth = eachStat.Value;
                                hud.SetHealth(playerHealth);
                                break;
                            case "difficulty":
                                foodRemaining = eachStat.Value;
                                hud.SetFoodRemaining(foodRemaining);
                                break;
                            default:
                                break;
                        }
                    }
                }
            },
            error =>
            {
                Debug.Log("Got error getting Player Statistics:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }



    public void UpdatePlayerStatistics()
    {
        // Update PlayFab Statistics
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "playerLevel", Value = playerLevel },
                new StatisticUpdate { StatisticName = "playerHealth", Value = playerHealth },
                new StatisticUpdate { StatisticName = "foodRemaining", Value = foodRemaining },
            }
        },
        result =>
        {
            Debug.Log("Updated Player Statistics on PlayFab");
        },
        error =>
        {
            Debug.Log("Got error updating Player Statistics:");
            Debug.Log(error.GenerateErrorReport());
        });

        hud.SetCoins(playerCoins);
        hud.SetHealth(playerHealth);
        hud.SetLevel(playerLevel);
        hud.SetFoodRemaining(foodRemaining);
    }

    private void UpdatePlayerParameters(Dictionary<string, object> gameParameters)
    {
        // React to any Player parameter modificaitons coming from deltaDNA
        Debug.Log("Received game parameters modifications from DDNA: " + DeltaDNA.MiniJSON.Json.Serialize(gameParameters));
        bool modified = false;
        if (gameParameters.ContainsKey("foodRemaining"))
        {
            foodRemaining = System.Convert.ToInt32(gameParameters["foodRemaining"]);
            modified = true;
        }

        if (modified)
        {

        }


    }

    public void SetLevel(int l)
    {/*
        chilliConnect.Economy.SetCurrencyBalance(new SetCurrencyBalanceRequestDesc("USERLEVEL", l)
            , (request, response) => Debug.Log("Set UserLevel " + l)
            , (request, error) => Debug.LogError(error.ErrorDescription)
        );
        */
        playerLevel = l;
        UpdatePlayerStatistics();
    }

    public void SetHealth(int h)
    {
        // TODO Save to cloud
        playerHealth = h;
        UpdatePlayerStatistics();

    }

    public void SpendCoins(int c)
    {
        SetCoins(playerCoins - c);
    }

    public void AddCoins(int c)
    {
        SetCoins(playerCoins + c);
    }

    private void SetCoins(int c)
    {/*
        chilliConnect.Economy.SetCurrencyBalance(new SetCurrencyBalanceRequestDesc("COINS", c)
            , (request, response) => Debug.Log("Set UserCoins " + c)
            , (request, error) => Debug.LogError(error.ErrorDescription)
        );*/
        playerCoins = c;
        UpdatePlayerStatistics();

    }

    public void SetFoodRemaining(int f)
    {
        foodRemaining = f;
        UpdatePlayerStatistics();
    }


    public void Kill()
    {
        this.state = State.DEAD;
    }
    public void NewPlayer()
    {
        this.state = State.ALIVE;


    }
}