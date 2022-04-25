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
            enemyHealth.poisoned = true;
        }

        sRenderer = GetComponent<SpriteRenderer>();

        if (sRenderer == null)
        {
            sRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        StartCoroutine(PoisonDamage());
    }

    private IEnumerator PoisonDamage()
    {
        int num = Random.Range(2, 3);
        sRenderer.color = Color.green;

        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(1.5f);

            if (isDummy)
            {
                trainingDummy.PlayAnim();
            }
            else
            {
                enemyHealth.StartDamage(1);
            }

            DamagePopup.Create(transform.position, 1, false);
        }

        enemyHealth.poisoned = false;
        yield return new WaitForSeconds(0.1f);
        
        sRenderer.color = Color.white;

        Destroy(this);
    }
}