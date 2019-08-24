using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumAttackType
{
    None = -1,
    DefUp, DefRight, DefDown, DefLeft,
    AtkEither, AtkA, AtkB, AtkNeither
};

public class CombatNote
{
    public static float loopLength = -1;

    EnumAttackType aType = EnumAttackType.None;
    Timer timer;
    float playTime = -1;
    bool failed = false;
    bool locked = false;

    public CombatNote(EnumAttackType type, float timeTill)
    {
        aType = type;
        timer = new Timer(timeTill); // ~~~ remove once verified redundant
        playTime = timeTill;
    }

    public bool IsSatisfied(EnumAttackType input)
    {
        if (aType == EnumAttackType.AtkEither)
        {
            if (input == EnumAttackType.AtkA || input == EnumAttackType.AtkB)
            {
                locked = true;
                return true;
            }
        }
        else if (aType == input)
        {
            locked = true;
            return true;
        }
        locked = true;
        failed = true;
        return false;
    }

    public void Reincarnate()
    {
        playTime += loopLength;
        locked = false;
        failed = false;
    }

    public void Lock()
    {
        failed = true;
    }

    public int Status
    {
        get
        {
            if (!locked)
            {
                return 0; // Undetermined
            }
            else
            {
                if (failed)
                {
                    return -1; // Failed timing
                }
                else
                {
                    return 1; // Succeeded timing
                }
            }
        }
    }

    public bool IsLocked { get { return failed; } }
    public EnumAttackType AtkType { get { return aType; } }
    public float PlayTime { get { return playTime; } }
}
