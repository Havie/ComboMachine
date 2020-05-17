using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public CharStats me;
    private Animator animator;

    public enum HitDirection { Default, RIGHT, FRONT, LEFT, BACK };

    // Start is called before the first frame update
    void Start()
    {
        //Should write a way to get stats if null once figured out hierarchy
        if(me==null)
        {
            //To-Do:
        }

        SetUpAnimator();
    }
    private void SetUpAnimator()
    {
        if (me)
            animator = me.GetComponent<Animator>();
    }
    public GameObject getGameObject()
    {
        if (me)
            return me.gameObject;

        return null;
    }
    public void damage(float dmg, HitDirection dir)
    {
        //print(dmg + " Damage to : " + getGameObject());
        if (me)
            me.damage(dmg, dir);

    }


    
}
