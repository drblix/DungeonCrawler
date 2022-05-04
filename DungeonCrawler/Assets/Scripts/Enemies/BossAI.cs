using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class BossAI : MonoBehaviour
{   
    private AIPath aiPath;
    private AIDestinationSetter setter;
    private GameObject shield;

    [SerializeField]
    private Transform rotatePart;

    [SerializeField]
    private GameObject bossProjectile;

    private AudioSource musicSource;

    [SerializeField]
    private AudioClip bossMusic;
    
    [SerializeField]
    private EdgeCollider2D roomCollider;

    [SerializeField]
    private ParticleSystem sparkEffect;

    AudioClip originalClip;

    bool dead = false;
    private bool canDamage = true;

    private void Awake() 
    {
        aiPath = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
        shield = transform.parent.Find("Shield").gameObject;
        musicSource = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();

        setter.target = FindObjectOfType<Player>().transform;
    }

    private void OnEnable() 
    {
        StartCoroutine(BossEnabled());
    }

    private IEnumerator BossEnabled()
    {
        aiPath.canMove = false;
        yield return new WaitUntil(() => Vector2.Distance(setter.target.position, transform.position) <= 4f);
        
        Destroy(shield);

        aiPath.canMove = true;

        Debug.Log("Player nearby!");

        roomCollider.enabled = true;
        
        originalClip = musicSource.clip;
        musicSource.Stop();
        musicSource.clip = bossMusic;
        musicSource.Play();

        StartCoroutine(BossAttackCooldown());
    }

    private IEnumerator BossAttack(int type)
    {
        Debug.Log(type);

        switch (type)
        {
            case 1: // Spin attack thingy
                aiPath.canMove = false;

                for (int i = 0; i < 12; i++)    
                {
                    rotatePart.RotateAround(transform.position, Vector3.forward, 30f);
                    Instantiate(bossProjectile, rotatePart.position, rotatePart.rotation);
                    yield return new WaitForSeconds(0.03f);
                }

                rotatePart.localPosition = new Vector2(0f, 0.386f);
                rotatePart.localRotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            case 2: // Speed boost
                sparkEffect.Play();
                float oldSpeed = aiPath.maxSpeed;
                aiPath.maxSpeed = 1.5f;

                yield return new WaitForSeconds(2f);

                aiPath.maxSpeed = oldSpeed;
                break;

            default:
                Debug.Log("Out of range");
                break;
        }

        aiPath.canMove = true;
    }

    private IEnumerator BossAttackCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 12f));

            if (dead) { break; }

            StartCoroutine(BossAttack(Random.Range(1, 3)));
        }
    }

    public void Death()
    {
        StopCoroutine(BossAttackCooldown());

        dead = true;
        musicSource.Stop();
        musicSource.clip = originalClip;
        musicSource.Play();

        SceneTransitioner.CreateTransition(SceneManager.GetActiveScene().buildIndex + 1, Vector2.zero);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && canDamage)
        {
            canDamage = false;
            StartCoroutine(TouchDmgCooldown());

            PlayerHealth plrHealth = other.GetComponent<PlayerHealth>();
            plrHealth.RemoveHealth(2);
        }
    }

    private IEnumerator TouchDmgCooldown()
    {
        yield return new WaitForSeconds(1f);
        canDamage = true;
    }
}
