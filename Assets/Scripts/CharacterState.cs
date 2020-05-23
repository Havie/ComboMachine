using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum eSTATE  {IDLE , STUN, MOVING, ATTACKING, TRANSITION};
    private bool _commmitted;
    private bool _inWindow;

    private int commitCount;

    [SerializeField] private eSTATE state;

    public void setState(eSTATE state) => this.state = state;
    public eSTATE getState() => state;


    public bool CanAttack()
    {
        return (state != eSTATE.STUN && _inWindow);
    }
    public bool CanMove()
    {
        return ( (state ==eSTATE.IDLE || state == eSTATE.MOVING) );
    }
    public bool CheckCommittment() => _commmitted;

    public void SetCommitted() { _commmitted = true;}
    public void ClearCommitted() { _commmitted = false;}
    public void OpenWindow() { _inWindow = true; }
    public void CloseWindow() { _inWindow = false; }
    public void SetAttacking() { state=eSTATE.ATTACKING; }
    public void SetIdle() { state = eSTATE.IDLE; }
    public void SetMoving() { state = eSTATE.MOVING; }
    public void SetStun() { state = eSTATE.STUN; }
    public void SetTransition() { state = eSTATE.TRANSITION; }
}
