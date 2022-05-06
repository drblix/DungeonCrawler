using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{   
    [SerializeField]
    private Slider volumeSlider;
    
    [SerializeField]
    private Toggle textNotiToggle;

    [SerializeField]
    private Toggle sfxToggle;

    [SerializeField]
    private TMP_InputField seedInput;

    const float scaleTime = 0.2f;
    
    private void Start() 
    {
        volumeSlider.value = AudioListener.volume;
        textNotiToggle.isOn = PlayerSettings.disableNotiSound;
        sfxToggle.isOn = PlayerSettings.disabledSFX;
    }

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(seedInput.text))
        {   
            int seed;

            if (int.TryParse(seedInput.text, out int result))
            {
                seed = result;
                RoomGeneration.SetGenerationSeed(seed);
            }
            else
            {
                Debug.LogError("Inputted seed could not be parsed into an integer!");
                RoomGeneration.RandomGenerationSeed();
            }
        }
        else
        {
            RoomGeneration.RandomGenerationSeed();
        }

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
