using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI CoinDisplay;
    public Inventory PlayerInventory;

    public void UpdateCoinCount()
    {
        CoinDisplay.text = "" + PlayerInventory.Coins;
    }
}