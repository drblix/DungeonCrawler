using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossAI : MonoBehaviour
{   
    private AIPath aiPath;
    private AIDestinationSetter setter;

    [SerializeField]
    private Transform rotatePart;

    [SerializeField]
    private GameObject bossProjectile;


    private void Awake() 
    {
        aiPath = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();

        setter.target = FindObjectOfType<Player>().transform;
    }

    private void OnEnable() 
    {
        if (Vector2.Distance(setter.target.position, transform.position) > 15f) { return; }
        
        Debug.Log("Player nearby!");

        StartCoroutine(BossAttackCooldown());
    }

    private IEnumerator BossAttack(int type)
    {
        aiPath.canMove = false;
        Debug.Log(type);

        switch (type)
        {
            case 1: // Spin attack thingy
                for (int i = 0; i < 12; i++)    
                {
                    rotatePart.RotateAround(transform.position, Vector3.forward, 30f);
                    Instantiate(bossProjectile, rotatePart.position, rotatePart.rotation);
                    yield return new WaitForSeconds(0.03f);
                }

                rotatePart.localPosition = new Vector2(0f, 0.386f);
                rotatePart.localRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }

        aiPath.canMove = true;
    }

    private IEnumerator BossAttackCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 12f));

            StartCoroutine(BossAttack(Random.Range(1, 1)));
        }
    }
}
