using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] GameObject fadeToPanel;
    [SerializeField] float fadeDuration;

    [Header("Buttons")]
    [SerializeField] Button continueButton;
    [SerializeField] TextMeshProUGUI continueButtonText;

    [Header("Orbs")]
    [SerializeField] GameObject orbsRotationCenter;
    [SerializeField] float rotationSpeed;
    [SerializeField] List<GameObject> orbs;


    void Start()
    {
        if (true) //has saved game
        {
            continueButton.interactable = false;
            continueButtonText.color = new Color(0.4f,0.4f,0.4f);
        }
        for(int i = 0; i < orbs.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(orbsRotationCenter.transform.position.x, orbsRotationCenter.transform.position.y + -100, 0);
            var instance = Instantiate(orbs[i], spawnPosition, Quaternion.identity, orbsRotationCenter.transform);
            instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            orbsRotationCenter.transform.Rotate(Vector3.back, 360 / orbs.Count);
        }
    }

    void Update()
    {
        orbsRotationCenter.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        if (orbsRotationCenter.transform.rotation.z <= -360)
            orbsRotationCenter.transform.rotation = new Quaternion(50, 0, 0, 0);
    }

    public void NewGame()
    {
        StartCoroutine(FadeToBlack());
    }

    public void Continue()
    {
        Debug.Log("Continue game");
    }

    public void SettingsMenu()
    {

    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public IEnumerator FadeToBlack()
    {
        if (fadeToPanel != null)
        {
            Instantiate(fadeToPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeDuration);
        var asyncOperation = SceneManager.LoadSceneAsync("Dungeon");
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
