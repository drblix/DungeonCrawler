using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private enum PotionType
    {
        ManaPotion,
        HealthPotion
    }

    [SerializeField]
    private PotionType type;

    [SerializeField]
    private int manaGain = 40;

    [SerializeField]
    private int healthGain = 1;

    private SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;
    private PlayerMana playerMana;

    [SerializeField]
    private Sprite manaPotionSprite;
    [SerializeField]
    private Sprite healthPotionSprite;

    bool used = false;

    private void Awake() 
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerMana = FindObjectOfType<PlayerMana>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (type)
        {
            case PotionType.ManaPotion:
                spriteRenderer.sprite = manaPotionSprite;
                transform.name = "ManaPotion";
                break;

            case PotionType.HealthPotion:
                spriteRenderer.sprite = healthPotionSprite;
                transform.name = "HealthPotion";
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player") && !used)
        {
            switch (type)
            {
                case PotionType.ManaPotion:
                    if (playerMana.CurrentMana != playerMana.MaxMana)
                    {
                        used = true;
                        playerMana.AddMana(manaGain);
                        Destroy(gameObject);
                    }
                    break;

                case PotionType.HealthPotion:
                    if (PlayerHealth.CurrentHealth != 6)
                    {
                        used = true;
                        playerHealth.AddHealth(healthGain);
                        Destroy(gameObject);
                    }
                    break;

                default:
                    throw new System.Exception("Enum potion type invalid!");
            }
        }
    }
}
