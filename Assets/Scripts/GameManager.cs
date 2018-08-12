using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using DeltaDNA; 

public class GameManager : MonoBehaviour {

    public string sharedUserID; 

	// Use this for initialization
	void Start () {


        // Setup some DeltaDNA config stuff
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);

        // Log the player in to PlayFab
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
    #region PlayFab Player Login Methods
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Player Logged in to PlayFab");
        sharedUserID = result.PlayFabId;
        Debug.Log("Shared UserID =  " + sharedUserID);
        
        // Start deltaDNA SDK and use the PlayFab userID
        DDNA.Instance.StartSDK(sharedUserID);

    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());


        
        // Start deltaDNA without PlayFab userID if not available.
        DDNA.Instance.StartSDK();

    }
    #endregion
}
