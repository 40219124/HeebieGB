using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    //Scene names so we can't type them wrong in random places
    public static string IntroScene = "Intro";
    public static string TitleScene = "Start";
    public static string FightScene = "Game";
    public static string FightHudScene = "HUD";
    public static string EngineScene = "BeatEngine";
    public static string TDScene = "TopDown";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadNewScreens(List<string> screenNames)
    {
        foreach (string screenName in screenNames)
        {
            SceneManager.LoadScene(screenName, LoadSceneMode.Additive);
        }
    }

    public static void LoadNewScreens(string screenName)
    {
        List<string> screenNames = new List<string>() { screenName };
        LoadNewScreens(screenNames);
    }

    public static void LoadNewScreenAsActive(string screenName)
    {
        LoadNewScreens(screenName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(screenName));

    }
    public static bool IsSceneLoaded(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    public static void LoadFight()
    {
        if (!SceneManager.GetSceneByName(FightScene).isLoaded)
        {
            LoadNewScreens(FightScene);
        }
        if (!SceneManager.GetSceneByName(FightHudScene).isLoaded)
        {
            LoadNewScreens(FightHudScene);
        }
        if (!SceneManager.GetSceneByName(EngineScene).isLoaded)
        {
            LoadNewScreens(EngineScene);
        }
    }

    public static IEnumerator<YieldInstruction> SetActiveWhenLoaded(string sceneName)
    {
        while (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public static void UnloadFight()
    {
        if (SceneManager.GetSceneByName(FightScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(FightScene);
        }
        if (SceneManager.GetSceneByName(FightHudScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(FightHudScene);
        }
        if (SceneManager.GetSceneByName(EngineScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(EngineScene);
        }
        if (SceneManager.GetSceneByName(TDScene).isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(TDScene));
        }
    }

    public static Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

}
