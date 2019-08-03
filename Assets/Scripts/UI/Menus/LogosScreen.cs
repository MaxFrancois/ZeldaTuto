using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogosScreen : MonoBehaviour
{
    AsyncOperation loader;
    void Start()
    {
        loader = SceneManager.LoadSceneAsync("MainMenu");
        loader.allowSceneActivation = false;
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(6);
        loader.allowSceneActivation = true;
    }
}
