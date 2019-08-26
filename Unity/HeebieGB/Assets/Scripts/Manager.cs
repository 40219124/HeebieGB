using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    bool done = false;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > 1 && !done)
        {
            StartCombat();
            done = true;
        }
        Timer.UpdateTimers(Time.deltaTime);
    }

    public void StartCombat()
    {
        ClearDelegates();
        FillDelegates();
        StartCoroutine(CombatCoordinator.Instance.BeginCombat("Slime"));
    }

    private void ClearDelegates()
    {
        HealthLevels.Instance.ClearDelegates();
        CombatCoordinator.Instance.ClearDelegates();
    }

    private void FillDelegates()
    {
        HealthLevels.Instance.FillDelegates();
        CombatCoordinator.Instance.FillDelegates();
        CombatUIHandler.Instance.FillDelegates();
    }
}
