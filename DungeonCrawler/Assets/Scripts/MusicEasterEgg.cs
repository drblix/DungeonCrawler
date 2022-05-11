using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEasterEgg : MonoBehaviour
{
    private AudioSource source;

    [SerializeField]
    private AudioClip scatmansWorld;
    
    [SerializeField]
    private AudioClip skipbabopScatman;

    private KeyCode[] sequence = new KeyCode[]
    {
        KeyCode.S,
        KeyCode.C,
        KeyCode.A,
        KeyCode.T,
        KeyCode.M,
        KeyCode.A,
        KeyCode.N
    };

    private int sequenceIndex = 0;

    public static bool secretTriggered = false;

    private void Awake() 
    {
        source = GetComponent<AudioSource>();

        if (secretTriggered)
        {
            Destroy(gameObject);
        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(sequence[sequenceIndex]) && PlayerSettings.cheatsEnabled && !secretTriggered)
        {
            CheatsManager cheatsManager = FindObjectOfType<CheatsManager>();

            if (cheatsManager != null)
            {
                if (cheatsManager.ConsoleOpen)
                {
                    return;
                }
            }

            if (++sequenceIndex == sequence.Length)
            {
                secretTriggered = true;
                sequenceIndex = 0;
                source.Stop();
                
                if (Random.Range(1, 3) == 1)
                {
                    source.clip = scatmansWorld;
                }
                else
                {
                    source.clip = skipbabopScatman;
                }

                source.volume = 1f;
                source.Play();

                transform.name = "SCATMAN WOAH";
                transform.tag = "Untagged";

                Debug.Log("Scatman secret triggered ;)");
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (Input.anyKeyDown)
        {
            sequenceIndex = 0;
        }
    }
}
