using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Create(int sceneToLoad)
    {
        var fabTransition = GameUtilities.LoadPrefabFromFile("SceneTransition");
        var newTransition = Instantiate(fabTransition, fabTransition.transform.position, Quaternion.identity);

        SceneTransitioner transitioner = newTransition.GetComponent<SceneTransitioner>();
        transitioner.Setup(sceneToLoad);

        return transitioner;
    }

    private Animator animator;

    private int loadingSceneIndex = 0;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Start() 
    {
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    private void Setup(int sceneLoading)
    {
        loadingSceneIndex = sceneLoading;
        StartCoroutine(Transition());
    }

    public void LoadNextScene()
    {
        Debug.Log("Loading scene " + loadingSceneIndex.ToString());
        SceneManager.LoadScene(loadingSceneIndex);
    }

    private IEnumerator Transition()
    {
        animator.SetTrigger("BeginTrans");
        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
}
