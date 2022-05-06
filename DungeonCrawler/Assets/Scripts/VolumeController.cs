using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeController : MonoBehaviour
{
    private Transform player;
    private AudioSource audioSource;

    private const float minDistance = 1f;

    [SerializeField]
    private float maxDistance = 100f;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        audioSource = GetComponent<AudioSource>();

        if (PlayerSettings.disabledSFX)
        {
            audioSource.volume = 0f;
        }
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < minDistance && !PlayerSettings.disabledSFX)
        {
            audioSource.volume = 1f;
        }
        else if (distance > maxDistance && !PlayerSettings.disabledSFX)
        {
            audioSource.volume = 0f;
        }
        else if (!PlayerSettings.disabledSFX)
        {
            audioSource.volume = 1f - ((distance - minDistance) / (maxDistance - minDistance));
        }
    }
}
