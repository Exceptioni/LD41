using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy  {
    
    public int Health;
    public int MaxHealth;

    public GameEnemy GE;
    public virtual int BaseHealth { get { return 2; } }
    public virtual int Strength { get { return 1; } }

    public Enemy()
    {
        MaxHealth = BaseHealth;
        Health = BaseHealth;
    }

    public virtual float Speed
    {
        get { return 0.55f; }
    }

    public bool TakeDamage(int amt)
    {
        Health -= amt;
        if(Health <= 0)
        {
            GE.Die(true);
            return true;
        }
        return false;
    }
}
