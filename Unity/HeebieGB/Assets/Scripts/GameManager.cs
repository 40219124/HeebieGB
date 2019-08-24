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
        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(2))
                return; //Do Nothing
            if (ScreenChanger.GetActiveScene().name == ScreenChanger.IntroScene)
            {
                ScreenChanger.LoadNewScene(ScreenChanger.TitleScene);
            }
            else if (ScreenChanger.GetActiveScene().name == ScreenChanger.TitleScene)
            {
                ScreenChanger.LoadFight();
            }
        }
    }
}
