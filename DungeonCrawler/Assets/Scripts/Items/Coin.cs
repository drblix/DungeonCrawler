using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManagement coinManagement;

    private SpriteRenderer spriteRenderer;
    private AudioSource pickUpSFX;

    [SerializeField] [Min(1)]
    private int coinValue = 1;

    private bool canPickup = true;

    private void Awake()
    {
        coinManagement = FindObjectOfType<CoinManagement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickUpSFX = GetComponent<AudioSource>();
    }

    // What happens when player picks up coin
    private IEnumerator PickUp()
    {
        spriteRenderer.enabled = false;
        pickUpSFX.Play();
        coinManagement.AddCoins(coinValue); // Adds coin value to player currency

        yield return new WaitForSeconds(pickUpSFX.clip.length);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canPickup)
        {
            canPickup = false; // Prevents from calling twice
            StartCoroutine(PickUp());
        }
    }

    public void SetCoinAmount(int amount)
    {
        if (amount != 0)
        {
            coinValue = Mathf.Abs(amount);
        }
        else
        {
            coinValue = 1;
        }
    }
}
