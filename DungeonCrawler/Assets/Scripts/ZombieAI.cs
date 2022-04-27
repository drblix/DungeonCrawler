using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ZombieAI : MonoBehaviour
{
    private AIPath aiPath;

    private AIDestinationSetter destinationSetter;

    private EnemyHealth enemyHealth;

    bool canDamage = true;

    private void Awake() 
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        enemyHealth = GetComponent<EnemyHealth>();

        destinationSetter.target = FindObjectOfType<Player>().transform;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && canDamage)
        {
            canDamage = false;
            StartCoroutine(Cooldown());

            PlayerHealth plrHealth = other.GetComponent<PlayerHealth>();
            plrHealth.RemoveHealth(1);
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canDamage = true;
    }
}
