using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private TextMeshProUGUI seedLabel;

    private RoomGeneration roomGeneration;

    const float scaleTime = 0.2f;

    private void Awake() 
    {
        roomGeneration = FindObjectOfType<RoomGeneration>();

        if (!RoomGeneration.UsingRandomSeed)
        {
            seedLabel.SetText("Seed: " + RoomGeneration.GenerationSeed.ToString());
        }
        else
        {
            seedLabel.SetText("Seed: " + (int)System.DateTime.Now.Ticks);
        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (roomGeneration != null)
            {
                if (!roomGeneration.DoneLoading) 
                { 
                    SceneTransitioner.CreateTransition(0, Vector2.zero); 
                    return; 
                }
            }
            
            FindObjectOfType<Player>().ToggleEnabled(false);
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    public void UnPause()
    {
        FindObjectOfType<Player>().ToggleEnabled(true);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneTransitioner.CreateTransition(0, Vector2.zero);
        Destroy(gameObject);
    }
}
