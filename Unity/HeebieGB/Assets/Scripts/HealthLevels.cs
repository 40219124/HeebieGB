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
    private HealthLevels() { CombatCoordinator.Instance.CombatInfoCall += GetCombatInfo; }
    #endregion

    private int totalPlayerHealth = 100;
    private int currentPlayerHealth = 100;
    private int totalEnemyHealth = 200;
    private int currentEnemyHealth = 200;

    public delegate void PlayerHealthUpdate(int health);
    public PlayerHealthUpdate PlayerHealthUpdateCall;
    
    public delegate void EnemyHealthUpdate(int health);
    public EnemyHealthUpdate EnemyHealthUpdateCall;

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
            if (currentEnemyHealth == 0)
            {
                // ~~~ win combat
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
            if (currentPlayerHealth == 0)
            {
                // ~~~ lose combat
            }
        }
    }

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
}
