using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlacementManager : MonoBehaviour
{
    //public Placement placement; 

    public string type { get; set; }
    public string position { get; set; }
    public int frequency { get; set; }
    public int limit { get; set; }
    public int promoID { get; set; }


    private bool isVisible = false;

    private GameManager gameManager;
        
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        SetVisibility(isVisible);
    }
    public void Show()
    {
        isVisible = true;
        SetVisibility(isVisible);
    }
    public void Hide()
    {
        isVisible = false;
        SetVisibility(isVisible);

    }
    private void SetVisibility(bool v)
    {
        gameObject.SetActive(v);
    }
    public void FreeCoins()
    {
        gameManager.player.AddCoins(50);
        Hide();
    }
}
