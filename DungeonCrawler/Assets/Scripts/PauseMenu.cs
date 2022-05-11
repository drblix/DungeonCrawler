using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private TextMeshProUGUI seedLabel;

    [SerializeField]
    private TextMeshProUGUI cheatsLabel;

    private RoomGeneration roomGeneration;

    private void Awake() 
    {
        roomGeneration = FindObjectOfType<RoomGeneration>();

        if (!RoomGeneration.UsingRandomSeed)
        {
            seedLabel.SetText("Seed: " + RoomGeneration.DisplaySeed);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            seedLabel.SetText("Seed: N/A");
        }
        else
        {
            seedLabel.SetText("Seed: " + (int)System.DateTime.Now.Ticks);
        }

        if (PlayerSettings.cheatsEnabled)
        {
            cheatsLabel.SetText("Cheats enabled: <color=\"green\">Yes");
        }
        else
        {
            cheatsLabel.SetText("Cheats enabled: <color=\"red\">No");
        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SceneTransitioner.transitioning)
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
    }
}
