using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneTransitioner.CreateTransition(0, Vector2.zero);
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
