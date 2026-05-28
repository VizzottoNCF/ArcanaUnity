using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BootstrapLoader
{
    private const string BootstrapScene = "Bootstrap";
    private const string MainMenuScene = "MainMenu";
    private const string CutsceneScene = "Cutscene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnsureBootstrapLoaded(SceneManager.GetActiveScene());
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureBootstrapLoaded(scene);
    }

    private static void EnsureBootstrapLoaded(Scene scene)
    {
        if (scene.name == MainMenuScene || scene.name == CutsceneScene) { return; }
        

        if (!SceneManager.GetSceneByName(BootstrapScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(BootstrapScene, LoadSceneMode.Additive);
        }
        
    }
}
