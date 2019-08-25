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

    public static bool UpArrowHeld = false;
    public static bool DownArrowHeld = false;
    public static bool LeftArrowHeld = false;
    public static bool RightArrowHeld = false;
    public static bool AButtonHeld = false;
    public static bool BButtonHeld = false;
    public static bool StartHeld = false;
    public static bool SelectHeld = false;

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

        if (Input.GetKey(KeyCode.X) && !AButton)
        {
            AButtonHeld = true;
        }
        else
        {
            AButtonHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BButton = true;
        }
        else
        {
            BButton = false;
        }

        if (Input.GetKey(KeyCode.A) && !BButton)
        {
            BButtonHeld = true;
        }
        else
        {
            BButtonHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrow = true;
        }
        else
        {
            UpArrow = false;
        }

        if (Input.GetKey(KeyCode.UpArrow) && !UpArrow)
        {
            UpArrowHeld = true;
        }
        else
        {
            UpArrowHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownArrow = true;
        }
        else
        {
            DownArrow = false;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !DownArrow)
        {
            DownArrowHeld = true;
        }
        else
        {
            DownArrowHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftArrow = true;
        }
        else
        {
            LeftArrow = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !LeftArrow)
        {
            LeftArrowHeld = true;
        }
        else
        {
            LeftArrowHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightArrow = true;
        }
        else
        {
            RightArrow = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !RightArrow)
        {
            RightArrowHeld = true;
        }
        else
        {
            RightArrowHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Start = true;
        }
        else
        {
            Start = false;
        }

        if (Input.GetKey(KeyCode.Return) && !Start)
        {
            StartHeld = true;
        }
        else
        {
            StartHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Select = true;
        }
        else
        {
            Select = false;
        }

        if (Input.GetKey(KeyCode.RightShift) && !Select)
        {
            SelectHeld = true;
        }
        else
        {
            SelectHeld = false;
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
