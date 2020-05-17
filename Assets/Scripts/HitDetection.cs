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
        //Check type
        var damageable = other.GetComponent<Damageable>();
        if(damageable)
        {
            //Not self
            if(damageable.getGameObject() != _parent.gameObject)
            {
                //Calculate direction
                Vector3 direction = (  _parent.gameObject.transform.position - damageable.getGameObject().transform.position).normalized;
                float dot = Vector3.Dot( damageable.getGameObject().transform.forward, direction);

                Damageable.HitDirection dir = Damageable.HitDirection.Default;

                if (dot > 0 && dot < 0.5f)
                    dir = Damageable.HitDirection.LEFT;
                else if (dot > 0 && dot > 0.5f)
                    dir = Damageable.HitDirection.FRONT;
                else if (dot < 0 && dot < -0.5f)
                    dir = Damageable.HitDirection.BACK;
                else
                    dir = Damageable.HitDirection.RIGHT;

                damageable.damage(_parent.getAttack(), dir);
            }
        }

    }
}
