using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private SpriteRenderer sRenderer;

    private Image heart01;
    private Image heart02;
    private Image heart03;

    private const int maxHealth = 6;
    private static int currentHealth = 6;

    private bool canDamage = true;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();

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

        if (currentHealth <= 0) { GameOver(); }

        StartCoroutine(DamageFlash());
        UpdateUI();
    }

    private void GameOver()
    {
        Debug.Log("You dead");
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
