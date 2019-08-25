using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum eGameState
    {
        Initial,
        Playing,
        Game_Over,
        Game_Win
    }


    GameObject Poweroff;
    public static eGameState GameState = eGameState.Initial;

    private void Awake()
    {
        Poweroff = GameObject.FindGameObjectWithTag("powerScreen");
        DontDestroyOnLoad(Poweroff);
        Poweroff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        string currentScene = ScreenChanger.GetActiveScene().name;

        if (GameState == eGameState.Game_Win && currentScene == ScreenChanger.TitleScene)
        {
            GameState = eGameState.Initial;
        }

        if (InputManager.GetAnyButtonDown())
        {
            if (currentScene == ScreenChanger.IntroScene || GameState == eGameState.Game_Over)
            {
                GameState = eGameState.Initial;
                ScreenChanger.LoadNewScene(ScreenChanger.TitleScene);
            }
            else if (currentScene == ScreenChanger.TitleScene)
            {
                GameState = eGameState.Playing;
                ScreenChanger.LoadFight();
            }
        }

        if (InputManager.PowerOff())
        {
            Poweroff.SetActive(true);
        }
    }

    public static void GameWin()
    {
        GameState = eGameState.Game_Win;
        Animator Game_Over_Anim = GameObject.FindGameObjectWithTag("GameOver")?.GetComponent<Animator>();
        Game_Over_Anim?.SetTrigger("Game_Win");
    }

    public static void GameOver()
    {
        GameState = eGameState.Game_Over;
        Animator Game_Over_Anim = GameObject.FindGameObjectWithTag("GameOver")?.GetComponent<Animator>();
        Game_Over_Anim?.SetTrigger("Game_Over");
    }
}
