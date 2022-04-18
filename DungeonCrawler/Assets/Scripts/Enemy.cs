using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private ParticleSystem pSystem;
    private AIPath aiPath;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer sRenderer;

    [SerializeField]
    private int maxHealth = 5;

    private int currentHealth;

    private bool dead = false;

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        aiPath = GetComponent<AIPath>();
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

        sRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        sRenderer.color = Color.white;
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

        pSystem.Emit(Random.Range(15, 25));
        sRenderer.enabled = false;

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
    }
}
