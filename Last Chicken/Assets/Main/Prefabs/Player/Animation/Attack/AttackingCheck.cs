using UnityEngine;

public class AttackingCheck : StateMachineBehaviour
{
    public static float aniTime = 0;
    bool flag = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!Player.instance.inFluid)
            SoundManager.instance.PlayerAttack();
        flag = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniTime = stateInfo.normalizedTime;
        if(aniTime >= 0.9f && !flag)
        {
            flag = true;
            Player.instance.attackFlag = false;
        }

        MouseManager.instance.UpdateMouseCursor();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniTime = 0;
        if (!flag)
            Player.instance.attackFlag = false;
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