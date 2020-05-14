using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public CharStats _parent;

    private void Start()
    {
        //To do set up via code
        if (_parent == null)
            Debug.Log("CharStats is null for a collider");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check type?
        var damageable = other.GetComponent<Damageable>();
        if(damageable)
        {
            //Not self
            if(damageable.getGameObject() != _parent.gameObject)
            {
                damageable.damage(_parent.getAttack());
               // Debug.Log("HIT: " + other.gameObject);
            }
        }

    }
}
