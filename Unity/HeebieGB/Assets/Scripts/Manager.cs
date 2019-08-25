using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
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
        StartCoroutine(CombatCoordinator.Instance.BeginCombat("Slime"));
    }
}
