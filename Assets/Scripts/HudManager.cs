using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    public Text txtHealth;
    public Text txtCoins;
    public Text txtLevel;
    public Text txtDifficulty;

    public void Start()
    {
        // Clear HUD until values are retrieved from server
        this.txtHealth.text ="";
        this.txtCoins.text = "";
        this.txtLevel.text = "";
        this.txtDifficulty.text = "";
    }

    public void SetHealth(int health)
    {
        this.txtHealth.text = health.ToString();
    }
    public void SetCoins(int coins)
    {
        this.txtCoins.text = coins.ToString();
    }
    public void SetLevel(int level)
    {
        this.txtLevel.text = level.ToString();
    }
    public void SetDifficulty(int difficulty)
    {
        this.txtDifficulty.text = difficulty.ToString();
    }
}
