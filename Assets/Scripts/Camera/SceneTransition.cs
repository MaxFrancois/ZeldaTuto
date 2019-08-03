using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string SceneToLoad;
    public Vector2 PlayerPosition;
    public VectorValue PlayerStorage;
    public GameObject FadeInPanel;
    public GameObject FadeToPanel;
    public float FadeWait;

    //private void Awake()
    //{
    //    if (FadeInPanel != null)
    //    {
    //        var panel = Instantiate(FadeInPanel, Vector3.zero, Quaternion.identity);
    //        Destroy(panel, 1);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            PlayerStorage.InitialValue = PlayerPosition;
            //SceneManager.LoadScene(SceneToLoad);/
            StartCoroutine(FadeCo());
        }
    }
    public IEnumerator FadeCo()
    {
        if (FadeToPanel != null)
        {
            var panel = Instantiate(FadeToPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(FadeWait);
        var asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
