using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SkeletonAI : MonoBehaviour
{
    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private EnemyHealth enemyHealth;

    [SerializeField]
    private GameObject enemyArrow;
    [SerializeField]
    private Transform aimBit;

    [SerializeField]
    private float distanceToShoot = 5f;

    [SerializeField]
    private float shootCooldown = 2.5f;

    private LayerMask collisionLayers;

    private bool canShoot = false;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        enemyHealth = GetComponent<EnemyHealth>();

        collisionLayers = LayerMask.GetMask("Player") | LayerMask.GetMask("Foreground");

        StartCoroutine(ShootArrow());
    }

    private void Update()
    {
        if (!enemyHealth.Dead)
        {
            CheckDistanceAndSight();
            AimBitLook();
        }
        else
        {
            aiPath.canMove = false;
            canShoot = false;
        }
    }

    private void AimBitLook()
    {
        Vector2 playerPos = destinationSetter.target.position;
        Vector2 diff = (playerPos - (Vector2)transform.position).normalized;

        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        aimBit.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    private void CheckDistanceAndSight()
    {
        Vector2 direction = (destinationSetter.target.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, collisionLayers);

        if (hit.collider.CompareTag("Weapon")) { return; }

        if (Vector2.Distance(transform.position, destinationSetter.target.position) < distanceToShoot && hit.collider != null && hit.collider.CompareTag("Player"))
        {
            aiPath.canMove = false;

            Debug.DrawRay(transform.position, direction, Color.red, 1f);

            canShoot = true;
        }
        else
        {
            aiPath.canMove = true;
            canShoot = false;
        }
    }

    private IEnumerator ShootArrow()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootCooldown);

            if (canShoot)
            {
                Instantiate(enemyArrow, aimBit.position, aimBit.rotation);
            }
        }
    }
}
