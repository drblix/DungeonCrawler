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
    private Toggle cheatsToggle;

    [SerializeField]
    private TMP_InputField seedInput;
    
    private void Start() 
    {
        volumeSlider.value = AudioListener.volume;
        textNotiToggle.isOn = PlayerSettings.disableNotiSound;
        sfxToggle.isOn = PlayerSettings.disabledSFX;
        cheatsToggle.isOn = PlayerSettings.cheatsEnabled;
    }

    public void StartGame()
    {
        PlayerHealth.godMode = false;
        PlayerMana.unlimitedMana = false;
        Player.shootCooldown = 0.35f;
        Player.noclipEnabled = false;
        ShopManagement.freeShopItems = false;
        // Reverts all cheats that could've carried over

        if (!string.IsNullOrEmpty(seedInput.text))
        {   
            int seed;

            seed = seedInput.text.GetHashCode();

            RoomGeneration.SetGenerationSeed(seedInput.text, seed);
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
        UIElementEffects.SizeUpButton(obj);
    }
    public void SizeDownButton(Transform obj)
    {
        UIElementEffects.SizeDownButton(obj);
    }

    public void DisableNotiSound(bool state)
    {
        PlayerSettings.disableNotiSound = state;
    }

    public void DisableSFX(bool state)
    {
        PlayerSettings.disabledSFX = state;
    }

    public void ToggleCheats(bool state)
    {
        PlayerSettings.cheatsEnabled = state;
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
    public static bool cheatsEnabled = false;
}
