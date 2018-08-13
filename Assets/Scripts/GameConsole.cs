using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeltaDNA;
using PlayFab;

public class GameConsole : MonoBehaviour {

    public Text textConsole;
    public Text textDdnaUserID;
    public Text textPlayFabID;
    public Text textUnityVersion;
    public Text textDdnaSDKVersion;
    public bool isConsoleVisible = false; 

    // UI Debug console will be used to let player see what the game and SDKs are doing.
    public int numConsoleLines = 16;
    private List<string> console = new List<string>();


    private void Awake()
    {
        // This will allow us to display Unity Debug.Log in the UI
        Application.logMessageReceived += PrintToConsole;

    }

    private void Start()
    {

        textDdnaUserID.text = "deltaDNA UserID : " + DDNA.Instance.UserID;               
        textDdnaSDKVersion.text = "deltaDNA SDK Version : " + Settings.SDK_VERSION;
        textUnityVersion.text = "Unity Version : " + Application.unityVersion;
        PlayFabClientAPI.GetAccountInfo(new PlayFab.ClientModels.GetAccountInfoRequest(),
                result =>{
                    if (result.AccountInfo != null && result.AccountInfo.PlayFabId != null)
                    {
                        textPlayFabID.text = "PlayFab UserID : " + result.AccountInfo.PlayFabId;
                    }  
                    
                },
                error => {
                    Debug.Log("Got error getting Account Info:");
                    Debug.Log(error.GenerateErrorReport());
                }
            );
        SetConsoleVisibility(false);
    }

    private void PrintToConsole(string logString, string stackTrace, LogType type)
    {
        console.Add(string.Format("{0}::{1}\n", System.DateTime.Now.ToString("h:mm:ss tt"), logString));
        if (console.Count > numConsoleLines)
        {
            console.RemoveRange(0, console.Count - numConsoleLines);
        }
        textConsole.text = "";
        console.ForEach(i => textConsole.text += i);
    }

    // Show / Hide Info panel and Console when player clicks Info button.
    public void ToggleConsoleVisibility()
    {
        isConsoleVisible = !isConsoleVisible;
        SetConsoleVisibility(isConsoleVisible);
    }

    private void SetConsoleVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

}
