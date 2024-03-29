﻿using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour, IDamageable 
{
    public float initialHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = initialHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDmg(damage);
    }
	
    public void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

    public void TakeDmg(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }
}
