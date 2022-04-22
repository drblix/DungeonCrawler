using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
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

    public static int damageAmount = 1;
    public static int maxCritChance = 6;

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

        if (collision.CompareTag("Enemy"))
        {
            int critChance = Random.Range(1, maxCritChance);
            bool isCrit = false;

            if (critChance == 1) { isCrit = true; }

            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            Vector2 collisionPos = collision.transform.position;
            collisionPos.y += 0.5f;

            if (enemy != null)
            {
                if (isCrit)
                {
                    enemy.StartDamage(damageAmount * 2);
                }
                else
                {
                    enemy.StartDamage(damageAmount);
                }
            }

            if (isCrit)
            {
                DamagePopup.Create(collisionPos, damageAmount * 2, true);
            }
            else
            {
                DamagePopup.Create(collisionPos, damageAmount, false);
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
