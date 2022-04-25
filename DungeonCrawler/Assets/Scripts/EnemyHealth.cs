using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyHealth : MonoBehaviour
{
    private ParticleSystem pSystem;
    private AudioSource audioSource;
    private AIPath aiPath;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer sRenderer;
    [SerializeField]
    private GameObject coin;

    [SerializeField]
    private int maxHealth = 5;
    private int currentHealth;

    private bool dead = false;
    public bool Dead { get { return dead; } }

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        aiPath = GetComponent<AIPath>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }


    public void StartDamage(int dmgAmount)
    {
        if (dead) { return; }

        StartCoroutine(TakeDamage(dmgAmount));
    }

    private IEnumerator TakeDamage(int amount)
    {
        currentHealth -= Mathf.Abs(amount);

        if (currentHealth <= 0)
        {
            dead = true;
            StartCoroutine(Death());
            yield return null;
        }

        Color ogColor = sRenderer.color;
        sRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        sRenderer.color = ogColor;
    }

    private IEnumerator Death()
    {
        aiPath.canMove = false;
        animator.SetBool("Dead", true);

        foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(1.5f);

        audioSource.Play();
        GameObject newCoin = Instantiate(coin, transform.position, Quaternion.identity);
        newCoin.GetComponent<Coin>().SetCoinAmount(1);
        pSystem.Emit(Random.Range(15, 25));
        sRenderer.enabled = false;

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
    }
}
