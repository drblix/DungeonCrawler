using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField]
    private Transform aimer;

    [SerializeField]
    private GameObject[] aimDisplay; // 0 - Right; 1 - Left; 2 - Top; 3 - Bottom;

    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private TextMeshProUGUI ammoDisplay;

    [SerializeField]
    private float moveSpeed;

    public int maxAmmo = 5;
    private int currentAmmo;

    private bool canShoot = true;
    private bool reloading = false;

    [SerializeField]
    private float shootCooldown = 0.2f;
    [SerializeField]
    private float reloadTime = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        aimDisplay[0].SetActive(true);
        currentAmmo = maxAmmo;

        UpdateDisplay();
    }

    private void Update()
    {
        InputCheck();
    }

    private void FixedUpdate()
    {
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
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            if (currentAmmo == 0)
            {
                StartCoroutine(Reload());
            }
            else if (currentAmmo > 0)
            {
                canShoot = false;
                currentAmmo--;
                UpdateDisplay();
                StartCoroutine(ShootArrow());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading && currentAmmo != maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    private void UpdateDisplay()
    {
        if (reloading) { return; }

        string newText = string.Format("{0} / {1}", currentAmmo, maxAmmo);

        ammoDisplay.text = newText;
    }

    private IEnumerator Reload()
    {
        if (reloading) { yield return null; }

        reloading = true;
        canShoot = false;

        ammoDisplay.text = "Reloading...";

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        reloading = false;
        canShoot = true;

        UpdateDisplay();
    }

    private IEnumerator ShootArrow()
    {
        Instantiate(arrow, aimer.position, aimer.rotation);

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }
}
