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
    float playTime = -1;
    float autoFailTime = -1;
    bool failed = false;
    bool locked = false;
    float lockTime = -1;
    bool resolved = false;

    public CombatNote(EnumAttackType type, float timeTill)
    {
        aType = type;
        playTime = timeTill;
        autoFailTime = playTime + CombatDecoder.Instance.beatLength * 0.15f;
    }

    public bool IsSatisfied(EnumAttackType input, float time)
    {
        if (!locked)
        {
            failed = true;
            if (Mathf.Abs(playTime - time) < CombatDecoder.Instance.beatLength * 0.15)
            {
                if (aType == EnumAttackType.AtkEither)
                {
                    if (input == EnumAttackType.AtkA || input == EnumAttackType.AtkB)
                    {
                        failed = false;
                    }
                }
                else if (aType == input)
                {

                    failed = false;
                }
            }
            locked = true;
            lockTime = time;
        }

        Debug.Log($"Satisfaction: {(!failed).ToString()}");
        return !failed;
    }

    public void Reincarnate()
    {
        playTime += loopLength;
        autoFailTime += loopLength;
        lockTime = -1;
        locked = false;
        failed = false;
        resolved = false;
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

    public bool TimeOutCheck(float time)
    {
        if (time > autoFailTime && !locked)
        {
            failed = true;
            locked = true;
            lockTime = time;
        }
        return failed;
    }

    public EnumAttackType AtkType { get { return aType; } }
    public bool Failed { get { return failed; } }
    public bool IsLocked { get { return locked; } }
    public float LockTime { get { return lockTime; } }
    public float PlayTime { get { return playTime; } }
    public float AutoFailTime { get { return autoFailTime; } }
    public bool Resolved { get { return resolved; } set { resolved = value; } }
}
