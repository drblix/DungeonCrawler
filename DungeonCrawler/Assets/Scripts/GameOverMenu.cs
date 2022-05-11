using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Transform title;
    [SerializeField]
    private Transform returnButton;

    const float scaleTime = 0.2f;
    
    bool canUse = false;

    private void Start() 
    {
        StartCoroutine(InitialAnimations());
    }

    private IEnumerator InitialAnimations()
    {
        title.LeanMoveLocal(new Vector2(0f, 377f), 2f).setEaseOutBounce();

        yield return new WaitForSeconds(2f);

        returnButton.LeanMoveLocal(new Vector2(0f, -353f), 1.5f);

        yield return new WaitForSeconds(1.5f);

        canUse = true;
    }

    public void ReturnToMenu()
    {
        if (canUse)
        {
            SceneTransitioner.CreateTransition(0, Vector2.zero);
        }
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
