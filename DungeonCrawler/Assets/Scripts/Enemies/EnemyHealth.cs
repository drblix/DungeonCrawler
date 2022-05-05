using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyHealth : MonoBehaviour
{
    private ParticleSystem pSystem;
    private AudioSource audioSource;
    private AIPath aiPath;
    private Seeker seeker;
    private AIDestinationSetter setter;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer sRenderer;
    [SerializeField]
    private GameObject droppedItem;

    [SerializeField]
    private int maxHealth = 5;
    private int currentHealth;

    public bool poisoned = false;

    [SerializeField]
    private bool isBoss = false;

    private bool dead = false;
    public bool Dead { get { return dead; } }

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        setter = GetComponent<AIDestinationSetter>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    
    private void OnEnable() 
    {
        if (dead)
        {
            animator.SetBool("Dead", true);
        }
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
            Death();
            yield return null;
        }

        sRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (!poisoned)
        {
           sRenderer.color = Color.white; 
        }
        else
        {
            sRenderer.color = Color.green;
        }
    }

    private void Death()
    {
        // aiPath.canMove = false;
        transform.parent.parent.GetComponentInChildren<RoomCameraHandler>().EnemyDied();
        Destroy(aiPath);
        Destroy(seeker);
        Destroy(setter);
        Destroy(GetComponentInChildren<GFXHandler>());

        animator.SetBool("Dead", true);
        audioSource.Play();

        if (droppedItem != null && !isBoss)
        {
            Vector2 coinPos = transform.position;
            coinPos.y += 1;

            GameObject newCoin = Instantiate(droppedItem, coinPos, Quaternion.identity);
            newCoin.GetComponent<Coin>().SetCoinAmount(1);
        }

        foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
        {
            collider.enabled = false;
        }


        if (isBoss)
        {
            GetComponent<BossAI>().Death();
        }
        /*

        yield return new WaitForSeconds(1.5f);

        newCoin.GetComponent<Coin>().SetCoinAmount(1);
        pSystem.Emit(Random.Range(15, 25));
        sRenderer.enabled = false;

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
        */
    }
}
