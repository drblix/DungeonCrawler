using UnityEngine;
using TMPro;

public class ShopManagement : MonoBehaviour
{
    private CoinManagement coinManager;

    [SerializeField]
    private GameObject shopCanvas;
    [SerializeField]
    private GameObject textPrompt;
    
    [SerializeField]
    private TextMeshProUGUI healthPotTextUI;
    [SerializeField]
    private TextMeshProUGUI manaPotTextUI;

    [SerializeField]
    private GameObject healthOoS;
    [SerializeField]
    private GameObject manaOoS;

    [SerializeField]
    private GameObject healthPotion;
    [SerializeField]
    private GameObject manaPotion;

    bool canOpenShop = false;

    private int healthPotPrice;
    private int healthPotQuantity;
    private int manaPotPrice;
    private int manaPotQuantity;

    const float scaleTime = 0.2f;

    private void Awake() 
    {
        coinManager = FindObjectOfType<CoinManagement>();

        healthPotPrice = Random.Range(3, 8);
        manaPotPrice = Random.Range(2, 5);
        healthPotQuantity = Random.Range(1, 4);
        manaPotQuantity = Random.Range(1, 4);

        healthPotTextUI.SetText(healthPotPrice.ToString() + " Coins");
        manaPotTextUI.SetText(manaPotPrice.ToString() + " Coins");
    }

    public void PurchaseHealthPot()
    {
        if (CoinManagement.CurrentCoins >= healthPotPrice && healthPotQuantity > 0)
        {
            healthPotQuantity--;
            Vector3 playerPos = FindObjectOfType<Player>().transform.position;

            coinManager.RemoveCoins(healthPotPrice);
            Instantiate(healthPotion, playerPos, Quaternion.identity);
        }
        else if (healthPotQuantity <= 0)
        {
            healthOoS.SetActive(true);
        }
    }

    public void PurchaseManaPot()
    {
        if (CoinManagement.CurrentCoins >= manaPotPrice && manaPotQuantity > 0)
        {
            manaPotQuantity--;
            Vector3 playerPos = FindObjectOfType<Player>().transform.position;

            coinManager.RemoveCoins(manaPotPrice);
            Instantiate(manaPotion, playerPos, Quaternion.identity);
        }
        else if (manaPotQuantity <= 0)
        {
            manaOoS.SetActive(true);
        }
    }

    public void SizeUpButton(Transform obj)
    {
        Vector3 scaleVector = new Vector3(1.1f, 1.1f, 1.1f);

        obj.LeanScale(scaleVector, scaleTime).setEaseOutCirc();
    }

    public void SizeDownButton(Transform obj)
    {
        obj.LeanScale(Vector3.one, scaleTime).setEaseInCirc();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            canOpenShop = true;
            textPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            canOpenShop = false;
            shopCanvas.SetActive(false);
            textPrompt.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.G) && canOpenShop)
        {
            shopCanvas.SetActive(true);
        }
    }
}