using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharStats : MonoBehaviour
{
    public UIOnChar ui;
    public Animator animator;

    [SerializeField] private float _hp;
    [SerializeField] private float _hpMax;
    [SerializeField] private float _attack;
    [SerializeField] private float _defense;
    [SerializeField] private float _speed;


    // Start is called before the first frame update
    void Start()
    {
        IniSpeed();
        IniAnimator();
        //To-Do: set UI if null


    }
    private void IniSpeed()
    {
        //tell AC the speed
        var ac = this.GetComponent<ActionCharacter>();
        if (ac)
            ac.setSpeed(_speed);
        else
            Debug.LogWarning("Cant find Action Character on " + this.gameObject);
    }
    private void IniAnimator()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Cant Find Animator for " +this.gameObject);
    }

    public float getHp() => _hp;
    public float getHpMax() => _hpMax;
    public float getAttack() => _attack;
    public float getDefense() => _defense;
    public float getSpeed() => _speed;

    public void damage(float amount, Damageable.HitDirection dir)
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

        if (amount > 0) // not being healed To=Do: some other threshold
        {
            //Tell UI
            if (ui)
                ui.PlayDamage(amount);

            //tell the Animator;
            AnimatorDamage(dir);
        }
    }
   private void AnimatorDamage(Damageable.HitDirection dir)
    {
        //System.Random rand = new System.Random();
       // int choice = rand.Next(1, 3);
        if (animator)
            animator.SetInteger("hit", (int)dir);
    }

    private void Die()
    {
        
    }
}
