using UnityEngine;

public class UIElementEffects
{
    const float scaleTime = 0.2f;

    public static void SizeUpButton(Transform obj)
    {
        Vector3 scaleVector = new Vector3(1.1f, 1.1f, 1.1f);

        obj.LeanScale(scaleVector, scaleTime).setEaseOutExpo();
    }

    public static void SizeDownButton(Transform obj)
    {
        obj.LeanScale(Vector3.one, scaleTime).setEaseOutExpo();
    }
}
