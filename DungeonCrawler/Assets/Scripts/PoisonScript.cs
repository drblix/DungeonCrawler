using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonScript : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private TrainingDummy trainingDummy;
    private SpriteRenderer sRenderer;

    bool isDummy = false;

    private void Awake()
    {
        if (transform.name.Contains("TrainingDummy")) { isDummy = true; }

        if (isDummy)
        {
            trainingDummy = GetComponent<TrainingDummy>();
        }
        else if (transform.CompareTag("Enemy"))
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        sRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(PoisonDamage());
    }

    private IEnumerator PoisonDamage()
    {
        int num = Random.Range(3, 5);
        sRenderer.color = Color.green;


        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(1.5f);

            if (isDummy)
            {
                trainingDummy.PlayAnim();
                DamagePopup.Create(transform.position, 1, false);
            }
            else
            {
                enemyHealth.StartDamage(1);
            }
        }

        yield return new WaitForSeconds(0.1f);

        sRenderer.color = Color.white;

        Destroy(this);
    }
}