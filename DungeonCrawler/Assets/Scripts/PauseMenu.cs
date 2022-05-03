using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private RoomGeneration roomGeneration;

    const float scaleTime = 0.2f;

    private void Awake() 
    {
        roomGeneration = FindObjectOfType<RoomGeneration>();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (roomGeneration != null)
            {
                if (!roomGeneration.DoneLoading) { return; }
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
