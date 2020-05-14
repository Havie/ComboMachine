using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboScript : StateMachineBehaviour
{
    public bool isIdleState;
    public bool attackAnim;
    [SerializeField] private float _comboTime;
    [SerializeField] private float _comboTimeMax = 2;
    [SerializeField] private float _comboTimeWindow = 2f;
    [SerializeField] private float _comboTimeStartDelay = 0;
    private bool inputRead;
    private ActionCharacter ac;

    public bool test;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isIdleState)
            animator.SetInteger("combo", 0);

        ac = animator.GetComponent<ActionCharacter>();
        if(ac && attackAnim)
        {
            ac.lockMovement();
        }

        float debugtime = _comboTimeMax;

        _comboTime = Time.time + _comboTimeStartDelay;
        _comboTimeMax = Time.time + _comboTimeWindow;
        inputRead = false;

        if (test)
        {
            Debug.Log("-----OnStateEnter------");
            Debug.Log("ENTERED @" + _comboTime);
            Debug.Log("MAX of " + _comboTime + "  +" + _comboTimeWindow + "  =" + _comboTimeMax);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!isIdleState)
             _comboTime += Time.deltaTime;

        if (_comboTime < _comboTimeMax)
        {
            if (Input.GetMouseButtonDown(0) ||(Input.GetKeyUp(KeyCode.JoystickButton2)))
                {
                //move to next state;
                if (!inputRead)
                {
                    animator.SetInteger("combo", animator.GetInteger("combo") + 1);
                    inputRead = true;
                }
            }
            else if (Input.GetMouseButtonDown(1) || (Input.GetKeyUp(KeyCode.JoystickButton3)))
            {
                //move to next state;
                if (!inputRead)
                {

                    animator.SetInteger("combo", animator.GetInteger("combo") + 10);
                    inputRead = true;
                }
            }
        }
        else if(!inputRead)
        {
            if (test)
            {
                Debug.Log("reset combo" +Time.time);
                test = false;
            }
            //return to default state;
            animator.SetInteger("combo", 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ac && !isIdleState)
            ac.unlockMovement();
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
