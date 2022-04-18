using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private ParticleSystem pSystem;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private const float arrowSpeed = 8f;

    private bool hit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pSystem = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!hit)
        {
            Vector2 movement = arrowSpeed * Time.fixedDeltaTime * transform.TransformDirection(Vector2.right);

            rb.MovePosition(rb.position + movement);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) { return; }
        hit = true;

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.StartDamage(1);
            }
        }

        StartCoroutine(Contact());
    }

    private IEnumerator Contact()
    {
        spriteRenderer.enabled = false;
        audioSource.Play();
        pSystem.Play();

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
    }
}
