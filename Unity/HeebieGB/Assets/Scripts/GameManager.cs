using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

        }
    }
}
