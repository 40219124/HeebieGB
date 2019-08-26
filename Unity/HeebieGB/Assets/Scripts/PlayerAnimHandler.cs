using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    #region singleton
    private PlayerAnimHandler() { }
    private static PlayerAnimHandler instance = null;
    public static PlayerAnimHandler Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private Animator animator;

    private float noteLength = -1;
    private string lastBool = "Idle";


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator?.SetBool("Idle", true);
    }

    public void SetAnimationSpeed(float speed)
    {
        animator?.SetFloat("BeatLength", 1 / speed);
        noteLength = speed;
    }

    public void FillDelegates()
    {
        CombatCoordinator.Instance.AnimationSpeedCall += SetAnimationSpeed;
        CombatCoordinator.Instance.CombatInfoCall += GetCombatInfo;
        CombatCoordinator.Instance.CombatOverCall += CombatOver;
    }

    private void GetCombatInfo(EnumAttackType action, bool successful)
    {
        bool change = false;
        if ((action == EnumAttackType.DefLeft || action == EnumAttackType.DefUp) && !successful)
        {
            SetBoolTrue("Hit");
            change = true;
        }
        else if (action == EnumAttackType.DefLeft && successful)
        {
            SetBoolTrue("Dodge_Back");
            change = true;
        }
        else if (action == EnumAttackType.DefUp && successful)
        {
            SetBoolTrue("Dodge_Up");
            change = true;
        }
        else if (action == EnumAttackType.AtkA)
        {
            SetBoolTrue("Attack_Fwd");
            change = true;
        }
        else if (action == EnumAttackType.AtkB)
        {
            SetBoolTrue("Attack_Dwn");
            change = true;
        }

        if (change)
        {
            SetBoolTrue("Idle");
            StartCoroutine(BackToIdle());
        }
    }

    private void SetBoolTrue(string name)
    {
        animator?.SetBool(name, true);
        lastBool = name;
    }

    IEnumerator BackToIdle()
    {
        yield return new WaitForSeconds(noteLength / 1.0f);
        animator?.SetBool(lastBool, false);
        animator?.SetTrigger("Idle");
        animator?.Play("player_idle", -1, (CombatCoordinator.Instance.SongProgress % noteLength) / noteLength);
        lastBool = "Idle";
    }

    private void CombatOver()
    {
        if (HealthLevels.Instance.CurrentPlayerHealth == 0)
        {
            SetBoolTrue("Dead");
        }
    }

}
