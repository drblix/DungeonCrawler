using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    const float scaleTime = 0.2f;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
