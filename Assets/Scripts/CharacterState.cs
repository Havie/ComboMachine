using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum eSTATE  {IDLE , STUN, MOVING, ATTACKING, TRANSITION};
    [SerializeField] private eSTATE state;

    public void setState(eSTATE state) => this.state = state;
    public eSTATE getState() => state;


    public bool CanAttack()
    {
        return (state != eSTATE.STUN && state != eSTATE.TRANSITION);
    }
    public bool CanMove()
    {
        return (state ==eSTATE.IDLE || state == eSTATE.MOVING );
    }
}
