using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    [SerializeField] private string initialScene = "MainMenu";
    private List<string> loadedScenes = new List<string>();

    public static event Action<string> OnSceneLoadStart;
    public static event Action<string> OnSceneLoadComplete;
    public static event Action<string> OnSceneUnloadStart;
    public static event Action<string> OnSceneUnloadComplete;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSceneSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSceneSystem()
    {
        if (!SceneManager.GetSceneByName(initialScene).isLoaded)
        {
            LoadSceneAsync(initialScene, true);
        }
    }

    public void LoadSceneAsync(string sceneName, bool setAsActive = false)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName, setAsActive));
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, bool setAsActive)
    {
        OnSceneLoadStart?.Invoke(sceneName);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        loadedScenes.Add(sceneName);
        if (setAsActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        OnSceneLoadComplete?.Invoke(sceneName);
    }

    public void UnloadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            StartCoroutine(UnloadSceneCoroutine(sceneName));
        }
    }

    private IEnumerator UnloadSceneCoroutine(string sceneName)
    {
        OnSceneUnloadStart?.Invoke(sceneName);

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        loadedScenes.Remove(sceneName);
        OnSceneUnloadComplete?.Invoke(sceneName);
    }

    public void SetActiveScene(string sceneName)
    {
        if (loadedScenes.Contains(sceneName))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }
}