using System.Collections;
using UnityEngine;
using TMPro;

public class TextNotification : MonoBehaviour
{
    /// <summary>
    /// Creates a text notification box using the provided text and reveal speed
    /// </summary>
    /// <param name="text">Text to display</param>
    /// <param name="revealSpeed">Speed at which the text reveals</param>
    /// <returns>Returns the notification box created using the previously provided parameters</returns>
    public static TextNotification Create(string[] text, float revealSpeed)
    {
        var fabNoti = GameUtilities.LoadPrefabFromFile("TextNoti");
        var newNoti = Instantiate(fabNoti, fabNoti.transform.position, Quaternion.identity);

        TextNotification notiScript = newNoti.GetComponent<TextNotification>();
        notiScript.Setup(text, revealSpeed);

        return notiScript;
    }

    private TextMeshProUGUI uiText;
    private AudioSource audioSource;
    private Canvas canvas;

    private string[] textToReveal;
    private float speedPerChar;
    private int pageAmount;

    bool skipped = false;

    private void Awake()
    {
        uiText = GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();

        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.2f;
        canvas.sortingLayerID = SortingLayer.NameToID("UI");

        if (PlayerSettings.disableNotiSound)
        {
            audioSource.volume = 0f;
        }
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skipped = true;
        }
    }

    private void Setup(string[] text, float num)
    {
        uiText.text = null;
        textToReveal = text;
        speedPerChar = num;
        pageAmount = text.Length;

        StartCoroutine(PlayText());
    }

    private IEnumerator PlayText()
    {
        RectTransform notiRect = transform.GetChild(0).GetComponent<RectTransform>();
        notiRect.localScale = new Vector3(0f, 0f, 0f);

        notiRect.LeanScale(Vector3.one, 1f).setEaseOutBounce();

        yield return new WaitForSeconds(1f);

        // Loops through each page (element) of the string array that was passed in
        for (int i = 1; i <= pageAmount; i++)
        {
            skipped = false;
            uiText.text = null;

            foreach (char c in textToReveal[i - 1])
            {
                uiText.text += c;
                audioSource.Play();

                if (char.IsPunctuation(c))
                {
                    yield return new WaitForSeconds(0.4f);
                }
                else
                {
                    yield return new WaitForSeconds(speedPerChar);
                }

                if (skipped) { break; }
            }

            uiText.text = textToReveal[i - 1];
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            if (i == pageAmount)
            {
                Player player = FindObjectOfType<Player>();
                player.ToggleEnabled(true);

                uiText.text = null;
                notiRect.LeanScale(Vector3.zero, 1f).setEaseInCirc();

                yield return new WaitForSeconds(1f);

                Destroy(gameObject);
            }
        }
    }
}
