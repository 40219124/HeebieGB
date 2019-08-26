using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBeat : MonoBehaviour
{
    public List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();
    public RuntimeAnimatorController failController;
    private EnumAttackType currentAnimator = EnumAttackType.None;

    private Animator animator;
    private bool locked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSprite(CombatNote note)
    {
        if (!locked)
        {
            if (currentAnimator != note.AtkType)
            {
                if (controllers[(int)note.AtkType] != null)
                {
                    currentAnimator = note.AtkType;
                    animator.runtimeAnimatorController = controllers[(int)note.AtkType];
                }
            }
            if (note.Status != 0)
            {
                locked = true;
                if (note.Status == 1) // Correct hit
                {
                    animator.SetBool("Pressed", true);
                }
                else
                {
                    animator.runtimeAnimatorController = failController;
                }
            }
        }
        if (locked)
        {
            if (CombatCoordinator.Instance.SongProgress > note.PlayTime && note.Status == 1 && animator.GetBool("Pop") != true)
            {
                animator.SetBool("Pop", true);
            }
            else if (note.Status == -1 && animator.GetInteger("State") == 0)
            {
                if (note.AtkType.ToString().Contains("Def"))
                {
                    animator.SetInteger("State", 1);
                }
                else if (note.AtkType.ToString().Contains("Atk"))
                {
                    animator.SetInteger("State", 2);
                }
            }
        }
    }

    public void CleanUI()
    {
        locked = false;
        animator.runtimeAnimatorController = null;
        currentAnimator = EnumAttackType.None;
        transform.localScale = Vector3.one;
        transform.position = -10 * Vector3.one;
    }
}
