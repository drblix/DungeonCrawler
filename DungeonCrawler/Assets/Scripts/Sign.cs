using UnityEngine;

public class Sign : MonoBehaviour
{
    private GameObject textNoti;

    [SerializeField] [Tooltip("The text to be displayed in the message notification. Each element is a page in the message.")]
    private string[] text;
    [SerializeField]
    private float revealSpeed;

    bool canUse = false;

    // <summary> Checks for when player collides with the sign. Upon doing so, toggles the
    // "inDialogue" bool in player and creates a text box with the text provided </summary>

    private void Awake()
    {
        textNoti = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canUse)
        {
            SignTriggered();
        }
    }

    private void SignTriggered()
    {
        Player player = FindObjectOfType<Player>();

        if (player == null || !player.PlayerEnabled) { return; }

        player.ToggleEnabled(false);

        TextNotification.Create(text, revealSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textNoti.SetActive(true);
            canUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textNoti.SetActive(false);
            canUse = false;
        }
    }
}
