using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboScript : StateMachineBehaviour
{
    public bool isIdleState;
    public bool attackAnim;
    private float _comboTimeStart;
    private float _comboTimeMax;
    private float _comboTimeWindow;
    private bool inputRead;
    private ActionCharacter ac;

    public bool test;
    public bool test2;

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
        _comboTimeWindow = stateInfo.length - (stateInfo.length/4);

        _comboTimeStart = Time.time ;
        _comboTimeMax = Time.time + _comboTimeWindow;
        inputRead = false;

        if (test)
        {
            Debug.Log("-----OnStateEnter------");
            Debug.Log("ENTERED @" + _comboTimeStart);
            Debug.Log("MAX of " + _comboTimeStart + "  +" + _comboTimeWindow + "  =" + _comboTimeMax);
            Debug.Log("Window = " + _comboTimeWindow);
            Debug.Log("StaryDelay = " + (_comboTimeStart + _comboTimeWindow /3));
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        //Seems to create a nice window, you have 3/8ths to 3/4th of the anim time to read input
        if ((isIdleState) || (Time.time < _comboTimeMax && Time.time >= (_comboTimeStart + _comboTimeWindow/2 )))
        {
            CheckAttack(animator);
        }
        else if(Time.time >= _comboTimeMax &&  !inputRead)
        {
            if (test2)
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
        {
            ac.unlockMovement();
            ac.UnCommitToAttack();
            if (test)
                Debug.Log("Final Exit @ " + Time.time);
        }
    }

    private void CheckAttack(Animator animator)
    {
        if (Input.GetMouseButtonDown(0) || (Input.GetKeyUp(KeyCode.JoystickButton2)))
        {
            CommitedAttack(animator, 1);
        }
        else if (Input.GetMouseButtonDown(1) || (Input.GetKeyUp(KeyCode.JoystickButton3)))
        {
            CommitedAttack(animator, 10);
        }
    }
    private void CommitedAttack(Animator animator, int combo)
    {
        //move to next state;
        if (!inputRead)
        {
            animator.SetInteger("combo", animator.GetInteger("combo") + combo);
            UIDirectionPressed._instance.UpdateText((animator.GetInteger("combo").ToString()));
            inputRead = true;
            //Tell the movement read we've commited to an attack and not to change direction
            if (ac)
                ac.CommitToAttack();
        }

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
