using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public int Health {get; protected set;}

    protected virtual void Start()
    {
        Health = 100;
    }

    public virtual void ReceiveDamage(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
        Destroy(this.gameObject);
    }
}
