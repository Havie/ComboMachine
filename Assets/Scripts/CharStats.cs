using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    [SerializeField] private float _hp;
    [SerializeField] private float _hpMax;
    [SerializeField] private float _attack;
    [SerializeField] private float _defense;
    [SerializeField] private float _speed;


    // Start is called before the first frame update
    void Start()
    {
        //tell AC the speed
        var ac  = this.GetComponent<ActionCharacter>();
        if (ac)
            ac.setSpeed(_speed);
        else
            Debug.LogWarning("Cant find Action Character on " + this.gameObject);
    }

    public float getHp() => _hp;
    public float getHpMax() => _hpMax;
    public float getAttack() => _attack;
    public float getDefense() => _defense;
    public float getSpeed() => _speed;

    public void damage(float amount)
    {
        if (_hp - amount <= 0)
        {
            Die();
            _hp = 0;
        }
        else if (_hp - amount >= _hpMax)
            _hp = _hpMax;
        else
            _hp -= amount;
        
        //Tell UI


    }
   private void Die()
    {
        
    }
}
