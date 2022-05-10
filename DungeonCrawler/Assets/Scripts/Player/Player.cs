using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMana playerMana;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField]
    private Transform aimer;

    [SerializeField]
    private GameObject[] aimDisplay; // 0 - Right; 1 - Left; 2 - Top; 3 - Bottom;

    [SerializeField]
    private float moveSpeed;

    private bool canShoot = true;

    public static float shootCooldown = 0.35f; // Default 0.35f

    public static bool noclipEnabled = false;

    private bool playerEnabled = true;
    public bool PlayerEnabled { get { return playerEnabled; } }

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        aimDisplay[0].SetActive(true);
    }

    private void Update()
    {
        if (!playerEnabled) { return; }

        InputCheck();
    }

    private void FixedUpdate()
    {
        if (!playerEnabled) { return; }

        Movement();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = 0f;

        if (horizontal == 0f) // Restricts diagonal movement
        {
            vertical = Input.GetAxisRaw("Vertical");
        }

        float num = Mathf.Sign(horizontal);

        SetAimer(horizontal, vertical);

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Moving", true);

            if (num == 1)
            {
                spriteRenderer.flipX = false;
            }
            else if (num == -1)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        Vector2 movement = moveSpeed * Time.fixedDeltaTime * new Vector2(horizontal, vertical).normalized;

        rb.MovePosition(rb.position + movement);
    }

    private void SetAimer(float horizontal, float vertical)
    {
        if (horizontal == 0f && vertical == 0f) { return; }

        float num1 = Mathf.Sign(horizontal);
        float num2 = Mathf.Sign(vertical);

        if (horizontal != 0f)
        {
            if (num1 == 1)
            {
                aimer.rotation = Quaternion.Euler(Vector3.zero);

                aimDisplay[0].SetActive(true);
                aimDisplay[1].SetActive(false);
                aimDisplay[2].SetActive(false);
                aimDisplay[3].SetActive(false);
            }
            else
            {
                aimer.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));

                aimDisplay[0].SetActive(false);
                aimDisplay[1].SetActive(true);
                aimDisplay[2].SetActive(false);
                aimDisplay[3].SetActive(false);
            }
        }

        if (vertical != 0f)
        {
            if (num2 == 1)
            {
                aimer.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));

                aimDisplay[0].SetActive(false);
                aimDisplay[1].SetActive(false);
                aimDisplay[2].SetActive(true);
                aimDisplay[3].SetActive(false);
            }
            else
            {
                aimer.rotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));

                aimDisplay[0].SetActive(false);
                aimDisplay[1].SetActive(false);
                aimDisplay[2].SetActive(false);
                aimDisplay[3].SetActive(true);
            }
        }
    }

    private void InputCheck()
    {
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            if (playerMana.RemoveMana(7f))
            {
                canShoot = false;
                StartCoroutine(ShootProjectile());
            }
        }
    }

    private IEnumerator ShootProjectile()
    {
        PlayerProjectile newMissile = PlayerProjectile.Create(PlayerProjectile.MissileType.MagicMissile, 1);
        newMissile.transform.SetPositionAndRotation(aimer.position, aimer.rotation);

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }

    public void ToggleEnabled(bool state)
    {
        playerEnabled = state;
        animator.SetBool("Moving", false);
    }

    public void SetPlayerSpeed(float value)
    {
        moveSpeed = Mathf.Abs(value);
    }

    public void ToggleNoclip(bool state)
    {
        noclipEnabled = state;

        if (noclipEnabled)
        {
            foreach (BoxCollider2D cllder in GetComponents<BoxCollider2D>())
            {
                if (!cllder.isTrigger)
                {
                    cllder.enabled = false;
                }
            }
        }
        else
        {
            foreach (BoxCollider2D cllder in GetComponents<BoxCollider2D>())
            {
                cllder.enabled = true;
            }
        }
    }
}
