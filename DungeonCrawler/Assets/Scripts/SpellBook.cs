using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    private Button mainButton;

    [SerializeField]
    private Button closeButton;
    
    [SerializeField]
    private Button page1Next;

    [SerializeField]
    private Button page2Back;

    [SerializeField]
    private GameObject bookStuff;

    [SerializeField]
    private GameObject page1;

    [SerializeField]
    private GameObject page2;

    private AudioSource audioSrc;

    private void Awake() 
    {
        audioSrc = GetComponent<AudioSource>();

        bookStuff.SetActive(false);
        page2.SetActive(false);
    }

    private void Start() 
    {
        mainButton.onClick.AddListener(delegate { ButtonPressed("Main"); });
        closeButton.onClick.AddListener(delegate { ButtonPressed("Close"); });
        page1Next.onClick.AddListener(delegate { ButtonPressed("Page1"); });
        page2Back.onClick.AddListener(delegate { ButtonPressed("Page2"); });
    }

    private void ButtonPressed(string eventName)
    {
        Player player = FindObjectOfType<Player>();

        switch (eventName)
        {
            case "Main":
                if (bookStuff.activeInHierarchy || !player.PlayerEnabled) { return; }
                bookStuff.SetActive(true);
                player.ToggleEnabled(false);
                break;

            case "Close":
                bookStuff.SetActive(false);
                page1.SetActive(true);
                page2.SetActive(false);
                player.ToggleEnabled(true);
                break;

            case "Page1":
                page1.SetActive(false);
                page2.SetActive(true);
                break;

            case "Page2":
                page1.SetActive(true);
                page2.SetActive(false);
                break;
        }

        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
    }
}
