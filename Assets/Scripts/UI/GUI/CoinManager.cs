using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI CoinDisplay;
    public Inventory PlayerInventory;

    private void Start()
    {
        
    }

    public void UpdateCoinCount()
    {
        CoinDisplay.text = "" + PlayerInventory.Coins;
    }
}
