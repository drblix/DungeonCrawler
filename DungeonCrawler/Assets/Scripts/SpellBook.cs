using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public void TogglePlayer(bool state)
    {
        Player player = FindObjectOfType<Player>();
        player.ToggleEnabled(state);
    }

    public void SizeUpButton(Transform obj)
    {
        UIElementEffects.SizeUpButton(obj);
    }

    public void SizeDownButton(Transform obj)
    {
        UIElementEffects.SizeDownButton(obj);
    }
}
