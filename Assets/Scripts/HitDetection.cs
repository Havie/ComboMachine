using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public ActionCharacter _parent;

    private void Start()
    {
        if (_parent == null)
            Debug.Log("Parent is null for a collider");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check type?

        //Apply Damage from _parent?

        Debug.Log("HIT: " +other.gameObject);
    }
}
