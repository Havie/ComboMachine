using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public CharStats me;

    // Start is called before the first frame update
    void Start()
    {
        //Should write a way to get stats if null once figured out hierarchy
    }
    public GameObject getGameObject()
    {
        if (me)
            return me.gameObject;

        return null;
    }
    public void damage(float dmg)
    {
        print(dmg + " Damage to : " + getGameObject());

        if (me)
            me.damage(dmg);
            
    }

    
}
