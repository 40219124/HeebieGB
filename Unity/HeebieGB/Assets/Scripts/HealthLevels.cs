using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLevels
{
    #region singleton
    private static HealthLevels instance;
    public static HealthLevels Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HealthLevels();
            }
            return instance;
        }
    }
    private HealthLevels() { }
    #endregion

    private int totalPlayerHealth = 100;
    private int currentPlayerHealth = 100;
    private int totalEnemyHealth = 200;
    private int currentEnemyHealth = 200;

    public delegate void HealthUpdate(int current, int total);
    public HealthUpdate PlayerHealthUpdateCall;
    public HealthUpdate EnemyHealthUpdateCall;

    public delegate void CombatEnd();
    public CombatEnd CombatEndVictoryCall;
    public CombatEnd CombatEndDefeatCall;

    public void SetEnemyHealth(int val)
    {
        totalEnemyHealth = val;
        currentEnemyHealth = val;
    }

    public int CurrentEnemyHealth
    {
        get
        {
            return currentEnemyHealth;
        }
        set
        {
            currentEnemyHealth = Mathf.Clamp(value, 0, totalEnemyHealth);
            // ~~~ display health
            EnemyHealthUpdateCall?.Invoke(currentEnemyHealth, totalEnemyHealth);
            if (currentEnemyHealth == 0)
            {
                // ~~~ win combat
                CombatEndVictoryCall?.Invoke();
            }
        }
    }

    public int CurrentPlayerHealth
    {
        get
        {
            return currentPlayerHealth;
        }
        set
        {
            currentPlayerHealth = Mathf.Clamp(value, 0, totalPlayerHealth);
            // ~~~ display health
            PlayerHealthUpdateCall?.Invoke(currentPlayerHealth, totalPlayerHealth);
            if (currentPlayerHealth == 0)
            {
                // ~~~ lose combat
                CombatEndDefeatCall?.Invoke();
            }
        }
    }

    public float CurrentEnemyFraction { get { return (float)currentEnemyHealth / totalEnemyHealth; } }
    public float CurrentPlayerFraction { get { return (float)currentPlayerHealth / totalPlayerHealth; } }

    private void GetCombatInfo(EnumAttackType action, bool successful)
    {
        if (successful)
        {
            if (action == EnumAttackType.AtkA || action == EnumAttackType.AtkB || action == EnumAttackType.AtkEither)
            {
                CurrentEnemyHealth -= 10;
            }
        }
        else
        {
            if (action == EnumAttackType.DefUp || action == EnumAttackType.DefLeft || action == EnumAttackType.DefDown || action == EnumAttackType.DefRight)
            {
                CurrentPlayerHealth -= 15;
            }
        }
    }

    public void PostCombatRefresh()
    {
        currentPlayerHealth = totalPlayerHealth;
        currentEnemyHealth = totalEnemyHealth;
    }

    public void ClearDelegates()
    {
        PlayerHealthUpdateCall = null;
        EnemyHealthUpdateCall = null;
        CombatEndVictoryCall = null;
        CombatEndDefeatCall = null;
    }

    public void FillDelegates()
    {
        CombatCoordinator.Instance.CombatInfoCall += GetCombatInfo;
        CombatCoordinator.Instance.CombatOverCall += PostCombatRefresh;
    }
}
