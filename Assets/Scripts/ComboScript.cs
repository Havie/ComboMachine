using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboScript : StateMachineBehaviour
{
    public bool isIdleState;
    [SerializeField] private float _comboTime;
    [SerializeField] private float _comboTimeMax = 2;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _comboTime = Time.time;
        _comboTimeMax = Time.time + _comboTimeMax;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!isIdleState)
             _comboTime += Time.deltaTime;

        if (_comboTime < _comboTimeMax)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //move to next state;
                animator.SetTrigger("Normal");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //move to next state;
                animator.SetTrigger("Strong");
            }
        }
        else 
        {
            //return to default state;
            animator.SetTrigger("Return");
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
