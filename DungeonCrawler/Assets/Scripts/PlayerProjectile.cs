using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
    // Streamlining the instantiation of magic missiles :)

    /// <summary>
    /// Creates a magic missile / player projectile
    /// </summary>
    /// <param name="type">Type of missile to create ('MagicMissile', 'AcidMissile', or 'FireMissile')</param>
    /// <param name="dmgAmnt">Amount of damage the missile does</param>
    /// <returns>Returns the missile created with the provided parameters</returns>
    public static PlayerProjectile Create(string type, int dmgAmnt)
    {
        bool isPoison;
        var fabMissile = GameAssets.LoadPrefabFromFile(type);
        var newMissile = Instantiate(fabMissile, fabMissile.transform.position, Quaternion.identity);

        PlayerProjectile projScript = newMissile.GetComponent<PlayerProjectile>();

        if (newMissile.name == "AcidMissile")
        {
            isPoison = true;
        }
        else
        {
            isPoison = false;
        }

        projScript.SetUp(dmgAmnt, isPoison);
        
        return projScript;
    }

    private Rigidbody2D rb;
    [SerializeField]
    private ParticleSystem pSystem;
    [SerializeField]
    private ParticleSystem smokeTrail;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private readonly string[] collisionExclusions = { "Pickup", }; // Tags

    private const float arrowSpeed = 8f;

    [SerializeField]
    private bool isPoison = false;
    private bool hit = false;

    [SerializeField]
    private int damageAmount = 1;

    private const int critChance = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        smokeTrail.Play();
    }

    private void SetUp(int dmgAmount, bool poison)
    {

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
            if (!isPoison)
            {
                bool isCrit = Random.Range(1, 100) <= critChance;

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
            else
            {
                collision.gameObject.AddComponent<PoisonScript>();
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
