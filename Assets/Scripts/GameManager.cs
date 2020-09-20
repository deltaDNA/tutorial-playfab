using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using DeltaDNA;

public class Level
{
    public int food { get; set; }
    public int poison { get; set; }
    public int cost { get; set; }
    public int reward { get; set; }
    public int timelimit { get; set; }
}



public class GameManager : MonoBehaviour
{

   // public ChilliConnectSdk chilliConnect;
    public string chilliConnectId = null;
    private string chilliConnectSecret = null;



    public PlayerManager player;
    public PlacementManager placementManager;
    public GameObject snakePrefab;
    private Snake snake;
    public MissionSummary missionSummary;

    public GameConsole console;
    public Text txtStart;
    public Text txtGameOver;
    public Text txtCost;
    public Button bttnStart;

    public int currentLevel = 1;
    public List<Level> levels;
    const int DEFAULT_FOOD_SPAWN = 6;
    public int foodSpawn;
    public int foodLevelOveride = 0;

    // Start Button Size and Color
    private Color sourceColor;
    private Color targetColor;
    private Vector3 InitialScale;
    private Vector3 FinalScale;
    public bool readyToStart = false;
    public bool gameplayPaused = true;

    bool waiting = false;



    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //Application.Quit();


        // A simple debug console in game
        console.UpdateConsole();

        // Start ChilliConnect SDK, Login, then start deltaDNA SDK
        StartSDKs();


