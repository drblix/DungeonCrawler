using UnityEngine;
using TMPro;

public class CoinManagement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    private const int maxCoins = 999;

    private static int currentCoins = 0;
    public static int CurrentCoins { get { return currentCoins; } }

    private void Update()
    {
        coinText.text = currentCoins.ToString();
    }

    public void AddCoins(int amount)
    {
        currentCoins += Mathf.Abs(amount);

        if (currentCoins > maxCoins) { currentCoins = maxCoins; }

        coinText.text = amount.ToString();
    }

    public void RemoveCoins(int amount)
    {
        currentCoins -= Mathf.Abs(amount);

        if (currentCoins > maxCoins) { currentCoins = maxCoins; }

        coinText.text = amount.ToString();
    }

    public static void ResetCurrency()
    {
        currentCoins = 0;
    }
}
