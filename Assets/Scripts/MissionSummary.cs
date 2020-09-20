using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSummary : MonoBehaviour
{
    public Text txtSummary;
    public Text txtMissionReward;
    public GameManager gameManager;
    public Button bttnContinue;
    public Button bttnClose;
    
    public bool isVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(PlayerManager.State s)
    {

        isVisible = true;
        SetVisibility(isVisible);

        
        
        


        switch (s)
        {
            case PlayerManager.State.ALIVE:
                txtSummary.text = string.Format("Level {0} Cleared",gameManager.currentLevel);
                bttnClose.gameObject.SetActive(false);
                bttnContinue.gameObject.SetActive(true);
                if (gameManager.levels[gameManager.currentLevel].reward > 0)
                {
                    txtMissionReward.text = string.Format("Reward {0} Coins", gameManager.levels[gameManager.currentLevel].reward);
                    txtMissionReward.gameObject.SetActive(true);
                }
                break;
            case PlayerManager.State.DEAD:
                txtSummary.text = "You Died";
                bttnClose.gameObject.SetActive(true);
                bttnContinue.gameObject.SetActive(false);
                txtMissionReward.gameObject.SetActive(false);
                break; 
        }
        if (gameManager.player.playerCoins < gameManager.levels[gameManager.currentLevel].cost)
        {
            gameManager.placementManager.Show();
        }

    }

    
    public void Hide()
    {
        isVisible = false;
        SetVisibility(isVisible);
    }

    public void Continue()
    {
        if (gameManager.player.playerCoins >= gameManager.levels[gameManager.currentLevel].cost)
        {
            Hide();
            gameManager.StartLevel(gameManager.currentLevel);
        }
    }

    private void SetVisibility(bool v)
    {       
        gameObject.SetActive(v);
    }
}