        // These are for pulsing the start button size and alpha 
        InitialScale = transform.localScale;
        FinalScale = new Vector3(InitialScale.x + 0.04f,
                                 InitialScale.y + 0.04f,
                                 InitialScale.z);
        sourceColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        targetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);






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



    private void StartSDKs()
    {
        // Start the ChilliConnect SDK
        //ChilliConnectInit();

        // Login to ChilliConnect and start deltaDNA SDK
        //LogIn(chilliConnectId, chilliConnectSecret);
    }



    /*
    // Start the ChilliConnect SDK, creating a player if one is not stored on the client.
    private void ChilliConnectInit()
    {
        // InitialiseChilliConnect SDK with our Game Token
        //chilliConnect = new ChilliConnectSdk("6PvaW0XKPZF3wUTOavDPwcLQUho9DQdS", true); 
        //chilliConnect = new ChilliConnectSdk("hxNSsyHz0KYmgNweytP4AiJGiuJIfl8t", true);

        // Create a Player and store the ChilliConnectId if we don't already have one
        if (!PlayerPrefs.HasKey("ChilliConnectId") || !PlayerPrefs.HasKey("ChilliConnectSecret"))
        {
            var createPlayerRequest = new CreatePlayerRequestDesc();

            // Create Player Account
            chilliConnect.PlayerAccounts.CreatePlayer(createPlayerRequest,
                (CreatePlayerRequest request, CreatePlayerResponse response) =>
                {
                    Debug.Log("Create Player successfull : " + response.ChilliConnectId);

                    PlayerPrefs.SetString("ChilliConnectId", response.ChilliConnectId);
                    PlayerPrefs.SetString("ChilliConnectSecret", response.ChilliConnectSecret);

                    chilliConnectId = response.ChilliConnectId;
                    chilliConnectSecret = response.ChilliConnectSecret;
                },
                (CreatePlayerRequest request, CreatePlayerError error) =>
                {
                    Debug.Log("An error occurred Creating Player : " + error.ErrorDescription);
                }
            );
        }
        else
        {
            chilliConnectId = PlayerPrefs.GetString("ChilliConnectId");
            chilliConnectSecret = PlayerPrefs.GetString("ChilliConnectSecret");
        }

    }
    */
    /*
    // Login to ChilliConnect, then start deltaDNA SDK
    private void LogIn(string chilliConnectId, string chilliConnectSecret)
    {
        var loginRequest = new LogInUsingChilliConnectRequestDesc(chilliConnectId, chilliConnectSecret);
        chilliConnect.PlayerAccounts.LogInUsingChilliConnect(loginRequest,
            (LogInUsingChilliConnectRequest request, LogInUsingChilliConnectResponse response) =>
            {
                Debug.Log("Login using ChilliConnect OK");

                // Start the deltaDNA SDK using the chilliConnectIs as the deltaDNA userID
                DeltaDNAInit(chilliConnectId);
                GetChilliGameConfig();
            },
            (LogInUsingChilliConnectRequest request, LogInUsingChilliConnectError error) =>
            {
                Debug.Log("An error occurred during ChilliConnect Player Login : " + error.ErrorDescription + "\n Data : " + error.ErrorData);
                Debug.Log("Quitting");
                Application.Quit();
            }
        );
    }

        */
        /*
    private void RemoteCampaign(string decisionPoint, string parameters)
    {

        var scriptParams = new Dictionary<string, SdkCore.MultiTypeValue>();
        scriptParams.Add("decisionPoint", decisionPoint);
        scriptParams.Add("locale", "en_GB");
        scriptParams.Add("platform", DDNA.Instance.Platform);
        if (!string.IsNullOrEmpty(parameters)) scriptParams.Add("parameters", parameters);

        var runScriptRequest = new RunScriptRequestDesc("ENGAGE_DECISION_POINT_CAMPAIGN");
        runScriptRequest.Params = scriptParams;

        Debug.Log("Running Engage Campaign Script for decisionPoint : " + decisionPoint);
        chilliConnect.CloudCode.RunScript(runScriptRequest
            , (request, response) => {

                var engageResponse = response.Output.AsDictionary();

                if (engageResponse.ContainsKey("parameters"))
                {
                    var p = engageResponse["parameters"].AsDictionary();
                    foreach (var i in p)
                    {
                        //Debug.Log("Response Parameter : " + i.Key + " Value : " + i.Value);

                        if (i.Key == "placementType")
                            placementManager.type = i.Value.AsString();

                        if (i.Key == "placementPosition")
                            placementManager.position = i.Value.AsString();

                        if (i.Key == "placementFrequency")
                            placementManager.frequency = i.Value.AsInt();

                        if (i.Key == "placementSessionCap")
                            placementManager.limit = i.Value.AsInt();

                        if (i.Key == "placementType")
                            placementManager.type = i.Value.AsString();

                        if (i.Key == "placementPromoID")
                            placementManager.promoID = i.Value.AsInt();
                    }
                }
            }
            , (request, error) => Debug.LogError(error.ErrorDescription));
    }

*/

    // Setup a few things and start the deltaDNA SDK
    private void DeltaDNAInit(string chilliConnectId)
    {
        // Configure some things
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
        DDNA.Instance.ClientVersion = Application.version;

        // Event Triggered Campaigns configuration settings
        DDNA.Instance.Settings.MultipleActionsForEventTriggerEnabled = true;
        DDNA.Instance.NotifyOnSessionConfigured(true);
        DDNA.Instance.OnSessionConfigured += (bool cachedConfig) => GetDDNAGameConfig(cachedConfig);

        // Register Handlers for Event Triggered Campaign responses
        DDNA.Instance.Settings.DefaultGameParameterHandler = new GameParametersHandler(gameParameters =>
        {
            MyGameParameterHandler(gameParameters);
        });
        DDNA.Instance.Settings.DefaultImageMessageHandler = new ImageMessageHandler(DDNA.Instance, imageMessage =>
        {
            MyImageMessageHandler(imageMessage);
        });


        // Start the SDK with the chilliConnectId to ensure deltaDNA.userID and ChilliConnectId are the same.
        DDNA.Instance.StartSDK(chilliConnectId);
    }


    
    // Get Game Configuration from PlayFab
    private void GetChilliGameConfig()
    {
       // player.FetchCurrency(chilliConnect);

        Debug.Log("Fetching metadata to configure game levels");
        //chilliConnect.Catalog.GetMetadataDefinitions(new GetMetadataDefinitionsRequestDesc(), OnMetaDataFetched, (request, error) => Debug.LogError(error.ErrorDescription));
    }

    
    /*
    // Handle Game Configuration repsonse from ChilliConnect
    private void OnMetaDataFetched(GetMetadataDefinitionsRequest request, GetMetadataDefinitionsResponse response)
    {
        Debug.Log("Metadata fetched: ");
        levels = new List<Level>();

        foreach (MetadataDefinition metadataItem in response.Items)
        {
            var levelList = metadataItem.CustomData.AsDictionary().GetList("levels");

            foreach (var level in levelList)
            {
                Level l = new Level();

                if (level.AsDictionary().ContainsKey("food"))
                    l.food = level.AsDictionary().GetInt("food");

                if (level.AsDictionary().ContainsKey("poison"))
                    l.poison = level.AsDictionary().GetInt("poison");

                if (level.AsDictionary().ContainsKey("cost"))
                    l.cost = level.AsDictionary().GetInt("cost");

                if (level.AsDictionary().ContainsKey("reward"))
                    l.reward = level.AsDictionary().GetInt("reward");

                if (level.AsDictionary().ContainsKey("timelimit"))
                    l.timelimit = level.AsDictionary().GetInt("timelimit");

                levels.Add(l);
            }
        }
        Debug.Log("Levels Loaded " + levels.Count);

        // Show the Start button       
        txtStart.gameObject.SetActive(true);
        txtGameOver.gameObject.SetActive(false);
        txtCost.gameObject.SetActive(true);
        txtCost.text = string.Format("Cost {0} Coins", levels[currentLevel].cost);
        bttnStart.gameObject.SetActive(true);

        // Start pulsing the start button
        readyToStart = true;
    }

    */


    private void GetDDNAGameConfig(bool cachedConfig)
    {
        Debug.Log("Received deltaDNA configuration");

        DDNA.Instance.RecordEvent(new GameEvent("gameConfigured")
            .AddParam("cachedConfiguration", cachedConfig ? 1 : 0))
            .Run();

       // RemoteCampaign("placement", null);


    }

    public void ShowRewardedAd(string rewardName, int rewardValue)
    {

    }


    private void MyGameParameterHandler(Dictionary<string, object> gameParameters)
    {
        Debug.Log("Received deltaDNA gameParameters from event triggered campaign" + DeltaDNA.MiniJSON.Json.Serialize(gameParameters));

        foreach (string key in gameParameters.Keys)
        {
            if (key == "action")
            {
                string actionName = gameParameters[key].ToString();
                if (actionName == "ShowRewardedAd")
                {
                    string rewardName = gameParameters["rewardName"].ToString();
                    int rewardAmount = System.Convert.ToInt32(gameParameters["rewardAmount"]);
                    ShowRewardedAd(rewardName, rewardAmount);
                }
            }

            // Coin Balalnce Modifier
            if (key == "coins")
            {
                int c = System.Convert.ToInt32(gameParameters[key]);
                RewardReceived("coins", "Event Triggered Campaign reward", c);
            }

            // Level configuration modifiers
            if (key == "food" || key == "poison" || key == "missionCost" || key == "missionReward" || key == "timelimit")
            {
                int v = System.Convert.ToInt32(gameParameters[key]);
                Debug.Log(string.Format("Mission {0} {1} configuration changed to {2}", currentLevel, key, v));
                missionModified(currentLevel, key, v);

                switch (key)
                {
                    case "food":
                        levels[currentLevel - 1].food = v;
                        break;
                    case "poison":
                        levels[currentLevel - 1].poison = v;
                        break;
                    case "missionCost":
                        levels[currentLevel - 1].cost = v;
                        break;
                    case "missionReward":
                        levels[currentLevel - 1].reward = v;
                        break;
                    case "timelimit":
                        levels[currentLevel - 1].timelimit = v;
                        break;
                }
            }
        }
    }




    private void MyImageMessageHandler(ImageMessage imageMessage)
    {
        // Add Handler for Image Message 'dismiss' action
        imageMessage.OnDismiss += (ImageMessage.EventArgs obj) =>
        {
            Debug.Log("Image Message dismissed by " + obj.ID);
            // NB We won't process any game parameters if the player dimisses the Image Message
        };

        // Add Handler for Image Message 'action' action
        imageMessage.OnAction += (ImageMessage.EventArgs obj) =>
        {
            Debug.Log("Image Message actioned by " + obj.ID + " with command " + obj.ActionValue);

            // Process any parameters received with the Image Message
            if (imageMessage.Parameters != null)
            {
                MyGameParameterHandler(imageMessage.Parameters);
            }
        };

        // The image message is cached, it will show instantly
        imageMessage.Show();
    }

    // Called from Start Button
    public void NewGame()
    {
        // Check the player can afford to play
        if (player.playerCoins >= levels[currentLevel - 1].cost)
        {
            player.NewPlayer();
            StartLevel(1);
        }
        else
        {
            placementManager.Show();
        }
    }


    public void StartLevel(int levelNo)
    {
        // Deduct Level cost from balance
        player.SpendCoins(levels[currentLevel - 1].cost);
        player.UpdatePlayerStatistics();


        // as well as from other end of previous levels.
        currentLevel = levelNo;
        player.state = PlayerManager.State.ALIVE;


        txtGameOver.gameObject.SetActive(false);
        txtStart.gameObject.SetActive(false);
        txtCost.gameObject.SetActive(false);
        bttnStart.gameObject.SetActive(false);

        // Only spawn new Snake at start of game or on retry
        if (readyToStart)
        {
            readyToStart = false;
            // Spawn new Snake 

            Vector3 pos = new Vector3(0, 0, -1);
            snake = Instantiate(snakePrefab, pos, Quaternion.identity).GetComponent<Snake>();
        }

        gameplayPaused = false;
        // Record DDNA MissionStarted event
        DDNA.Instance.RecordEvent(new GameEvent("missionStarted")
            .AddParam("missionName", "Mission " + currentLevel.ToString("D3"))
            .AddParam("missionID", currentLevel.ToString("D3"))
            .AddParam("userLevel", player.playerLevel)
            .AddParam("isTutorial", false)
            .AddParam("coinBalance", player.playerCoins)
            .AddParam("food", levels[currentLevel - 1].food)
            .AddParam("poison", levels[currentLevel - 1].poison)
            .AddParam("missionCost", levels[currentLevel - 1].cost)
            .AddParam("missionReward", levels[currentLevel - 1].reward)
            .AddParam("timelimit", levels[currentLevel - 1].timelimit))
            .Run();

    }



    public void PlayerDied()
    {
        player.Kill();
        missionSummary.Show(PlayerManager.State.DEAD);

        // Record DDNA MissionFailed event
        DDNA.Instance.RecordEvent(new GameEvent("missionFailed")
            .AddParam("missionName", "Mission " + currentLevel.ToString("D3"))
            .AddParam("missionID", currentLevel.ToString("D3"))
            .AddParam("userLevel", player.playerLevel)
            .AddParam("isTutorial", false)
            .AddParam("coinBalance", player.playerCoins)
            .AddParam("foodRemaining", player.foodRemaining)
            .AddParam("food", levels[currentLevel - 1].food)
            .AddParam("poison", levels[currentLevel - 1].poison)
            .AddParam("missionCost", levels[currentLevel - 1].cost)
            .AddParam("missionReward", levels[currentLevel - 1].reward)
            .AddParam("timelimit", levels[currentLevel - 1].timelimit))
            .Run();

        txtGameOver.gameObject.SetActive(true);
        txtStart.gameObject.SetActive(true);
        txtCost.gameObject.SetActive(true);
        bttnStart.gameObject.SetActive(true);
        readyToStart = true;
        Pause();



    }


    public void Pause()
    {
        gameplayPaused = true;
    }

    public void LevelUp()
    {


        GameEvent m = new GameEvent("missionCompleted")
            .AddParam("missionName", "Mission " + currentLevel.ToString("D3"))
            .AddParam("missionID", currentLevel.ToString("D3"))
            .AddParam("isTutorial", false)
            .AddParam("userLevel", player.playerLevel)
            .AddParam("coinBalance", player.playerCoins)
            .AddParam("food", levels[currentLevel - 1].food)
            .AddParam("poison", levels[currentLevel - 1].poison)
            .AddParam("missionCost", levels[currentLevel - 1].cost)
            .AddParam("missionReward", levels[currentLevel - 1].reward)
            .AddParam("timelimit", levels[currentLevel - 1].timelimit);

        if (levels[currentLevel - 1].reward > 0)
        {
            RewardReceived("coins", "mission " + currentLevel + "completion reward", levels[currentLevel - 1].reward);
            m.AddParam("reward", new Params()
                .AddParam("rewardName", string.Format("mission {0} completion reward", currentLevel))
                .AddParam("rewardProducts", new Product()
                    .AddVirtualCurrency("Coins", "GRIND", 20)
                ));
        }

        // Record DDNA MissionCompleted event
        DDNA.Instance.RecordEvent(m)
            .Run();

        currentLevel++;

        if (currentLevel > player.playerLevel)
        {
            player.SetLevel(currentLevel);
            Debug.Log("Level Up - playerLevel " + player.playerLevel);
        }


        Debug.Log("Next Level " + currentLevel);

        player.UpdatePlayerStatistics();

        //missionSummary.Show(player.state);
        // Pause();

    }


    public void RewardReceived(string rewardType, string rewardReason, int rewardAmount)
    {
        Debug.Log(string.Format("Player Rewarded {0} {1} for {2}", rewardAmount, rewardType, rewardReason));

        if (rewardType == "coins")
        {
            player.AddCoins(rewardAmount);
        }

        DDNA.Instance.RecordEvent(new GameEvent("rewardReceived")
            .AddParam("rewardType", rewardType)
            .AddParam("rewardReason", rewardReason)
            .AddParam("rewardAmount", rewardAmount)
            .AddParam("userLevel", player.playerLevel)
            .AddParam("coinBalance", player.playerCoins))
            .Run();
    }
    public void missionModified(int missionID, string modifierType, int modifierAmount)
    {
        DDNA.Instance.RecordEvent(new GameEvent("missionModified")
            .AddParam("missionID", missionID.ToString("D3"))
            .AddParam("userLevel", player.playerLevel)
            .AddParam("coinBalance", player.playerCoins)
            .AddParam("food", levels[missionID - 1].food)
            .AddParam("poison", levels[missionID - 1].poison)
            .AddParam("missionCost", levels[missionID - 1].cost)
            .AddParam("missionReward", levels[missionID - 1].reward)
            .AddParam("timelimit", levels[missionID - 1].timelimit))
            .Run();
    }


}
