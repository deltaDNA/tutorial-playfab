using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using DeltaDNA;

// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// https://api.playfab.com/docs/tutorials/landing-players/sign-in-with-google 
// https://github.com/playgameservices/play-games-plugin-for-unity
// https://developers.google.com/games/services/console/enabling

public class SdkManager : MonoBehaviour {

    // For simplicity this tutorial will require a network connection 
    // so it can connect to PlayFab

    private string sharedUserID;
    public GameObject panelQuit;
    public Button bttnStart;
    private bool readyToStart = false;

    // Start Button Size and Color
    private Color sourceColor;
    private Color targetColor;
    private Vector3 InitialScale;
    private Vector3 FinalScale;

    // Use this for initialization
    void Start()
    {
        // These are for pulsing the start button size and alpha 
        InitialScale = transform.localScale;
        FinalScale = new Vector3(InitialScale.x + 0.04f,
                                 InitialScale.y + 0.04f,
                                 InitialScale.z);
        sourceColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        targetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);


        // Setup some DeltaDNA config stuff
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

        // Log the player in to PlayFab
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            var request = new LoginWithCustomIDRequest { CustomId = "DDNA Tutorial Playfab", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                PlayFabClientAPI.LoginWithAndroidDeviceID( new LoginWithAndroidDeviceIDRequest()  {
                    CreateAccount = true,
                    AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                    TitleId = PlayFabSettings.TitleId,
                    AndroidDevice = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem
                }, OnLoginSuccess , OnLoginFailure);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
                {
                    CreateAccount = true,
                    DeviceId = SystemInfo.deviceUniqueIdentifier,
                    TitleId = PlayFabSettings.TitleId,
                    DeviceModel = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem
                }, OnLoginSuccess, OnLoginFailure);
            }
        }          
    }

    private void Update()
    {
        if (readyToStart)
        {
            // Pulse the start button size and alpha
            bttnStart.image.color = Color.Lerp(sourceColor, targetColor, Mathf.PingPong(Time.time, 1.2f));
            bttnStart.transform.localScale = Vector3.Lerp(InitialScale, FinalScale, Mathf.PingPong(Time.time, 1.2f));

        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Player Logged in to PlayFab");
        sharedUserID = result.PlayFabId;
        Debug.Log("Shared UserID =  " + sharedUserID);

        // Start deltaDNA SDK and use the PlayFab userID
        DDNA.Instance.StartSDK(sharedUserID);

        // Run deltaDNA playerConfig AB Test
        DdnaPlayerConfig();       
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

    private void DdnaPlayerConfig()
    {
        // Execute Cloud Script to get deltaDNA AB Test result   
        // containing any parameter modifiers
        ExecuteCloudScriptRequest myRequest = new ExecuteCloudScriptRequest();
        myRequest.FunctionName = "makeDdnaEngageRequest";
        myRequest.FunctionParameter = new
        {
            decisionPoint = "playerConfig",
            platform = DDNA.Instance.Platform,
            engageURL = DDNA.Instance.EngageURL,
            environmentKey = DDNA.Instance.EnvironmentKey
        };
        myRequest.GeneratePlayStreamEvent = true;

        PlayFabClientAPI.ExecuteCloudScript(myRequest,
            result => {
                Debug.Log("CloudScript result " + result.FunctionResult.ToString());
                ReadyToLoad();

                /*---------------------------------------------------------------------------------------------------------------------------------------
                // We have the results of the Engage AB Test on the client should we want to do anything with it,
                // but we don't need to. The Statistics have been updated on the PlayFab server by the cloud script 

                Dictionary<string, object> response = DeltaDNA.MiniJSON.Json.Deserialize(result.FunctionResult.ToString()) as Dictionary<string, object>;
                if (response.ContainsKey("parameters"))
                {
                    Dictionary<string, object> parameters = response["parameters"] as Dictionary<string, object>;
                    UpdatePlayerParameters(parameters);
                }
                //--------------------------------------------------------------------------------------------------------------------------------------*/


            },
            error => {
                Debug.Log("CloudScript error" + error.ErrorMessage);
                ReadyToLoad();
            });
    }

    public void ReadyToLoad()
    {
        //bttnStart.image.color = new Color(1.0f,1.0f,1.0f,0.0f);
        bttnStart.gameObject.SetActive(true);
        readyToStart = true;
    }


    public void ButtonStart_Click()
    {
        // Move to Main scene now that DDNA and PlayFab SDKs are running
        // and player config AB Tests have been checked.
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
