using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomStateMachineBehavior : StateMachineBehaviour
{
    #region Variables

    public int numberOfStates = 3;
    public float minNormTime = 0f;
    public float maxNormTime = 5f;

    public float randomNormTime;

    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");
    int random;

    #endregion Variables
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Decide transition time Randomly
        randomNormTime = Random.Range(minNormTime, maxNormTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If state is not in Idle, set parameter to -1
        if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger(hashRandomIdle, -1);
        }

        // Transition
        if (stateInfo.normalizedTime > randomNormTime && !animator.IsInTransition(0))
        {
            random = Random.Range(0, numberOfStates);
            Debug.Log("random: " + random);
            animator.SetInteger(hashRandomIdle,random);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
