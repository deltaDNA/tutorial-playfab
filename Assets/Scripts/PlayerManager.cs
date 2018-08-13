using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using DeltaDNA;

public class PlayerManager : MonoBehaviour {

    public int playerLevel = 0 ;
    public int playerHealth = 0 ;
    public int playerCoins = 0 ; 

	// Use this for initialization
	void Start () {
        GetPlayerInventory();   // Contains Virtual Currency Balance
        GetPlayerStatistics();  // Contains Numeric Stats (playerLevel, playerHealth..)
    }

    public void LevelUp()
    {
        playerLevel++;
        UpdatePlayerStatistics();
        Debug.Log("Level Up - playerLevel " + playerLevel);

        // Record a deltaDNA Analytics Event
        GameEvent levelup = new GameEvent("levelUp")
            .AddParam("levelUpName", "Level " + playerLevel)
            .AddParam("userLevel", playerLevel);

        DDNA.Instance.RecordEvent(levelup);
    }



    private void GetPlayerInventory()
    {
        // Get User Inventory from PlayFab on launch
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => {
                if (result.VirtualCurrency != null && result.VirtualCurrency.ContainsKey("GC"))
                {
                    playerCoins = result.VirtualCurrency["GC"];
                    Debug.Log("Retrieved from Playfab : Gold Coins " + playerCoins);
                }
            },
            error => {
                Debug.Log("Got error getting UserInventory:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }



    private void GetPlayerStatistics()
    {
        // Get User Statistics
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
                                break;
                            case "playerHealth":
                                playerHealth = eachStat.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            },
            error => {
                Debug.Log("Got error getting Player Statistics:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }



    private void UpdatePlayerStatistics()
    { 
        // Update PlayFab Statistics
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "playerLevel", Value = playerLevel },
                new StatisticUpdate { StatisticName = "playerHealth", Value = playerHealth },
            }
        },       
        result => {
            Debug.Log("Updated Player Statistics");
        },
        error => {
            Debug.Log("Got error updating Player Statistics:");
            Debug.Log(error.GenerateErrorReport());
         });
    }
}
