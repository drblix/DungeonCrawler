using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private AudioSource pickUpSFX;

    [SerializeField] [Min(1)]
    private int coinValue;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        pickUpSFX = GetComponent<AudioSource>();
    }

    private IEnumerator PickUp()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        pickUpSFX.Play();
        // Add to coins amount

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
