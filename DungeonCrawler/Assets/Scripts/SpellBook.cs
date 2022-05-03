using UnityEngine;

public class SpellBook : MonoBehaviour
{
    const float scaleTime = 0.2f;

    public void TogglePlayer(bool state)
    {
        Player player = FindObjectOfType<Player>();
        player.ToggleEnabled(state);
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
}
