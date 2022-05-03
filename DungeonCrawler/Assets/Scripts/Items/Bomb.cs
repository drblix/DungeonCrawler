using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private CircleCollider2D circleCollider;
    private ParticleSystem pSystem;
    private AudioSource audioSource;

    private const float preBlastTime = 2f;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        pSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Explodey());
    }

    private IEnumerator Explodey()
    {
        transform.LeanScale(Vector2.one, preBlastTime);
        transform.gameObject.LeanColor(Color.red, preBlastTime);

        yield return new WaitForSeconds(preBlastTime);

        sRenderer.enabled = false;
        circleCollider.enabled = true;
        circleCollider.radius = 3f;
        pSystem.Play();
        audioSource.Play();

        yield return new WaitForSeconds(0.05f);

        circleCollider.enabled = false;

        yield return new WaitForSeconds(pSystem.main.duration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage thingy
        Debug.Log(collision.name);
    }
}
