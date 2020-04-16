using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryingCheck : StateMachineBehaviour
{
    public static float aniTime = 0;

    public bool Coco = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Coco)
        {
            SoundManager.instance.ChickenCoco(true);
        }
        else
        {
            int[] bark = new int[] { 0, 1, 2, 3, 5, 6, 7 };
            SoundManager.instance.ChickenBark(bark[Random.Range(0, bark.Length)]);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniTime = stateInfo.normalizedTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniTime = 0;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
