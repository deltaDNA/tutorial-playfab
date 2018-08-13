using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using DeltaDNA;

public class SdkManager : MonoBehaviour {

    // For simplicity this tutorial will require a network connection 
    // so it can connect to PlayFab

    public string sharedUserID;
    public GameObject panelQuit;

    // Use this for initialization
    void Start()
    {

        // Setup some DeltaDNA config stuff
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

        // Log the player in to PlayFab
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
        // TODO - Add furher login methods for iOS / Android etc .. 

    }



    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Player Logged in to PlayFab");
        sharedUserID = result.PlayFabId;
        Debug.Log("Shared UserID =  " + sharedUserID);

        // Start deltaDNA SDK and use the PlayFab userID
        DDNA.Instance.StartSDK(sharedUserID);

        // Move to Main scene now that DDNA and PlayFab SDKs are running
        SceneManager.LoadScene("Main", LoadSceneMode.Single);        
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("This tutorial requires a network connection to connect to PlayFab for some key game logic");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        ShowQuitPanel();
    }

    private void ShowQuitPanel()
    {
        panelQuit.SetActive(true);
    }
}
