using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI CoinDisplay;
    public Inventory PlayerInventory;

    private void Awake()
    {
        CoinDisplay.text = "" + PlayerInventory.Coins;
    }

    public void UpdateCoinCount()
    {
        CoinDisplay.text = "" + PlayerInventory.Coins;
    }
}