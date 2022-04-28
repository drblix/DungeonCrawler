using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSrc;

    float scaleTime = 0.2f;

    private void Awake() 
    {
        audioSrc = GetComponent<AudioSource>();
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
