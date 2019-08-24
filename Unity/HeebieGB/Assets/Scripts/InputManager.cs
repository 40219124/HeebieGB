using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool UpArrow = false;
    public static bool DownArrow = false;
    public static bool LeftArrow = false;
    public static bool RightArrow = false;
    public static bool AButton = false;
    public static bool BButton = false;
    public static bool Start = false;
    public static bool Select = false;
    static float powerOffTime = 0;
    const float waitTime = 1.0f;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AButton = true;
        }
        else
        {
            AButton = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BButton = true;
        }
        else
        {
            BButton = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrow = true;
        }
        else
        {
            UpArrow = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownArrow = true;
        }
        else
        {
            DownArrow = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftArrow = true;
        }
        else
        {
            LeftArrow = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightArrow = true;
        }
        else
        {
            RightArrow = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Start = true;
        }
        else
        {
            Start = false;
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Select = true;
        }
        else
        {
            Select = false;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            powerOffTime += Time.deltaTime;
        }
        else
        {
            powerOffTime = 0;
        }
    }

    public static bool GetAnyButtonDown()
    {
        return UpArrow || DownArrow || LeftArrow || RightArrow || RightArrow || AButton || BButton || Start || Select;
    }

    public static bool PowerOff()
    {
        if (powerOffTime >= waitTime)
        {
            return true;
        }
        return false;
    }
}
