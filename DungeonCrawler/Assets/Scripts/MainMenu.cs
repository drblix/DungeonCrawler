using UnityEngine;

public class MainMenu : MonoBehaviour
{
    const float scaleTime = 0.2f;
    
    public void StartGame()
    {
        SceneTransitioner.CreateTransition(3, Vector2.zero);
    }

    public void StartTutorial()
    {
        SceneTransitioner.CreateTransition(1, Vector2.zero);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SizeUpButton(Transform obj)
    {
        Vector3 scaleVector = new Vector3(1.1f, 1.1f, 1.1f);

        obj.LeanScale(scaleVector, scaleTime).setEaseOutCirc();
    }

    public void SizeDownButton(Transform obj)
    {
        obj.LeanScale(Vector3.one, scaleTime).setEaseInCirc();
    }

    public void DisableNotiSound(bool state)
    {
        PlayerSettings.disableNotiSound = state;
    }

    public void DisableSFX(bool state)
    {
        PlayerSettings.disabledSFX = state;
    }

    public void ModifyVolume(float num)
    {
        AudioListener.volume = num;
    }
}

public class PlayerSettings
{
    public static bool disableNotiSound = false;
    public static bool disabledSFX = false;
}
