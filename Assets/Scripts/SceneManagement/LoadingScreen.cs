using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;

    [SerializeField]
    private float splashTimerSeconds = 2.0f;

    // private ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        // progressBar = root.Q<ProgressBar>("ProgressBarLoading");
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(splashTimerSeconds);

        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        while (!loadLevel.isDone)
        {
            // progressBar.value = Mathf.Clamp01(loadLevel.progress / 0.9f) * 100;
            yield return null;
        }
    }
}
