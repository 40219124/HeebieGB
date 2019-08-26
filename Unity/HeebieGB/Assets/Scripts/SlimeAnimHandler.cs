using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimHandler : MonoBehaviour
{
    #region singleton
    private SlimeAnimHandler() { }
    private static SlimeAnimHandler instance = null;
    public static SlimeAnimHandler Instance { get { return instance; } }
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
        animator?.SetFloat("BeatLength", 1/speed);
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
        if((action == EnumAttackType.AtkA || action == EnumAttackType.AtkB) && !successful)
        {
            SetBoolTrue("Dodge");
            change = true;
        }
        else if (action == EnumAttackType.AtkA && successful)
        {
            SetBoolTrue("Hit_Fwd");
            change = true;
        }
        else if (action == EnumAttackType.AtkB && successful)
        {
            SetBoolTrue("Hit_Dwn");
            change = true;
        }
        else if (action == EnumAttackType.DefLeft)
        {
            SetBoolTrue("Attack_Fwd");
            change = true;
        }
        else if (action == EnumAttackType.DefUp)
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
        animator?.SetBool("Idle", true);
        animator?.Play("slime_idle", -1, (CombatCoordinator.Instance.SongProgress % noteLength) / noteLength);
        lastBool = "Idle";
    }

    private void CombatOver()
    {
        if(HealthLevels.Instance.CurrentEnemyHealth == 0)
        {
            SetBoolTrue("Dead");
        }
    }
}
