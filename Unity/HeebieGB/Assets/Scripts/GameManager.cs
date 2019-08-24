using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameObject Poweroff;

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

        if (InputManager.GetAnyButtonDown())
        {
            if (currentScene == ScreenChanger.IntroScene)
            {
                ScreenChanger.LoadNewScene(ScreenChanger.TitleScene);
            }
            else if (currentScene == ScreenChanger.TitleScene)
            {
                ScreenChanger.LoadFight();
            }
        }

        if (InputManager.PowerOff())
        {
            Poweroff.SetActive(true);
        }
    }
}
