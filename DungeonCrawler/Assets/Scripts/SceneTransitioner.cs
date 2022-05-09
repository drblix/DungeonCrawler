using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public static void CreateTransition(int sceneToLoad, Vector2 position)
    {
        var fabTransition = GameUtilities.LoadPrefabFromFile("SceneTransition");
        var newTransition = Instantiate(fabTransition, fabTransition.transform.position, Quaternion.identity);

        SceneTransitioner transitioner = newTransition.GetComponentInChildren<SceneTransitioner>();
        transitioner.Setup(sceneToLoad, position);
    }

    private Animator animator;

    private int loadingSceneIndex = 0;

    public static bool transitioning = false;

    private void Awake() 
    {
        if (FindObjectsOfType<SceneTransitioner>().Length > 1)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        animator = GetComponent<Animator>();
        Time.timeScale = 1f;
    }

    private void Setup(int sceneLoading, Vector2 position)
    {
        loadingSceneIndex = sceneLoading;
        transform.parent.SetPositionAndRotation(position, Quaternion.identity);
        StartCoroutine(Transition());
    }

    public void LoadNextScene()
    {
        Debug.Log("Loading scene " + loadingSceneIndex.ToString());
        SceneManager.LoadScene(loadingSceneIndex);
    }

    private IEnumerator Transition()
    {
        transitioning = true;
        yield return new WaitForSeconds(2f);
        transitioning = false;
        Destroy(transform.parent.gameObject);
    }
}
