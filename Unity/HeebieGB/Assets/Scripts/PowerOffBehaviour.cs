using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOffBehaviour : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL("https://elfqueen.itch.io/codegame");
#else
            Application.Quit();
#endif
    }
}
