using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class PlayerHealth : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sRenderer;
    private Animator animator;
    private ParticleSystem pSystem;

    private Image heart01;
    private Image heart02;
    private Image heart03;

    private AudioSource source;

    private const int maxHealth = 6;
    private static int currentHealth = 6;
    public static int CurrentHealth { get { return currentHealth; } }

    private bool canDamage = true;

    private void Awake()
    {
        player = GetComponent<Player>();
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        pSystem = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();

        heart01 = GameObject.Find("Heart1").GetComponent<Image>();
        heart02 = GameObject.Find("Heart2").GetComponent<Image>();
        heart03 = GameObject.Find("Heart3").GetComponent<Image>();

        UpdateUI();
    }

    public void AddHealth(int amount)
    {
        currentHealth += Mathf.Abs(amount);

        if (currentHealth > maxHealth) { currentHealth = maxHealth; }

        UpdateUI();
    }

    public void RemoveHealth(int amount)
    {
        if (!canDamage) { return; }
        canDamage = false;

        currentHealth -= Mathf.Abs(amount);

        if (currentHealth > maxHealth) { currentHealth = maxHealth; }

        if (currentHealth <= 0) { StartCoroutine(GameOver()); }

        StartCoroutine(DamageFlash());
        UpdateUI();
    }

    private IEnumerator GameOver()
    {
        player.ToggleEnabled(false);
        animator.SetBool("Dead", true);

        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            collider.enabled = false;
        }

        foreach (AIPath path in FindObjectsOfType<AIPath>())
        {
            path.enabled = false;
        }

        transform.Find("AimDisplays").gameObject.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        sRenderer.enabled = false;
        pSystem.Play();
        source.Play();

        yield return new WaitForSeconds(2f);

        // To game over screen or something
    }

    private IEnumerator DamageFlash()
    {
        sRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        sRenderer.color = Color.white;
        canDamage = true;
    }

    private void UpdateUI()
    {
        switch (currentHealth)
        {
            case 6:
                heart01.GetComponent<Animator>().SetBool("Broken", false);
                heart01.gameObject.SetActive(true);
                heart02.GetComponent<Animator>().SetBool("Broken", false);
                heart02.gameObject.SetActive(true);
                heart03.GetComponent<Animator>().SetBool("Broken", false);
                heart03.gameObject.SetActive(true);
                break;

            case 5:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(true);
                heart02.GetComponent<Animator>().SetBool("Broken", false);
                heart02.gameObject.SetActive(true);
                heart03.GetComponent<Animator>().SetBool("Broken", false);
                heart03.gameObject.SetActive(true);
                break;

            case 4:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(false);
                heart02.GetComponent<Animator>().SetBool("Broken", false);
                heart02.gameObject.SetActive(true);
                heart03.GetComponent<Animator>().SetBool("Broken", false);
                heart03.gameObject.SetActive(true);
                break;

            case 3:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(false);
                heart02.GetComponent<Animator>().SetBool("Broken", true);
                heart02.gameObject.SetActive(true);
                heart03.GetComponent<Animator>().SetBool("Broken", false);
                heart03.gameObject.SetActive(true);
                break;

            case 2:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(false);
                heart02.GetComponent<Animator>().SetBool("Broken", true);
                heart02.gameObject.SetActive(false);
                heart03.GetComponent<Animator>().SetBool("Broken", false);
                heart03.gameObject.SetActive(true);
                break;

            case 1:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(false);
                heart02.GetComponent<Animator>().SetBool("Broken", true);
                heart02.gameObject.SetActive(false);
                heart03.GetComponent<Animator>().SetBool("Broken", true);
                heart03.gameObject.SetActive(true);
                break;

            case 0:
                heart01.GetComponent<Animator>().SetBool("Broken", true);
                heart01.gameObject.SetActive(false);
                heart02.GetComponent<Animator>().SetBool("Broken", true);
                heart02.gameObject.SetActive(false);
                heart03.GetComponent<Animator>().SetBool("Broken", true);
                heart03.gameObject.SetActive(false);
                break;
        }
    }
}
