using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManagement coinManagement;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private AudioSource pickUpSFX;

    [SerializeField] [Min(1)]
    private int coinValue;

    private void Awake()
    {
        coinManagement = FindObjectOfType<CoinManagement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        pickUpSFX = GetComponent<AudioSource>();
    }

    private IEnumerator PickUp()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        pickUpSFX.Play();
        coinManagement.AddCoins(coinValue);

        yield return new WaitForSeconds(pickUpSFX.clip.length);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PickUp());
        }
    }
}
