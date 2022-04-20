using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyArrow : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private ParticleSystem pSystem;
    [SerializeField]
    private ParticleSystem smokeTrail;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private readonly string[] collisionExclusions = { "Pickup", }; // Tags

    private const float arrowSpeed = 8f;

    private bool hit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        smokeTrail.Play();
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
        bool excluded = false;

        foreach (string exclusion in collisionExclusions)
        {
            if (collision.CompareTag(exclusion))
            {
                excluded = true;
            }
        }

        if (hit || excluded) { return; }
        hit = true;
        GetComponent<BoxCollider2D>().enabled = false;

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log("Damaging!");
                playerHealth.RemoveHealth(1);
            }
        }

        StartCoroutine(Contact());
    }

    private IEnumerator Contact()
    {
        var emission = smokeTrail.emission;
        spriteRenderer.enabled = false;
        audioSource.Play();
        emission.enabled = false;
        pSystem.Play();

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
    }
}
