using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryChecker : MonoBehaviour
{
    public static int EnemiesLeft;

    // Start is called before the first frame update
    void Awake()
    {
        EnemiesLeft = GameObject.FindGameObjectsWithTag("Monster").Length;
    }
}
